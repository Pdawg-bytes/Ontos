using System;
using Ontos.Core.ARM32;

namespace Ontos.ARM32.Databases;

internal enum ParameterKind : byte
{
    Templated,
    Runtime
}

internal readonly record struct BitRange(byte Lo, byte Hi);

internal readonly record struct Parameter(
    string        Name,
    ParameterKind Kind,
    BitRange[]    Ranges
);

internal readonly record struct Instruction(
    string Name,
    uint   Mask,
    uint   Expected
);

internal sealed record GroupDefinition<T>(
    T               Group,
    ARMVersion      Version,
    VersionOverride Override,
    Parameter[]     Parameters,
    Instruction[]   Instructions
) where T : Enum;


internal enum VersionOverride
{
    None,
    V5_Link,          // Link == 0b1,    V5+   (bits[24])
    V5_Unconditional, // Cond == 0b1111, V5+   (bits[31:28])
    V6_Unconditional, // Cond == 0b1111, V6+   (bits[31:28])
    V6K_SizeNotZero,  // Size != 0b00,   V6K+  (bits[22:21])
}