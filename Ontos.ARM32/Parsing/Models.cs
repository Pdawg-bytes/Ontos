using System.Text;
using Ontos.Core.ARM32;
using Ontos.Generators.Core.Collections;
using Ontos.Generators.Core.Diagnostics;

namespace Ontos.ARM32.Parsing;

internal sealed record DecoderModel(ARMVersion Version, ARMFeatures Features);

internal sealed record ContainingTypeModel(
    string Keyword,
    string Name,
    EquatableArray<string> TypeParameters);

internal sealed record TargetModel(
    string Namespace,
    string Keyword,
    string TypeName,
    EquatableArray<string> TypeParameters,
    EquatableArray<ContainingTypeModel> ContainingTypes,
    EquatableArray<DecoderModel> Decoders,
    bool ThumbDecode)
{
    public string SourceName
    {
        get
        {
            StringBuilder sb = new();

            if (Namespace.Length > 0) 
                sb.Append(Namespace).Append('.');

            foreach (var c in ContainingTypes)
                sb.Append(c.Name).Append(c.TypeParameters.Length > 0 ? "_" + c.TypeParameters.Length : "").Append('.');

            sb.Append(TypeName);

            if (TypeParameters.Length > 0)
                sb.Append('_').Append(TypeParameters.Length);

            return sb.Append(".g.cs").ToString();
        }
    }
}

internal sealed record ParseResult(TargetModel? Model, EquatableArray<DiagnosticInfo> Diagnostics);