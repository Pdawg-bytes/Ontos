using Microsoft.CodeAnalysis;

namespace Ontos.ARM32.Parsing;

internal static class DiagnosticDescriptors
{
    private const string Category = "Ontos.ARM32";

    internal static readonly DiagnosticDescriptor MustBePartial = new(
        id:                 "ONT100",
        title:              "Decoder target must be partial",
        messageFormat:      "'{0}' is decorated with [ARMDecoder] and must be declared 'partial' (as must all containing types)",
        category:           Category,
        defaultSeverity:    DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    internal static readonly DiagnosticDescriptor DuplicateVersion = new(
        id:                 "ONT101",
        title:              "Duplicate ARM version",
        messageFormat:      "'{0}' declares ARMVersion.{1} more than once, each version may appear at most once per type",
        category:           Category,
        defaultSeverity:    DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );

    internal static readonly DiagnosticDescriptor FeatureUnsupported = new(
        id:                 "ONT102",
        title:              "Feature not supported on this ARM version",
        messageFormat:      "'{0}' requests ARMFeatures.{1} on ARMVersion.{2}, which does not support this feature",
        category:           Category,
        defaultSeverity:    DiagnosticSeverity.Error,
        isEnabledByDefault: true
    );
}