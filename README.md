# Ontos
A collection of source-generator-based reusable ISA decoders.

Each decoder is a Roslyn incremental generator. You declare a `partial` type with a decoder attribute, and the generator emits the instruction dispatch tables and the contracts you implement.

## Packages

| Package | Purpose |
|---|---|
| `Ontos.Core` | Attributes and enums shared by all decoders. Pulled in automatically by any decoder package. |
| `Ontos.ARM32` | ARM32 (ARM and Thumb) decoder generator. |

Decoder packages are analyzer-only and self-contained, so a project can reference several of them without conflicts.

## Requirements

- .NET 10 SDK or newer (the generators use Roslyn 5.9)
- `AllowUnsafeBlocks` enabled in the consuming project, since the generated dispatch uses function pointers

## Usage

```
dotnet add package Ontos.ARM32
```

```csharp
using Ontos.Core.ARM32;
using Ontos.ARM32.Generated;

[ARMDecoder(ARMVersion.V6)]
public partial class ARMImplementation : IARMv6 /* generated contract */
{
    // Implement the generated contract for the version
}
```

`ARMDecoder` can be applied more than once, with a different `ARMVersion` each time, to generate a decoder per version. Each version may appear at most once per type.


## License
See [LICENSE](LICENSE).