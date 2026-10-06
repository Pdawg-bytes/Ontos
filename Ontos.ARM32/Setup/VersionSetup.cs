using System;
using System.Linq;
using System.Text;
using Ontos.Core.ARM32;
using Ontos.ARM32.Databases;
using System.Collections.Generic;
using Ontos.Generators.Core.Collections;

namespace Ontos.ARM32.Setup;

internal sealed record Trait(InstructionSet Set, string Group, EquatableArray<uint> Values)
{
    internal GroupInfo Info => GroupCatalog.Find(Set, Group);
    internal string Name    => $"{Info.MethodName}_{string.Join("_", Values)}";
}

internal sealed record LUT(EquatableArray<string> Handlers, EquatableArray<ushort> Map);
internal sealed record VersionOutput(ARMVersion Version, LUT ARM, EquatableArray<string>? ARMUnconditional, LUT? Thumb, EquatableArray<Trait> Traits);

internal readonly record struct InstructionPair(Instruction Definition, GroupInfo Group);

internal static class VersionSetup
{
    private const uint CondNibble = 0xF0000000;

    internal static VersionOutput Build(ARMVersion version)
    {
        HashSet<Trait> traits = [];

        LUT arm                        = BuildLUT(InstructionSet.ARM, version, traits);
        EquatableArray<string>? uncond = version >= ARMVersion.V5  ? BuildUnconditional(version, traits)             : null;
        LUT? thumb                     = version >= ARMVersion.V4T ? BuildLUT(InstructionSet.Thumb, version, traits) : null;

        return new VersionOutput(version, arm, uncond, thumb, traits.OrderBy(t => t.Name, StringComparer.Ordinal).ToEquatableArray());
    }

    private static int PopCount(uint v)
    {
        int n = 0;
        for (; v != 0; v &= v - 1) n++;
        return n;
    }

    private static uint ToKey(InstructionSet set, uint word) => set == InstructionSet.ARM
        ? ((word & 0x0FF00000) >> 16) | ((word & 0x000000F0) >> 4)
        : (word & 0xFFC0) >> 6;

    private static uint KeyToOpcode(InstructionSet set, int key) => set == InstructionSet.ARM
        ? (((uint)key >> 4) & 0xFF) << 20 | ((uint)key & 0xF) << 4
        : (uint)key << 6;

    private static InstructionPair[] GetInstructions(InstructionSet set, ARMVersion version) =>
        [..GroupCatalog.GroupsFor(set, version)
            .SelectMany(g => g.Instructions.Select(i => new InstructionPair(i, g)))
            .OrderByDescending(i => PopCount(i.Definition.Mask))
            .ThenByDescending(i  => PopCount(i.Definition.Expected))];

    private static bool IsUnconditional(InstructionPair i, ARMVersion version)
    {
        if ((i.Definition.Expected & CondNibble) == CondNibble)
            return true;

        var rule = GroupCatalog.RuleFor(i.Group.Override);

        return rule is { Parameter: null } && version >= rule.Since;
    }

    private static LUT BuildLUT(InstructionSet set, ARMVersion version, HashSet<Trait> traits)
    {
        bool arm = set == InstructionSet.ARM;
        int size = arm ? 4096 : 1024;

        InstructionPair[] instructions = [..GetInstructions(set, version).Where(i => !arm || (i.Definition.Expected & CondNibble) != CondNibble)];

        var keyed = instructions
            .Select(i => (Pair: i, Mask: ToKey(set, i.Definition.Mask), Expected: ToKey(set, i.Definition.Expected)))
            .ToArray();

        string[] lut = new string[size];

        for (int key = 0; key < size; key++)
        {
            var matches = keyed.Where(k => ((uint)key & k.Mask) == k.Expected).ToArray();

            if (matches.Length == 0)
            {
                lut[key] = Lambda(Call(set, null, null));
                continue;
            }

            lut[key] = Lambda(ResolveMatches(set, [.. matches.Select(m => m.Pair)], key, version, traits));
        }

        return Compress(lut);
    }

    private static LUT Compress(string[] lut)
    {
        Dictionary<string, ushort> ids = new(StringComparer.Ordinal);
        List<string> handlers          = [];
        ushort[] map                   = new ushort[lut.Length];

        for (int i = 0; i < lut.Length; i++)
        {
            if (!ids.TryGetValue(lut[i], out ushort id))
            {
                id = (ushort)handlers.Count;
                ids.Add(lut[i], id);
                handlers.Add(lut[i]);
            }

            map[i] = id;
        }

        return new LUT(handlers.ToEquatableArray(), map.ToEquatableArray());
    }

