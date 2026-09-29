using System;
using System.Linq;
using Ontos.ARM32.Parsing;
using Microsoft.CodeAnalysis;
using Ontos.Generators.Core.Utilities;
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
                predicate: static (node, _) => node is ClassDeclarationSyntax or StructDeclarationSyntax or RecordDeclarationSyntax,
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

        ctx.RegisterSourceOutput(targets, static (spc, model) =>
        {
            SourceBuilder builder = new();
            EnterContainingType(builder, model);

            // emit decoder

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