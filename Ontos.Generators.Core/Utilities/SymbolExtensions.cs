using System.Linq;
using Microsoft.CodeAnalysis;
using Ontos.Generators.Core.Collections;

namespace Ontos.Generators.Core.Utilities;

internal static class SymbolExtensions
{
    public static string GetAccessibilityString(this INamedTypeSymbol symbol) => symbol.DeclaredAccessibility switch
    {
        Accessibility.Public               => "public",
        Accessibility.Internal             => "internal",
        Accessibility.Private              => "private",
        Accessibility.Protected            => "protected",
        Accessibility.ProtectedAndInternal => "protected internal",
        Accessibility.ProtectedOrInternal  => "private protected",
        _                                  => ""
    };

    public static string GetKeyword(this INamedTypeSymbol symbol) =>
        symbol.IsRecord ? (symbol.IsValueType ? "record struct" : "record")
                        : (symbol.IsValueType ? "struct"        : "class");

    public static EquatableArray<string> TypeParameterNames(this INamedTypeSymbol symbol) =>
        symbol.TypeParameters.Select(p => p.Name).ToEquatableArray();
}