using System;
using System.Linq;
using System.Threading;
using Ontos.Core.ARM32;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;
using Ontos.Generators.Core.Collections;
using Ontos.Generators.Core.Diagnostics;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using static Ontos.Generators.Core.Utilities.SymbolExtensions;

namespace Ontos.ARM32.Parsing;

internal static class TargetParser
{
    public static ParseResult Parse(GeneratorAttributeSyntaxContext ctx, CancellationToken ct)
    {
        if (ctx.TargetSymbol is not INamedTypeSymbol symbol || ctx.TargetNode is not TypeDeclarationSyntax node)
            return new ParseResult(null, EquatableArray<DiagnosticInfo>.Empty);

        List<DiagnosticInfo> diagnostics = [];
        bool hasError                    = false;
        string displayName               = symbol.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat);

        bool allPartial = node.Modifiers.Any(SyntaxKind.PartialKeyword) && 
                          node.Ancestors().OfType<TypeDeclarationSyntax>()
                            .All(t => t.Modifiers.Any(SyntaxKind.PartialKeyword));

        if (!allPartial)
        {
            diagnostics.Add(new DiagnosticInfo(
                DiagnosticDescriptors.MustBePartial,
                LocationInfo.From(node.Identifier.GetLocation()),
                new[] { displayName }.ToEquatableArray()
            ));

            hasError = true;
        }

        HashSet<ARMVersion> seen    = [];
        List<DecoderModel> decoders = [];

        foreach (var attr in ctx.Attributes)
        {
            ct.ThrowIfCancellationRequested();

            if (attr.ConstructorArguments.Length != 2) continue;
            TypedConstant vArg = attr.ConstructorArguments[0];
            TypedConstant fArg = attr.ConstructorArguments[1];

            if (vArg.Kind == TypedConstantKind.Error || fArg.Kind == TypedConstantKind.Error) 
                continue;

            if (vArg.Value is null || fArg.Value is null) continue;

            ARMVersion version   = (ARMVersion)Convert.ToByte(vArg.Value);
            ARMFeatures features = (ARMFeatures)Convert.ToUInt32(fArg.Value);

            if (version < ARMVersion.V6 && (features & ARMFeatures.VFPv2) != 0)
            {
                diagnostics.Add(new DiagnosticInfo(
                    DiagnosticDescriptors.FeatureUnsupported,
                    LocationInfo.From(attr.ApplicationSyntaxReference?.GetSyntax(ct).GetLocation()),
                    new[] { displayName, "VFPv2", version.ToString() }.ToEquatableArray()
                ));
            }

            if (!seen.Add(version))
            {
                diagnostics.Add(new DiagnosticInfo(
                    DiagnosticDescriptors.DuplicateVersion,
                    LocationInfo.From(attr.ApplicationSyntaxReference?.GetSyntax(ct).GetLocation()),
                    new[] { displayName, version.ToString() }.ToEquatableArray()
                ));

                hasError = true;
                continue;
            }

            decoders.Add(new DecoderModel(version, features));
        }

        decoders.Sort((a, b) => a.Version.CompareTo(b.Version));

        var diagArray = diagnostics.ToEquatableArray();

        if (hasError || decoders.Count == 0)
            return new ParseResult(null, diagArray);

        List<ContainingTypeModel> containing = [];

        for (var c = symbol.ContainingType; c is not null; c = c.ContainingType)
            containing.Add(new ContainingTypeModel(c.GetKeyword(), c.Name, c.TypeParameterNames(), c.GetAccessibilityString()));

        containing.Reverse();

        TargetModel model = new(
            Namespace:       symbol.ContainingNamespace.IsGlobalNamespace ? "" : symbol.ContainingNamespace.ToDisplayString(),
            Keyword:         symbol.GetKeyword(),
            TypeName:        symbol.Name,
            TypeParameters:  symbol.TypeParameterNames(),
            ContainingTypes: containing.ToEquatableArray(),
            Decoders:        decoders.ToEquatableArray(),
            ThumbDecode:     decoders.Any(d => d.Version >= ARMVersion.V4T),
            Accessibility:   symbol.GetAccessibilityString()
        );

        return new ParseResult(model, diagArray);
    }
}