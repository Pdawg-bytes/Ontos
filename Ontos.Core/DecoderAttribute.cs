using System;

namespace Ontos.Core;

public abstract class DecoderAttribute : Attribute { }

public abstract class  DecoderAttribute<TVersion, TFeatures>(TVersion version, TFeatures features) : DecoderAttribute
    where TVersion  : struct, Enum
    where TFeatures : struct, Enum
{
    public TVersion Version   { get; } = version;
    public TFeatures Features { get; } = features;
}