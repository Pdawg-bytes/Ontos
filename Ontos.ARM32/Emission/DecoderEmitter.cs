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
        string self      = model.TypeParameters.Length > 0 ? $"{model.TypeName}<{string.Join(", ", model.TypeParameters)}>" : model.TypeName;
        string armT      = $"global::System.Action<{self}, uint>";
        string thumbT    = $"global::System.Action<{self}, ushort>";
        bool multi       = model.Decoders.Length > 1;

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
        EmitWrappers(sb, model);

        sb.Line();
        sb.OpenBlock($"private static TDelegate[] __OntosExpand<TDelegate>(TDelegate[] handlers, ushort[] map)");
        sb.Line("var lut = new TDelegate[map.Length];");
        sb.Line("for (int i = 0; i < map.Length; i++) lut[i] = handlers[map[i]];");
        sb.Line("return lut;");
        sb.CloseBlock();

        foreach (var (d, plan) in decoders)
        {
            string suffix = multi ? "_" + d.Version : "";

            sb.Line();
            sb.OpenBlock($"private static class __OntosLUT{suffix}");
            EmitLUT(sb, "ARM", armT, plan.ARM);

            if (plan.Thumb is not null)
            {
                sb.Line();
                EmitLUT(sb, "Thumb", thumbT, plan.Thumb);
            }

            sb.CloseBlock();
        }
    }

    private static void EmitDispatch(SourceBuilder sb, string set, string suffix, string delegateT, string opType, string index)
    {
        sb.Line();
        sb.Line($"private readonly {delegateT}[] _lut{set}{suffix} = __OntosLUT{suffix}.{set};");
        sb.Line(Inline);
        sb.Line($"public void Dispatch{set}{suffix.Replace("_", "")}({opType} opcode) => _lut{set}{suffix}[{index}](this, opcode);");
    }

    private static void EmitUnconditional(SourceBuilder sb, string suffix, EquatableArray<string> body)
    {
        sb.Line();
        sb.OpenBlock($"public void DispatchUnconditional{suffix.Replace("_", "")}(uint op)");

        foreach (var line in body)
            sb.Line(line);

        sb.Line("this.ARM_Undefined(op);");
        sb.CloseBlock();
    }

    private static void EmitWrappers(SourceBuilder sb, TargetModel model)
    {
        ARMVersion highest = model.Decoders[model.Decoders.Length - 1].Version;

        foreach (var set in highest >= ARMVersion.V4T ? [ InstructionSet.ARM, InstructionSet.Thumb ] : new[] { InstructionSet.ARM })
        foreach (var g in GroupCatalog.GroupsFor(set, highest))
        {
            string generic = g.Templated.Length > 0 ? $"<T>" : "";
            string where   = g.Templated.Length > 0 ? $" where T : struct, {g.TraitInterface}" : "";
            string opType  = set == InstructionSet.ARM ? "uint" : "ushort";
            string src     = set == InstructionSet.ARM ? "opcode" : "(uint)opcode";
            string args    = string.Join(", ", g.Runtime.Select(p => Extract(p, src)));

            sb.Line(Inline);
            sb.Line($"private void {g.WrapperName}{generic}({opType} opcode){where} => {g.MethodName}{generic}({args});");
        }
    }

    private static void EmitLUT(SourceBuilder sb, string set, string delegateT, LUT lut)
    {
        sb.OpenBlock($"public static readonly {delegateT}[] {set} = __OntosExpand", BlockType.ParameterList);

        sb.OpenBlock($"new {delegateT}[]");
        foreach (var h in lut.Handlers)
            sb.Line(h + ",");
        sb.CloseBlock(",");

        sb.OpenBlock("new ushort[]");
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