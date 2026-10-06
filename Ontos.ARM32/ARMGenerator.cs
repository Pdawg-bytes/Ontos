using System;
using System.Linq;
using Ontos.Core.ARM32;
using Ontos.ARM32.Setup;
using Ontos.ARM32.Parsing;
using Ontos.ARM32.Emission;
using Microsoft.CodeAnalysis;
using Ontos.Generators.Core.Utilities;
using Ontos.Generators.Core.Collections;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Ontos.ARM32;

[Generator]
public sealed class ARMGenerator : IIncrementalGenerator
{
    private const string AttributeMetadataName = "Ontos.Core.ARM32.ARMDecoderAttribute";

    public void Initialize(IncrementalGeneratorInitializationContext ctx)
    {
        var parsed = ctx.SyntaxProvider
            .ForAttributeWithMetadataName(
                AttributeMetadataName,
                predicate: static (node, _) => node is ClassDeclarationSyntax,
                transform: static (c, ct)   => TargetParser.Parse(c, ct)
            )
            .WithTrackingName("ARMParse");

        ctx.RegisterSourceOutput(
            parsed.SelectMany(static (r, _) => r.Diagnostics),
            static (spc, d)                 => spc.ReportDiagnostic(d.ToDiagnostic())
        );

        var targets = parsed
            .Where(static r       => r.Model is not null)
            .Select(static (r, _) => r.Model!)
            .WithTrackingName("ARMTargets");

        var versions = targets
            .SelectMany(static (m, _) => m.Decoders.Select(d => (byte)d.Version).ToArray())
            .Collect()
            .Select(static (all, _) => all.Distinct().OrderBy(v => v).ToEquatableArray())
            .WithTrackingName("ARMVersions");

        var versionItems = versions.SelectMany(static (v, _) => v.ToArray());

        var plans = versionItems
            .Select(static (v, _) => VersionSetup.Build((ARMVersion)v))
            .WithTrackingName("ARMPlans");

        ctx.RegisterSourceOutput(
            versions.Select(static (v, _) => v.Length == 0 ? (byte)0xFF : v[v.Length - 1]),
            static (spc, highest) =>
            {
                if (highest != 0xFF)
                    spc.AddSource("Ontos.TraitContracts.g.cs", ContractEmitter.EmitTraitContracts((ARMVersion)highest));
            });

        ctx.RegisterSourceOutput(versionItems, static (spc, v) =>
        {
            var version = (ARMVersion)v;
            spc.AddSource($"Ontos.{ContractEmitter.VersionInterfaceName(version)}.g.cs", ContractEmitter.EmitVersionInterface(version));
        });

        ctx.RegisterSourceOutput(plans.Collect(), static (spc, all) =>
        {
            if (all.IsEmpty)
                return;

            var traits = all.SelectMany(p => p.Traits).Distinct().OrderBy(t => t.Name, StringComparer.Ordinal).ToEquatableArray();
            spc.AddSource("Ontos.Traits.g.cs", ContractEmitter.EmitTraits(traits));
        });

        ctx.RegisterSourceOutput(targets.Combine(plans.Collect()), static (spc, pair) =>
        {
            var (model, planArray) = pair;

            SourceBuilder builder = new();
            builder.Line($"using {ContractEmitter.Namespace};");
            builder.Line();

            EnterContainingType(builder, model);
            DecoderEmitter.EmitBody(builder, model, planArray);
            ExitContainingType(builder, model);

            spc.AddSource(model.SourceName, builder.ToString());
        });
    }


    private static void EnterContainingType(SourceBuilder builder, TargetModel model)
    {
        if (model.Namespace.Length > 0)
            builder.OpenBlock($"namespace {model.Namespace}");

        string FormatType(string accessibility, string keyword, string name, int paramLength, Func<string> getJoinedParams)
        {
            string typeParams = paramLength > 0 ? $"<{getJoinedParams()}>" : "";
            return $"{(accessibility.Length > 0 ? accessibility + " " : "")}partial {keyword} {name}{typeParams}";
        }

        foreach (var c in model.ContainingTypes)
            builder.OpenBlock(FormatType(c.Accessibility, c.Keyword, c.Name, c.TypeParameters.Length, () => string.Join(", ", c.TypeParameters)));

        builder.OpenBlock(FormatType(model.Accessibility, model.Keyword, model.TypeName, model.TypeParameters.Length, () => string.Join(", ", model.TypeParameters)));
    }

    private static void ExitContainingType(SourceBuilder builder, TargetModel model)
    {
        builder.CloseBlock();

        foreach (var _ in model.ContainingTypes)
            builder.CloseBlock();

        if (model.Namespace.Length > 0)
            builder.CloseBlock();
    }
}