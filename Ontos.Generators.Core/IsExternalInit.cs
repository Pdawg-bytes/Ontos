using System.ComponentModel;

namespace System.Runtime.CompilerServices
{
    // Polyfill for record types and init-only properties in projects targeting netstandard2.0
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static class IsExternalInit { }
}