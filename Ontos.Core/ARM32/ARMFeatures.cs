using System;

namespace Ontos.Core.ARM32;

[Flags]
public enum ARMFeatures
{
    None  = 0,
    Thumb = 1 << 0,
    VFPv2 = 1 << 1,
}