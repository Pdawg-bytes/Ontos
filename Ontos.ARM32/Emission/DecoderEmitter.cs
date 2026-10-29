using System;
using System.Linq;
using System.Text;
using Ontos.Core.ARM32;
using Ontos.ARM32.Setup;
using Ontos.ARM32.Parsing;
using System.Collections.Immutable;
using Ontos.Generators.Core.Utilities;
using Ontos.Generators.Core.Collections;

namespace Ontos.ARM32.Emission;

internal static class DecoderEmitter
{
    private const string Inline = "[global::System.Runtime.CompilerServices.MethodImpl(global::System.Runtime.CompilerServices.MethodImplOptions.AggressiveInlining)]";

    internal static void EmitBody(SourceBuilder sb, TargetModel model, ImmutableArray<VersionOutput> versions)
    {
        string self   = model.TypeParameters.Length > 0 ? $"{model.TypeName}<{string.Join(", ", model.TypeParameters)}>" : model.TypeName;
        string armT   = $"delegate* managed<{self}, uint, void>";
        string thumbT = $"delegate* managed<{self}, ushort, void>";
        bool multi    = model.Decoders.Length > 1;

        var decoders = model.Decoders
            .Select(d => (Decoder: d, Output: versions.First(p => p.Version == d.Version)))
            .ToArray();

        foreach (var (d, version) in decoders)
        {
            string suffix = multi ? "_" + d.Version : "";

            EmitDispatch(sb, "ARM", suffix, armT, "uint", "((opcode >> 16) & 0xFF0) | ((opcode >> 4) & 0xF)");

            if (version.ARMUnconditional is not null)
                EmitUnconditional(sb, suffix, version.ARMUnconditional.Value);

            if (version.Thumb is not null)
                EmitDispatch(sb, "Thumb", suffix, thumbT, "ushort", "opcode >> 6");
        }

        sb.Line();
        EmitWrappers(sb, model, self);

        foreach (var (d, plan) in decoders)
        {
            string suffix = multi ? "_" + d.Version : "";

            sb.Line();
            sb.OpenBlock($"private static unsafe class __OntosLUT{suffix}");
            EmitLUT(sb, "ARM", armT, "uint", plan.ARM);

            if (plan.Thumb is not null)
            {
                sb.Line();
                EmitLUT(sb, "Thumb", thumbT, "ushort", plan.Thumb);
            }

            sb.CloseBlock();
        }
    }

    private static void EmitDispatch(SourceBuilder sb, string set, string suffix, string delegateT, string opType, string index)
    {
        sb.Line();
        sb.Line($"private readonly nint[] _lut{set}{suffix} = __OntosLUT{suffix}.{set};");
        sb.Line(Inline);

        string methodName = $"Dispatch{set}{suffix.Replace("_", "")}";
        string lutField   = $"_lut{set}{suffix}";

        sb.OpenBlock($"public unsafe void {methodName}({opType} opcode)");
        sb.Line($"int lutIndex         = (int)({index});");
        sb.Line($"void* funcPtrAddress = (void*)global::System.Runtime.CompilerServices.Unsafe.Add(");
        sb.Line($"    ref global::System.Runtime.InteropServices.MemoryMarshal.GetArrayDataReference({lutField}),");
        sb.Line($"    lutIndex");
        sb.Line(");");
        sb.Line();
        sb.Line($"(({delegateT})funcPtrAddress)(this, opcode);");
        sb.CloseBlock();
    }

    private static void EmitUnconditional(SourceBuilder sb, string suffix, EquatableArray<string> body)
    {
        sb.Line();
        sb.OpenBlock($"public void DispatchUnconditional{suffix.Replace("_", "")}(uint opcode)");

        foreach (var line in body)
            sb.Line(line);

        sb.Line("this.ARM_Undefined(opcode);");
        sb.CloseBlock();
    }

