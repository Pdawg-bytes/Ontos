using System.Linq;
using Ontos.Core.ARM32;
using Ontos.ARM32.Setup;
using System.Collections.Generic;
using Ontos.Generators.Core.Utilities;
using Ontos.Generators.Core.Collections;

namespace Ontos.ARM32.Emission;

internal static class ContractEmitter
{
    internal const string Namespace = "Ontos.ARM32.Generated";

    internal static string VersionInterfaceName(ARMVersion v) => "IARMv" + v.ToString().Substring(1);

    private static IEnumerable<InstructionSet> SetsFor(ARMVersion v) =>
        v >= ARMVersion.V4T ? [InstructionSet.ARM, InstructionSet.Thumb] : [InstructionSet.ARM];

    internal static string EmitTraitContracts(ARMVersion highest)
    {
        SourceBuilder sb = new();
        sb.OpenBlock($"namespace {Namespace}");

        bool first = true;

        foreach (var set in SetsFor(highest))
        foreach (var g in GroupCatalog.GroupsFor(set, highest).Where(g => g.Templated.Length > 0))
        {
            if (!first) sb.Line();
            first = false;

            sb.OpenBlock($"public interface {g.TraitInterface}");

            foreach (var p in g.Templated)
                sb.Line($"static abstract {p.TypeName} {p.Name} {{ get; }}");

            sb.CloseBlock();
        }

        sb.CloseBlock();
        return sb.ToString();
    }

    internal static string EmitVersionInterface(ARMVersion version)
    {
        SourceBuilder sb = new();
        sb.OpenBlock($"namespace {Namespace}");
        sb.OpenBlock($"public interface {VersionInterfaceName(version)}");

        foreach (var set in SetsFor(version))
        {
            string prefix = GroupInfo.PrefixOf(set);

            sb.Line($"void {prefix}_Undefined(uint opcode);");

            foreach (var g in GroupCatalog.GroupsFor(set, version))
                sb.Line(Signature(g));

            if (set == InstructionSet.ARM && version >= ARMVersion.V4T)
                sb.Line();
        }

        sb.CloseBlock();
        sb.CloseBlock();
        return sb.ToString();
    }

    internal static string Signature(GroupInfo g)
    {
        string args    = string.Join(", ", g.Runtime.Select(p => $"uint {p.ArgName}"));
        string generic = g.Templated.Length > 0 ? "<T>" : "";
        string where   = g.Templated.Length > 0 ? $" where T : struct, {g.TraitInterface}" : "";

        return $"void {g.MethodName}{generic}({args}){where};";
    }

    internal static string EmitTraits(EquatableArray<Trait> traits)
    {
        SourceBuilder sb = new();
        sb.OpenBlock($"namespace {Namespace}");

        bool first = true;

        foreach (var t in traits)
        {
            if (!first) sb.Line();
            first = false;

            var info = t.Info;
            sb.OpenBlock($"internal readonly struct {t.Name} : {info.TraitInterface}");

            for (int i = 0; i < info.Templated.Length; i++)
            {
                ParameterInfo p = info.Templated[i];
                string v    = p.Width == 1 ? (t.Values[i] != 0 ? "true" : "false") : t.Values[i].ToString();

                sb.Line($"public static {p.TypeName} {p.Name} => {v};");
            }

            sb.CloseBlock();
        }

        sb.CloseBlock();
        return sb.ToString();
    }
}