using System.Linq;
using Ontos.Generators.Core.Collections;

namespace Ontos.Generators.Core.Diagnostics;

using Microsoft.CodeAnalysis;

internal sealed record DiagnosticInfo(
    DiagnosticDescriptor Descriptor,
    LocationInfo? Location,
    EquatableArray<string> Args)
{
    public Diagnostic ToDiagnostic() =>
        Diagnostic.Create(Descriptor, Location?.ToLocation(), Args.Select(a => (object)a).ToArray());
}