    private static void EmitWrappers(SourceBuilder sb, TargetModel model, string self)
    {
        ARMVersion highest = model.Decoders[model.Decoders.Length - 1].Version;
        var sets           = highest >= ARMVersion.V4T ? [ InstructionSet.ARM, InstructionSet.Thumb ] : new[] { InstructionSet.ARM };

        sb.OpenBlock($"private static class {VersionSetup.OpsClass}");

        foreach (var set in sets)
        {
            string opType = set == InstructionSet.ARM ? "uint" : "ushort";

            sb.Line(Inline);
            sb.Line($"public static void {GroupInfo.PrefixOf(set)}_Undefined({self} t, {opType} opcode) => t.{GroupInfo.PrefixOf(set)}_Undefined(opcode);");
        }

        foreach (var set in sets)
        {
            foreach (var g in GroupCatalog.GroupsFor(set, highest))
            {
                string generic = g.Templated.Length > 0 ? $"<T>" : "";
                string where   = g.Templated.Length > 0 ? $" where T : struct, {g.TraitInterface}" : "";
                string opType  = set == InstructionSet.ARM ? "uint" : "ushort";
                string src     = set == InstructionSet.ARM ? "opcode" : "(uint)opcode";
                string args    = string.Join(", ", g.Runtime.Select(p => Extract(p, src)));

                sb.Line(Inline);
                sb.Line($"public static void {g.WrapperName}{generic}({self} t, {opType} opcode){where} => t.{g.MethodName}{generic}({args});");
            }
        }

        sb.CloseBlock();
    }

    private static void EmitLUT(SourceBuilder sb, string set, string delegateT, string opType, LUT lut)
    {
        string self = delegateT.Substring("delegate* managed<".Length, delegateT.IndexOf(',') - "delegate* managed<".Length);

        string[] targets = new string[lut.Handlers.Length];

        for (int i = 0; i < lut.Handlers.Length; i++)
        {
            string body = lut.Handlers[i];

            if (body.StartsWith(VersionSetup.OpsClass + ".") && body.EndsWith("(t, op)"))
            {
                targets[i] = $"(nint)({delegateT})&" + body.Substring(0, body.Length - "(t, op)".Length);
                continue;
            }

            targets[i] = $"(nint)({delegateT})&{set}_{i}";
            sb.Line($"private static void {set}_{i}({self} t, {opType} op)");
            sb.Line(body);
        }

        sb.Line();
        sb.OpenBlock($"private static nint[] Expand{set}(global::System.ReadOnlySpan<nint> handlers, global::System.ReadOnlySpan<ushort> map)");
        sb.Line($"nint[] lut = new nint[map.Length];");
        sb.Line("for (int i = 0; i < map.Length; i++) lut[i] = handlers[map[i]];");
        sb.Line("return lut;");
        sb.CloseBlock();

        sb.Line();
        sb.OpenBlock($"public static readonly nint[] {set} = Expand{set}", BlockType.ParameterList);

        sb.OpenBlock(null, BlockType.Collection);
        for (int i = 0; i < lut.Handlers.Length; i++)
            sb.Line(targets[i] + ",");

        sb.CloseBlock(",");

        sb.OpenBlock(null, BlockType.Collection);
        for (int i = 0; i < lut.Map.Length; i += 32)
        {
            StringBuilder row = new();

            for (int j = i; j < Math.Min(i + 32, lut.Map.Length); j++)
                row.Append(lut.Map[j]).Append(", ");

            sb.Line(row.ToString().TrimEnd());
        }
        sb.CloseBlock();

        sb.CloseBlock(";");
    }

    private static string Extract(ParameterInfo p, string op)
    {
        int outShift = p.Width;
        var terms    = new System.Collections.Generic.List<string>();

        foreach (var r in p.Ranges)
        {
            int width = r.Hi - r.Lo + 1;
            outShift -= width;

            int diff   = r.Lo - outShift;
            string src = diff > 0 ? $"({op} >> {diff})" : diff < 0 ? $"({op} << {-diff})" : op;

            if (r.Hi == 31 && outShift == 0)
            {
                terms.Add(diff > 0 ? $"{op} >> {diff}" : op);
                continue;
            }

            ulong mask = ((1UL << width) - 1) << outShift;
            string m   = mask <= 0xFF ? $"0x{mask:X2}" : mask <= 0xFFFF ? $"0x{mask:X4}" : $"0x{mask:X}";

            terms.Add(diff == 0 ? $"{op} & {m}" : $"{src} & {m}");
        }

        return terms.Count == 1 ? terms[0] : string.Join(" | ", terms.Select(t => $"({t})"));
    }
}