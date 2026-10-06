using System;

namespace Ontos.Core.ARM32;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
public class ARMDecoderAttribute : DecoderAttribute<ARMVersion, ARMFeatures>
{
    public ARMDecoderAttribute(ARMVersion version, ARMFeatures features = ARMFeatures.None) 
        : base(version, features) { }
}