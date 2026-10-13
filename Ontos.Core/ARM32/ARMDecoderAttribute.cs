using System;

namespace Ontos.Core.ARM32;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, AllowMultiple = true)]
public class ARMDecoderAttribute(ARMVersion version, ARMFeatures features = ARMFeatures.None) : DecoderAttribute<ARMVersion, ARMFeatures>(version, features)
{
}