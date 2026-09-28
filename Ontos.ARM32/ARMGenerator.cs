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
            parsed.SelectMany(static (r, _) => r.Diagnostics.AsImmutableArray()),
            static (spc, d) => spc.ReportDiagnostic(d.ToDiagnostic())
        );

        var targets = parsed
            .Where(static r => r.Model is not null)
            .Select(static (r, _) => r.Model!)
            .WithTrackingName("ARMTargets");

        ctx.RegisterSourceOutput(targets, static (spc, model) =>
        {
            SourceBuilder builder = new();
            spc.AddSource(model.SourceName, builder.ToString());
        });
    }
}