    private static EquatableArray<string> BuildUnconditional(ARMVersion version, HashSet<Trait> traits)
    {
        InstructionPair[] instructions = [..GetInstructions(InstructionSet.ARM, version).Where(i => IsUnconditional(i, version))];
        List<string> lines             = [];

        foreach (var (def, g) in instructions)
        {
            uint expected  = def.Expected & def.Mask;
            uint templated = MaskFromRanges(g.Templated.SelectMany(p => p.Ranges).Select(r => (r.Hi, r.Lo)));
            uint free      = templated & ~def.Mask;
            string test    = $"(op & 0x{def.Mask:X8}) == 0x{expected:X8}";

            if (free == 0)
            {
                lines.Add($"if ({test}) {{ {Resolve(InstructionSet.ARM, g, expected, version, traits, "this")}; return; }}");
                continue;
            }

            lines.Add($"if ({test})");
            lines.Add("{");
            lines.Add($"    switch (op & 0x{free:X8})");
            lines.Add("    {");

            uint sub = 0;
            do
            {
                lines.Add($"        case 0x{sub:X8}: {Resolve(InstructionSet.ARM, g, expected | sub, version, traits, "this")}; return;");
                sub = (sub - free) & free;
            }
            while (sub != 0);

            lines.Add("    }");
            lines.Add("}");
        }

        return lines.ToEquatableArray();
    }

    private static string ResolveMatches(InstructionSet set, InstructionPair[] matches, int key, ARMVersion version, HashSet<Trait> traits)
    {
        uint lutMask = set == InstructionSet.ARM ? 0x0FF000F0 : 0xFFC0u;
        uint op      = KeyToOpcode(set, key);

        List<(string Call, List<(uint Mask, uint Expected)>? Conditions)> cases = [];

        foreach (var pair in matches)
        {
            uint runtimeBits = MaskFromRanges(pair.Group.Runtime.SelectMany(p => p.Ranges).Select(r => (r.Hi, r.Lo)));
            uint residual    = pair.Definition.Mask & ~lutMask & ~runtimeBits;
            string call      = Resolve(set, pair.Group, op, version, traits);
            var condition    = (residual, pair.Definition.Expected & residual);

            int existing = cases.FindIndex(c => c.Call == call);

            if (existing < 0)
                cases.Add((call, residual == 0 ? null : [condition]));
            else if (cases[existing].Conditions is not null)
            {
                if (residual == 0)
                    cases[existing] = (call, null);
                else if (!cases[existing].Conditions!.Contains(condition))
                    cases[existing].Conditions!.Add(condition);
            }

            int unconditional = cases.FindIndex(c => c.Conditions is null);

            if (unconditional >= 0)
            {
                cases.RemoveRange(unconditional + 1, cases.Count - unconditional - 1);
                break;
            }
        }

        cases[cases.Count - 1] = (cases[cases.Count - 1].Call, null);

        if (cases.Count == 1)
            return cases[0].Call;

        StringBuilder sb = new("{ ");

        foreach (var (call, conditions) in cases)
        {
            if (conditions is null)
            {
                sb.Append(call).Append("; }");
                return sb.ToString();
            }

            sb.Append("if (").Append(Test(conditions)).Append(") ").Append(call).Append("; else ");
        }

        sb.Append(Call(set, null, null)).Append("; }");
        return sb.ToString();
    }


    private static uint MaskFromRanges(IEnumerable<(byte Hi, byte Lo)> ranges)
    {
        uint mask = 0;

        foreach (var (hi, lo) in ranges)
            mask |= (uint)(((1UL << (hi - lo + 1)) - 1) << lo);

        return mask;
    }

    private static string Test(List<(uint Mask, uint Expected)> conditions)
    {
        string One((uint Mask, uint Expected) p) => $"(op & 0x{p.Mask:X}) == 0x{p.Expected:X}";

        return conditions.Count == 1 ? One(conditions[0]) : string.Join(" || ", conditions.Select(p => "(" + One(p) + ")"));
    }

    private static string Lambda(string body) => "static (t, op) => " + body;

    private static string Resolve(InstructionSet set, GroupInfo g, uint op, ARMVersion version, HashSet<Trait> traits, string self = "t")
    {
        uint[] values = [.. g.Templated.Select(p => ExtractValue(op, p))];
        var rule = GroupCatalog.RuleFor(g.Override);

        if (rule is { Parameter: not null } && version < rule.Since)
        {
            int i = Array.FindIndex(g.Templated, p => p.Name == rule.Parameter);

            if (rule.NonZero ? values[i] != 0 : values[i] == 1)
                return Call(set, null, null, self);
        }

        Trait? trait = null;

        if (values.Length > 0)
        {
            trait = new Trait(g.Set, g.Name, values.ToEquatableArray());
            traits.Add(trait);
        }

        return Call(set, g, trait, self);
    }

    private static uint ExtractValue(uint op, ParameterInfo p)
    {
        uint value = 0;

        foreach (var r in p.Ranges)
        {
            int width = r.Hi - r.Lo + 1;
            value     = (value << width) | ((op >> r.Lo) & ((1u << width) - 1));
        }

        return value;
    }

    private static string Call(InstructionSet set, GroupInfo? g, Trait? trait, string self = "t")
    {
        if (g is null)
            return $"{self}.{GroupInfo.PrefixOf(set)}_Undefined(op)";

        return trait is null
            ? $"{self}.{g.WrapperName}(op)"
            : $"{self}.{g.WrapperName}<{trait.Name}>(op)";
    }
}