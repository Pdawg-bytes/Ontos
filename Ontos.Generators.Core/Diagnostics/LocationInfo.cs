using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Ontos.Generators.Core.Diagnostics;

public sealed record LocationInfo(string FilePath, TextSpan TextSpan, LinePositionSpan LineSpan)
{
    public static LocationInfo? From(Location? location)
    {
        if (location is null || location.SourceTree is null) return null;

        return new LocationInfo(
            location.SourceTree.FilePath,
            location.SourceSpan,
            location.GetLineSpan().Span
        );
    }

    public Location ToLocation() => Location.Create(FilePath, TextSpan, LineSpan);
}