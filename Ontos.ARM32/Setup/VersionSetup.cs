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
internal sealed record VersionOutput(ARMVersion Version, LUT ARM, LUT? ARMUnconditional, LUT? Thumb, EquatableArray<Trait> Traits);

internal readonly record struct InstructionPair(Instruction Definition, GroupInfo Group);

internal static class VersionSetup
{
    internal const string OpsClass = "__OntosWrappers";

    private const uint CondNibble    = 0xF0000000;
    private const uint UncondLUTMask = 0x0FF10010;

    internal static VersionOutput Build(ARMVersion version)
    {
        HashSet<Trait> traits = [];

        LUT arm      = BuildLUT(InstructionSet.ARM, version, traits);
        LUT? uncond  = version >= ARMVersion.V5  ? BuildLUT(InstructionSet.ARM, version, traits, true) : null;
        LUT? thumb   = version >= ARMVersion.V4T ? BuildLUT(InstructionSet.Thumb, version, traits) : null;

        return new VersionOutput(version, arm, uncond, thumb, traits.OrderBy(t => t.Name, StringComparer.Ordinal).ToEquatableArray());
    }

    private static int PopCount(uint v)
    {
        int n = 0;
        for (; v != 0; v &= v - 1) n++;
        return n;
    }

    private static uint ToKey(InstructionSet set, uint word, bool uncond) =>
        uncond ? ((word >> 18) & 0x3FC) | ((word >> 15) & 0x2) | ((word >> 4) & 0x1)
        : set == InstructionSet.ARM
            ? ((word & 0x0FF00000) >> 16) | ((word & 0x000000F0) >> 4)
            : (word & 0xFFC0) >> 6;

    private static uint KeyToOpcode(InstructionSet set, int key, bool uncond) =>
        uncond ? CondNibble | (((uint)key >> 2) & 0xFF) << 20 | (((uint)key >> 1) & 1) << 16 | ((uint)key & 1) << 4
        : set == InstructionSet.ARM
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

        OverrideRule? rule = GroupCatalog.RuleFor(i.Group.Override);
        return rule is { Parameter: null } && version >= rule.Since;
    }

    private static LUT BuildLUT(InstructionSet set, ARMVersion version, HashSet<Trait> traits, bool uncond = false)
    {
        bool arm = set == InstructionSet.ARM;
        int size = uncond ? 1024 : arm ? 4096 : 1024;

        InstructionPair[] instructions = [..GetInstructions(set, version).Where(i => uncond
            ? IsUnconditional(i, version)
            : !arm || (i.Definition.Expected & CondNibble) != CondNibble)];

        var keyed = instructions
            .Select(i => (Pair: i, Mask: ToKey(set, i.Definition.Mask, uncond), Expected: ToKey(set, i.Definition.Expected, uncond)))
            .ToArray();

        string[] lut = new string[size];

        for (int key = 0; key < size; key++)
        {
            var matches = keyed.Where(k => ((uint)key & k.Mask) == k.Expected).ToArray();

            if (matches.Length == 0)
            {
                lut[key] = Call(set, null, null);
                continue;
            }

            lut[key] = ResolveMatches(set, [.. matches.Select(m => m.Pair)], key, version, traits, uncond);
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

    private static string ResolveMatches(InstructionSet set, InstructionPair[] matches, int key, ARMVersion version, HashSet<Trait> traits, bool uncond)
    {
        uint lutMask = uncond ? UncondLUTMask | CondNibble : set == InstructionSet.ARM ? 0x0FF000F0 : 0xFFC0u;
        uint op      = KeyToOpcode(set, key, uncond);

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
        static string One((uint Mask, uint Expected) p) => $"(op & 0x{p.Mask:X}) == 0x{p.Expected:X}";

        return conditions.Count == 1 ? One(conditions[0]) : string.Join(" || ", conditions.Select(p => "(" + One(p) + ")"));
    }

    private static string Resolve(InstructionSet set, GroupInfo g, uint op, ARMVersion version, HashSet<Trait> traits, string self = "t")
    {
        uint[] values      = [.. g.Templated.Select(p => ExtractValue(op, p))];
        OverrideRule? rule = GroupCatalog.RuleFor(g.Override);

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

        foreach (BitRange r in p.Ranges)
        {
            int width = r.Hi - r.Lo + 1;
            value     = (value << width) | ((op >> r.Lo) & ((1u << width) - 1));
        }

        return value;
    }

    private static string Call(InstructionSet set, GroupInfo? g, Trait? trait, string self = "t")
    {
        if (g is null)
            return $"{OpsClass}.{GroupInfo.PrefixOf(set)}_Undefined({self}, op)";

        return trait is null
            ? $"{OpsClass}.{g.WrapperName}({self}, op)"
            : $"{OpsClass}.{g.WrapperName}<{trait.Name}>({self}, op)";
    }
}