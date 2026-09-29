using Ontos.Core.ARM32;

namespace Ontos.ARM32.Databases;

internal static partial class InstructionDatabase
{
    internal static readonly GroupDefinition<ARMGroup>[] ARMDatabase =
    [
        new
        (
            Group:    ARMGroup.ClearExclusive,
            Version:  ARMVersion.V6K,
            Override: VersionOverride.None,

            Parameters: [],
            Instructions:
            [
                new Instruction(Name: "arm_CLREX", Mask: 0xFFFFFFFF, Expected: 0xF57FF01F),
            ]
        ),
        new
        (
            Group:    ARMGroup.SetEndianness,
            Version:  ARMVersion.V6,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("Endianness", ParameterKind.Runtime, [ new BitRange(9, 9), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "arm_SETEND", Mask: 0xFFFFFDFF, Expected: 0xF1010000),
            ]
        ),
        new
        (
            Group:    ARMGroup.HintStatusCollision,
            Version:  ARMVersion.V6K,
            Override: VersionOverride.None,

            Parameters: [],
            Instructions:
            [
                new Instruction(Name: "arm_COLLISION", Mask: 0x0FFFFFFF, Expected: 0x0320F007),
            ]
        ),
        new
        (
            Group:    ARMGroup.WaitForInterrupt,
            Version:  ARMVersion.V6K,
            Override: VersionOverride.None,

            Parameters: [],
            Instructions:
            [
                new Instruction(Name: "arm_WFI", Mask: 0x0FFFFFFF, Expected: 0x0320F003),
            ]
        ),
        new
        (
            Group:    ARMGroup.SendEvent,
            Version:  ARMVersion.V6K,
            Override: VersionOverride.None,

            Parameters: [],
            Instructions:
            [
                new Instruction(Name: "arm_SEV", Mask: 0x0FFFFFFF, Expected: 0x0320F004),
            ]
        ),
        new
        (
            Group:    ARMGroup.WaitForEvent,
            Version:  ARMVersion.V6K,
            Override: VersionOverride.None,

            Parameters: [],
            Instructions:
            [
                new Instruction(Name: "arm_WFE", Mask: 0x0FFFFFFF, Expected: 0x0320F002),
            ]
        ),
        new
        (
            Group:    ARMGroup.Yield,
            Version:  ARMVersion.V6K,
            Override: VersionOverride.None,

            Parameters: [],
            Instructions:
            [
                new Instruction(Name: "arm_YIELD", Mask: 0x0FFFFFFF, Expected: 0x0320F001),
            ]
        ),
        new
        (
            Group:    ARMGroup.NoOperation,
            Version:  ARMVersion.V6K,
            Override: VersionOverride.None,

            Parameters: [],
            Instructions:
            [
                new Instruction(Name: "arm_NOP", Mask: 0x0FFFFFFF, Expected: 0x0320F000),
            ]
        ),
        new
        (
            Group:    ARMGroup.ReturnFromException,
            Version:  ARMVersion.V6,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("PreIndex", ParameterKind.Templated, [ new BitRange(24, 24), ]),
                new Parameter("Add", ParameterKind.Templated, [ new BitRange(23, 23), ]),
                new Parameter("Writeback", ParameterKind.Templated, [ new BitRange(21, 21), ]),
                new Parameter("Rn", ParameterKind.Runtime, [ new BitRange(16, 19), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "arm_RFE", Mask: 0xFE50FFFF, Expected: 0xF8100A00),
            ]
        ),
        new
        (
            Group:    ARMGroup.StoreReturnState,
            Version:  ARMVersion.V6,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("PreIndex", ParameterKind.Templated, [ new BitRange(24, 24), ]),
                new Parameter("Add", ParameterKind.Templated, [ new BitRange(23, 23), ]),
                new Parameter("Writeback", ParameterKind.Templated, [ new BitRange(21, 21), ]),
                new Parameter("Mode", ParameterKind.Runtime, [ new BitRange(0, 4), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "arm_SRS", Mask: 0xFE5FFFE0, Expected: 0xF84D0500),
            ]
        ),
        new
        (
            Group:    ARMGroup.SecureMonitorCall,
            Version:  ARMVersion.V6KZ,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("Immediate", ParameterKind.Runtime, [ new BitRange(0, 3), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "arm_SMC", Mask: 0x0FFFFFF0, Expected: 0x01600070),
            ]
        ),
        new
        (
            Group:    ARMGroup.BranchExchange,
            Version:  ARMVersion.V4T,
            Override: VersionOverride.V5_Link,

            Parameters:
            [
                new Parameter("Link", ParameterKind.Templated, [ new BitRange(5, 5), ]),
                new Parameter("Rm", ParameterKind.Runtime, [ new BitRange(0, 3), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "arm_BLX_reg", Mask: 0x0FFFFFF0, Expected: 0x012FFF30),
                new Instruction(Name: "arm_BX", Mask: 0x0FFFFFF0, Expected: 0x012FFF10),
            ]
        ),
        new
        (
            Group:    ARMGroup.ChangeState,
            Version:  ARMVersion.V6,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("Imod", ParameterKind.Runtime, [ new BitRange(18, 19), ]),
                new Parameter("UseMode", ParameterKind.Runtime, [ new BitRange(17, 17), ]),
                new Parameter("A", ParameterKind.Runtime, [ new BitRange(8, 8), ]),
                new Parameter("I", ParameterKind.Runtime, [ new BitRange(7, 7), ]),
                new Parameter("F", ParameterKind.Runtime, [ new BitRange(6, 6), ]),
                new Parameter("Mode", ParameterKind.Runtime, [ new BitRange(0, 4), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "arm_CPS", Mask: 0xFFF1FE20, Expected: 0xF1000000),
            ]
        ),
        new
        (
            Group:    ARMGroup.CountLeadingZeros,
            Version:  ARMVersion.V5,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("Rd", ParameterKind.Runtime, [ new BitRange(12, 15), ]),
                new Parameter("Rm", ParameterKind.Runtime, [ new BitRange(0, 3), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "arm_CLZ", Mask: 0x0FFF0FF0, Expected: 0x016F0F10),
            ]
        ),
        new
        (
            Group:    ARMGroup.BitReverse,
            Version:  ARMVersion.V6,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("SignExtend", ParameterKind.Templated, [ new BitRange(22, 22), ]),
                new Parameter("Halfword", ParameterKind.Templated, [ new BitRange(7, 7), ]),
                new Parameter("Rd", ParameterKind.Runtime, [ new BitRange(12, 15), ]),
                new Parameter("Rm", ParameterKind.Runtime, [ new BitRange(0, 3), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "arm_REV", Mask: 0x0FFF0FF0, Expected: 0x06BF0F30),
                new Instruction(Name: "arm_REV16", Mask: 0x0FFF0FF0, Expected: 0x06BF0FB0),
                new Instruction(Name: "arm_REVSH", Mask: 0x0FFF0FF0, Expected: 0x06FF0FB0),
            ]
        ),
        new
        (
            Group:    ARMGroup.ExclusiveLoad,
            Version:  ARMVersion.V6,
            Override: VersionOverride.V6K_SizeNotZero,

            Parameters:
            [
                new Parameter("Size", ParameterKind.Templated, [ new BitRange(21, 22), ]),
                new Parameter("Rn", ParameterKind.Runtime, [ new BitRange(16, 19), ]),
                new Parameter("Rt", ParameterKind.Runtime, [ new BitRange(12, 15), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "arm_LDREX", Mask: 0x0FF00FFF, Expected: 0x01900F9F),
                new Instruction(Name: "arm_LDREXD", Mask: 0x0FF00FFF, Expected: 0x01B00F9F),
                new Instruction(Name: "arm_LDREXB", Mask: 0x0FF00FFF, Expected: 0x01D00F9F),
                new Instruction(Name: "arm_LDREXH", Mask: 0x0FF00FFF, Expected: 0x01F00F9F),
            ]
        ),
        new
        (
            Group:    ARMGroup.ByteSelect,
            Version:  ARMVersion.V6,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("Rn", ParameterKind.Runtime, [ new BitRange(16, 19), ]),
                new Parameter("Rd", ParameterKind.Runtime, [ new BitRange(12, 15), ]),
                new Parameter("Rm", ParameterKind.Runtime, [ new BitRange(0, 3), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "arm_SEL", Mask: 0x0FF00FF0, Expected: 0x06800FB0),
            ]
        ),
        new
        (
            Group:    ARMGroup.Swap,
            Version:  ARMVersion.V2,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("ByteMode", ParameterKind.Templated, [ new BitRange(22, 22), ]),
                new Parameter("Rn", ParameterKind.Runtime, [ new BitRange(16, 19), ]),
                new Parameter("Rt", ParameterKind.Runtime, [ new BitRange(12, 15), ]),
                new Parameter("Rt2", ParameterKind.Runtime, [ new BitRange(0, 3), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "arm_SWP", Mask: 0x0FF00FF0, Expected: 0x01000090),
                new Instruction(Name: "arm_SWPB", Mask: 0x0FF00FF0, Expected: 0x01400090),
            ]
        ),
        new
        (
            Group:    ARMGroup.Preload,
            Version:  ARMVersion.V5E,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("RegisterOffset", ParameterKind.Templated, [ new BitRange(25, 25), ]),
                new Parameter("AddOffset", ParameterKind.Templated, [ new BitRange(23, 23), ]),
                new Parameter("Rn", ParameterKind.Runtime, [ new BitRange(16, 19), ]),
                new Parameter("Value", ParameterKind.Runtime, [ new BitRange(0, 11), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "arm_PLD_reg", Mask: 0xFF70F010, Expected: 0xF750F000),
                new Instruction(Name: "arm_PLD_imm", Mask: 0xFF70F000, Expected: 0xF550F000),
            ]
        ),
        new
        (
            Group:    ARMGroup.ExclusiveStore,
            Version:  ARMVersion.V6,
            Override: VersionOverride.V6K_SizeNotZero,

            Parameters:
            [
                new Parameter("Size", ParameterKind.Templated, [ new BitRange(21, 22), ]),
                new Parameter("Rn", ParameterKind.Runtime, [ new BitRange(16, 19), ]),
                new Parameter("Rd", ParameterKind.Runtime, [ new BitRange(12, 15), ]),
                new Parameter("Rt", ParameterKind.Runtime, [ new BitRange(0, 3), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "arm_STREX", Mask: 0x0FF00FF0, Expected: 0x01800F90),
                new Instruction(Name: "arm_STREXD", Mask: 0x0FF00FF0, Expected: 0x01A00F90),
                new Instruction(Name: "arm_STREXB", Mask: 0x0FF00FF0, Expected: 0x01C00F90),
                new Instruction(Name: "arm_STREXH", Mask: 0x0FF00FF0, Expected: 0x01E00F90),
            ]
        ),
        new
        (
            Group:    ARMGroup.SaturatedArithmetic,
            Version:  ARMVersion.V5E,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("Opcode", ParameterKind.Templated, [ new BitRange(21, 22), ]),
                new Parameter("Rn", ParameterKind.Runtime, [ new BitRange(16, 19), ]),
                new Parameter("Rd", ParameterKind.Runtime, [ new BitRange(12, 15), ]),
                new Parameter("Rm", ParameterKind.Runtime, [ new BitRange(0, 3), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "arm_QADD", Mask: 0x0FF00FF0, Expected: 0x01000050),
                new Instruction(Name: "arm_QSUB", Mask: 0x0FF00FF0, Expected: 0x01200050),
                new Instruction(Name: "arm_QDADD", Mask: 0x0FF00FF0, Expected: 0x01400050),
                new Instruction(Name: "arm_QDSUB", Mask: 0x0FF00FF0, Expected: 0x01600050),
            ]
        ),
        new
        (
            Group:    ARMGroup.SumAbsoluteDifferences,
            Version:  ARMVersion.V6,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("Rd", ParameterKind.Runtime, [ new BitRange(16, 19), ]),
                new Parameter("Ra", ParameterKind.Runtime, [ new BitRange(12, 15), ]),
                new Parameter("Rm", ParameterKind.Runtime, [ new BitRange(8, 11), ]),
                new Parameter("Rn", ParameterKind.Runtime, [ new BitRange(0, 3), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "arm_USAD8", Mask: 0x0FF0F0F0, Expected: 0x0780F010),
                new Instruction(Name: "arm_USADA8", Mask: 0x0FF000F0, Expected: 0x07800010),
            ]
        ),
        new
        (
            Group:    ARMGroup.Breakpoint,
            Version:  ARMVersion.V5,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("Immediate", ParameterKind.Runtime, [ new BitRange(8, 19), new BitRange(0, 3), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "arm_BKPT", Mask: 0x0FF000F0, Expected: 0x01200070),
            ]
        ),
        new
        (
            Group:    ARMGroup.MultiplyLongDoubleAccumulate,
            Version:  ARMVersion.V6,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("RdHi", ParameterKind.Runtime, [ new BitRange(16, 19), ]),
                new Parameter("RdLo", ParameterKind.Runtime, [ new BitRange(12, 15), ]),
                new Parameter("Rm", ParameterKind.Runtime, [ new BitRange(8, 11), ]),
                new Parameter("Rn", ParameterKind.Runtime, [ new BitRange(0, 3), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "arm_UMAAL", Mask: 0x0FF000F0, Expected: 0x00400090),
            ]
        ),
        new
        (
            Group:    ARMGroup.Extension,
            Version:  ARMVersion.V6,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("Unsigned", ParameterKind.Templated, [ new BitRange(22, 22), ]),
                new Parameter("DataType", ParameterKind.Templated, [ new BitRange(20, 21), ]),
                new Parameter("Rn", ParameterKind.Runtime, [ new BitRange(16, 19), ]),
                new Parameter("Rd", ParameterKind.Runtime, [ new BitRange(12, 15), ]),
                new Parameter("Rotation", ParameterKind.Runtime, [ new BitRange(10, 11), ]),
                new Parameter("Rm", ParameterKind.Runtime, [ new BitRange(0, 3), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "arm_SXTB", Mask: 0x0FFF03F0, Expected: 0x06AF0070),
                new Instruction(Name: "arm_SXTB16", Mask: 0x0FFF03F0, Expected: 0x068F0070),
                new Instruction(Name: "arm_SXTH", Mask: 0x0FFF03F0, Expected: 0x06BF0070),
                new Instruction(Name: "arm_UXTB", Mask: 0x0FFF03F0, Expected: 0x06EF0070),
                new Instruction(Name: "arm_UXTB16", Mask: 0x0FFF03F0, Expected: 0x06CF0070),
                new Instruction(Name: "arm_UXTH", Mask: 0x0FFF03F0, Expected: 0x06FF0070),
                new Instruction(Name: "arm_SXTAB", Mask: 0x0FF003F0, Expected: 0x06A00070),
                new Instruction(Name: "arm_SXTAB16", Mask: 0x0FF003F0, Expected: 0x06800070),
                new Instruction(Name: "arm_SXTAH", Mask: 0x0FF003F0, Expected: 0x06B00070),
                new Instruction(Name: "arm_UXTAB", Mask: 0x0FF003F0, Expected: 0x06E00070),
                new Instruction(Name: "arm_UXTAB16", Mask: 0x0FF003F0, Expected: 0x06C00070),
                new Instruction(Name: "arm_UXTAH", Mask: 0x0FF003F0, Expected: 0x06F00070),
            ]
        ),
        new
        (
            Group:    ARMGroup.ParallelArithmetic,
            Version:  ARMVersion.V6,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("Unsigned", ParameterKind.Templated, [ new BitRange(22, 22), ]),
                new Parameter("Mode", ParameterKind.Templated, [ new BitRange(20, 21), ]),
                new Parameter("Opcode", ParameterKind.Templated, [ new BitRange(5, 7), ]),
                new Parameter("Rn", ParameterKind.Runtime, [ new BitRange(16, 19), ]),
                new Parameter("Rd", ParameterKind.Runtime, [ new BitRange(12, 15), ]),
                new Parameter("Rm", ParameterKind.Runtime, [ new BitRange(0, 3), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "arm_SADD8", Mask: 0x0FF00FF0, Expected: 0x06100F90),
                new Instruction(Name: "arm_SADD16", Mask: 0x0FF00FF0, Expected: 0x06100F10),
                new Instruction(Name: "arm_SASX", Mask: 0x0FF00FF0, Expected: 0x06100F30),
                new Instruction(Name: "arm_SSAX", Mask: 0x0FF00FF0, Expected: 0x06100F50),
                new Instruction(Name: "arm_SSUB8", Mask: 0x0FF00FF0, Expected: 0x06100FF0),
                new Instruction(Name: "arm_SSUB16", Mask: 0x0FF00FF0, Expected: 0x06100F70),
                new Instruction(Name: "arm_UADD8", Mask: 0x0FF00FF0, Expected: 0x06500F90),
                new Instruction(Name: "arm_UADD16", Mask: 0x0FF00FF0, Expected: 0x06500F10),
                new Instruction(Name: "arm_UASX", Mask: 0x0FF00FF0, Expected: 0x06500F30),
                new Instruction(Name: "arm_USAX", Mask: 0x0FF00FF0, Expected: 0x06500F50),
                new Instruction(Name: "arm_USUB8", Mask: 0x0FF00FF0, Expected: 0x06500FF0),
                new Instruction(Name: "arm_USUB16", Mask: 0x0FF00FF0, Expected: 0x06500F70),
                new Instruction(Name: "arm_QADD8", Mask: 0x0FF00FF0, Expected: 0x06200F90),
                new Instruction(Name: "arm_QADD16", Mask: 0x0FF00FF0, Expected: 0x06200F10),
                new Instruction(Name: "arm_QASX", Mask: 0x0FF00FF0, Expected: 0x06200F30),
                new Instruction(Name: "arm_QSAX", Mask: 0x0FF00FF0, Expected: 0x06200F50),
                new Instruction(Name: "arm_QSUB8", Mask: 0x0FF00FF0, Expected: 0x06200FF0),
                new Instruction(Name: "arm_QSUB16", Mask: 0x0FF00FF0, Expected: 0x06200F70),
                new Instruction(Name: "arm_UQADD8", Mask: 0x0FF00FF0, Expected: 0x06600F90),
                new Instruction(Name: "arm_UQADD16", Mask: 0x0FF00FF0, Expected: 0x06600F10),
                new Instruction(Name: "arm_UQASX", Mask: 0x0FF00FF0, Expected: 0x06600F30),
                new Instruction(Name: "arm_UQSAX", Mask: 0x0FF00FF0, Expected: 0x06600F50),
                new Instruction(Name: "arm_UQSUB8", Mask: 0x0FF00FF0, Expected: 0x06600FF0),
                new Instruction(Name: "arm_UQSUB16", Mask: 0x0FF00FF0, Expected: 0x06600F70),
                new Instruction(Name: "arm_SHADD8", Mask: 0x0FF00FF0, Expected: 0x06300F90),
                new Instruction(Name: "arm_SHADD16", Mask: 0x0FF00FF0, Expected: 0x06300F10),
                new Instruction(Name: "arm_SHASX", Mask: 0x0FF00FF0, Expected: 0x06300F30),
                new Instruction(Name: "arm_SHSAX", Mask: 0x0FF00FF0, Expected: 0x06300F50),
                new Instruction(Name: "arm_SHSUB8", Mask: 0x0FF00FF0, Expected: 0x06300FF0),
                new Instruction(Name: "arm_SHSUB16", Mask: 0x0FF00FF0, Expected: 0x06300F70),
                new Instruction(Name: "arm_UHADD8", Mask: 0x0FF00FF0, Expected: 0x06700F90),
                new Instruction(Name: "arm_UHADD16", Mask: 0x0FF00FF0, Expected: 0x06700F10),
                new Instruction(Name: "arm_UHASX", Mask: 0x0FF00FF0, Expected: 0x06700F30),
                new Instruction(Name: "arm_UHSAX", Mask: 0x0FF00FF0, Expected: 0x06700F50),
                new Instruction(Name: "arm_UHSUB8", Mask: 0x0FF00FF0, Expected: 0x06700FF0),
                new Instruction(Name: "arm_UHSUB16", Mask: 0x0FF00FF0, Expected: 0x06700F70),
            ]
        ),
        new
        (
            Group:    ARMGroup.MultiplyDualLong,
            Version:  ARMVersion.V6,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("Subtract", ParameterKind.Templated, [ new BitRange(6, 6), ]),
                new Parameter("X", ParameterKind.Templated, [ new BitRange(5, 5), ]),
                new Parameter("RdHi", ParameterKind.Runtime, [ new BitRange(16, 19), ]),
                new Parameter("RdLo", ParameterKind.Runtime, [ new BitRange(12, 15), ]),
                new Parameter("Rm", ParameterKind.Runtime, [ new BitRange(8, 11), ]),
                new Parameter("Rn", ParameterKind.Runtime, [ new BitRange(0, 3), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "arm_SMLALD", Mask: 0x0FF000D0, Expected: 0x07400010),
                new Instruction(Name: "arm_SMLSLD", Mask: 0x0FF000D0, Expected: 0x07400050),
            ]
        ),
        new
        (
            Group:    ARMGroup.Bitpack,
            Version:  ARMVersion.V6K,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("Order", ParameterKind.Templated, [ new BitRange(6, 6), ]),
                new Parameter("Rn", ParameterKind.Runtime, [ new BitRange(16, 19), ]),
                new Parameter("Rd", ParameterKind.Runtime, [ new BitRange(12, 15), ]),
                new Parameter("ShiftAmount", ParameterKind.Runtime, [ new BitRange(7, 11), ]),
                new Parameter("Rm", ParameterKind.Runtime, [ new BitRange(0, 3), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "arm_PKHBT", Mask: 0x0FF00070, Expected: 0x06800010),
                new Instruction(Name: "arm_PKHTB", Mask: 0x0FF00070, Expected: 0x06800050),
            ]
        ),
        new
        (
            Group:    ARMGroup.MultiplyDual,
            Version:  ARMVersion.V6,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("Subtract", ParameterKind.Templated, [ new BitRange(6, 6), ]),
                new Parameter("X", ParameterKind.Templated, [ new BitRange(5, 5), ]),
                new Parameter("Rd", ParameterKind.Runtime, [ new BitRange(16, 19), ]),
                new Parameter("Ra", ParameterKind.Runtime, [ new BitRange(12, 15), ]),
                new Parameter("Rm", ParameterKind.Runtime, [ new BitRange(8, 11), ]),
                new Parameter("Rn", ParameterKind.Runtime, [ new BitRange(0, 3), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "arm_SMUAD", Mask: 0x0FF0F0D0, Expected: 0x0700F010),
                new Instruction(Name: "arm_SMUSD", Mask: 0x0FF0F0D0, Expected: 0x0700F050),
                new Instruction(Name: "arm_SMLAD", Mask: 0x0FF000D0, Expected: 0x07000010),
                new Instruction(Name: "arm_SMLSD", Mask: 0x0FF000D0, Expected: 0x07000050),
            ]
        ),
        new
        (
            Group:    ARMGroup.MultiplyHalfwordLong,
            Version:  ARMVersion.V5E,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("RdHi", ParameterKind.Runtime, [ new BitRange(16, 19), ]),
                new Parameter("RdLo", ParameterKind.Runtime, [ new BitRange(12, 15), ]),
                new Parameter("Rm", ParameterKind.Runtime, [ new BitRange(8, 11), ]),
                new Parameter("X", ParameterKind.Runtime, [ new BitRange(6, 6), ]),
                new Parameter("Y", ParameterKind.Runtime, [ new BitRange(5, 5), ]),
                new Parameter("Rn", ParameterKind.Runtime, [ new BitRange(0, 3), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "arm_SMLALxy", Mask: 0x0FF00090, Expected: 0x01400080),
            ]
        ),
        new
        (
            Group:    ARMGroup.MultiplyWordHalfword,
            Version:  ARMVersion.V5E,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("NoAccumulate", ParameterKind.Templated, [ new BitRange(5, 5), ]),
                new Parameter("Rd", ParameterKind.Runtime, [ new BitRange(16, 19), ]),
                new Parameter("Ra", ParameterKind.Runtime, [ new BitRange(12, 15), ]),
                new Parameter("Rm", ParameterKind.Runtime, [ new BitRange(8, 11), ]),
                new Parameter("Y", ParameterKind.Runtime, [ new BitRange(6, 6), ]),
                new Parameter("Rn", ParameterKind.Runtime, [ new BitRange(0, 3), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "arm_SMULWy", Mask: 0x0FF0F0B0, Expected: 0x012000A0),
                new Instruction(Name: "arm_SMLAWy", Mask: 0x0FF000B0, Expected: 0x01200080),
            ]
        ),
        new
        (
            Group:    ARMGroup.Multiply,
            Version:  ARMVersion.V2,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("Accumulate", ParameterKind.Templated, [ new BitRange(21, 21), ]),
                new Parameter("SetFlags", ParameterKind.Templated, [ new BitRange(20, 20), ]),
                new Parameter("Rd", ParameterKind.Runtime, [ new BitRange(16, 19), ]),
                new Parameter("Ra", ParameterKind.Runtime, [ new BitRange(12, 15), ]),
                new Parameter("Rm", ParameterKind.Runtime, [ new BitRange(8, 11), ]),
                new Parameter("Rn", ParameterKind.Runtime, [ new BitRange(0, 3), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "arm_MUL", Mask: 0x0FE0F0F0, Expected: 0x00000090),
                new Instruction(Name: "arm_MLA", Mask: 0x0FE000F0, Expected: 0x00200090),
            ]
        ),
        new
        (
            Group:    ARMGroup.MultiplySignedMSW,
            Version:  ARMVersion.V6,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("Subtract", ParameterKind.Templated, [ new BitRange(6, 6), ]),
                new Parameter("Round", ParameterKind.Templated, [ new BitRange(5, 5), ]),
                new Parameter("Rd", ParameterKind.Runtime, [ new BitRange(16, 19), ]),
                new Parameter("Ra", ParameterKind.Runtime, [ new BitRange(12, 15), ]),
                new Parameter("Rm", ParameterKind.Runtime, [ new BitRange(8, 11), ]),
                new Parameter("Rn", ParameterKind.Runtime, [ new BitRange(0, 3), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "arm_SMMUL", Mask: 0x0FF0F0D0, Expected: 0x0750F010),
                new Instruction(Name: "arm_SMMLA", Mask: 0x0FF000D0, Expected: 0x07500010),
                new Instruction(Name: "arm_SMMLS", Mask: 0x0FF000D0, Expected: 0x075000D0),
            ]
        ),
        new
        (
            Group:    ARMGroup.MultiplyLong,
            Version:  ARMVersion.V3M,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("Signed", ParameterKind.Templated, [ new BitRange(22, 22), ]),
                new Parameter("Accumulate", ParameterKind.Templated, [ new BitRange(21, 21), ]),
                new Parameter("SetFlags", ParameterKind.Templated, [ new BitRange(20, 20), ]),
                new Parameter("RdHi", ParameterKind.Runtime, [ new BitRange(16, 19), ]),
                new Parameter("RdLo", ParameterKind.Runtime, [ new BitRange(12, 15), ]),
                new Parameter("Rm", ParameterKind.Runtime, [ new BitRange(8, 11), ]),
                new Parameter("Rn", ParameterKind.Runtime, [ new BitRange(0, 3), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "arm_SMLAL", Mask: 0x0FE000F0, Expected: 0x00E00090),
                new Instruction(Name: "arm_SMULL", Mask: 0x0FE000F0, Expected: 0x00C00090),
                new Instruction(Name: "arm_UMLAL", Mask: 0x0FE000F0, Expected: 0x00A00090),
                new Instruction(Name: "arm_UMULL", Mask: 0x0FE000F0, Expected: 0x00800090),
            ]
        ),
        new
        (
            Group:    ARMGroup.MultiplyHalfword,
            Version:  ARMVersion.V5E,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("NoAccumulate", ParameterKind.Templated, [ new BitRange(21, 21), ]),
                new Parameter("Rd", ParameterKind.Runtime, [ new BitRange(16, 19), ]),
                new Parameter("Ra", ParameterKind.Runtime, [ new BitRange(12, 15), ]),
                new Parameter("Rm", ParameterKind.Runtime, [ new BitRange(8, 11), ]),
                new Parameter("X", ParameterKind.Runtime, [ new BitRange(6, 6), ]),
                new Parameter("Y", ParameterKind.Runtime, [ new BitRange(5, 5), ]),
                new Parameter("Rn", ParameterKind.Runtime, [ new BitRange(0, 3), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "arm_SMULxy", Mask: 0x0FF0F090, Expected: 0x01600080),
                new Instruction(Name: "arm_SMLAxy", Mask: 0x0FF00090, Expected: 0x01000080),
            ]
        ),
        new
        (
            Group:    ARMGroup.BranchExchangeImmediate,
            Version:  ARMVersion.V5,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("H", ParameterKind.Runtime, [ new BitRange(24, 24), ]),
                new Parameter("Offset", ParameterKind.Runtime, [ new BitRange(0, 23), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "arm_BLX_imm", Mask: 0xFE000000, Expected: 0xFA000000),
            ]
        ),
        new
        (
            Group:    ARMGroup.BitwiseSaturation,
            Version:  ARMVersion.V6,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("Unsigned", ParameterKind.Templated, [ new BitRange(22, 22), ]),
                new Parameter("IsHalfword", ParameterKind.Templated, [ new BitRange(5, 5), ]),
                new Parameter("SaturationSize", ParameterKind.Runtime, [ new BitRange(16, 20), ]),
                new Parameter("Rd", ParameterKind.Runtime, [ new BitRange(12, 15), ]),
                new Parameter("Rn", ParameterKind.Runtime, [ new BitRange(0, 3), ]),
                new Parameter("Operand", ParameterKind.Runtime, [ new BitRange(6, 11), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "arm_SSAT16", Mask: 0x0FF00FF0, Expected: 0x06A00F30),
                new Instruction(Name: "arm_USAT16", Mask: 0x0FF00FF0, Expected: 0x06E00F30),
                new Instruction(Name: "arm_SSAT", Mask: 0x0FE00030, Expected: 0x06A00010),
                new Instruction(Name: "arm_USAT", Mask: 0x0FE00030, Expected: 0x06E00010),
            ]
        ),
        new
        (
            Group:    ARMGroup.DualDataTransfer,
            Version:  ARMVersion.V5E,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("PreIndex", ParameterKind.Templated, [ new BitRange(24, 24), ]),
                new Parameter("AddOffset", ParameterKind.Templated, [ new BitRange(23, 23), ]),
                new Parameter("Immediate", ParameterKind.Templated, [ new BitRange(22, 22), ]),
                new Parameter("Writeback", ParameterKind.Templated, [ new BitRange(21, 21), ]),
                new Parameter("Store", ParameterKind.Templated, [ new BitRange(5, 5), ]),
                new Parameter("Rn", ParameterKind.Runtime, [ new BitRange(16, 19), ]),
                new Parameter("Rd", ParameterKind.Runtime, [ new BitRange(12, 15), ]),
                new Parameter("Value", ParameterKind.Runtime, [ new BitRange(8, 11), new BitRange(0, 3), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "arm_LDRD_lit", Mask: 0x0F7F00F0, Expected: 0x014F00D0),
                new Instruction(Name: "arm_LDRD_reg", Mask: 0x0E500FF0, Expected: 0x000000D0),
                new Instruction(Name: "arm_STRD_reg", Mask: 0x0E500FF0, Expected: 0x000000F0),
                new Instruction(Name: "arm_LDRD_imm", Mask: 0x0E5000F0, Expected: 0x004000D0),
                new Instruction(Name: "arm_STRD_imm", Mask: 0x0E5000F0, Expected: 0x004000F0),
            ]
        ),
        new
        (
            Group:    ARMGroup.CoprocDualRegisterTransfer,
            Version:  ARMVersion.V5E,
            Override: VersionOverride.V6_Unconditional,

            Parameters:
            [
                new Parameter("Direction", ParameterKind.Templated, [ new BitRange(20, 20), ]),
                new Parameter("Rt2", ParameterKind.Runtime, [ new BitRange(16, 19), ]),
                new Parameter("Rt", ParameterKind.Runtime, [ new BitRange(12, 15), ]),
                new Parameter("CoprocID", ParameterKind.Runtime, [ new BitRange(8, 11), ]),
                new Parameter("Opcode1", ParameterKind.Runtime, [ new BitRange(4, 7), ]),
                new Parameter("CRm", ParameterKind.Runtime, [ new BitRange(0, 3), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "arm_MCRR", Mask: 0x0FF00000, Expected: 0x0C400000),
                new Instruction(Name: "arm_MRRC", Mask: 0x0FF00000, Expected: 0x0C500000),
            ]
        ),
        new
        (
            Group:    ARMGroup.CoprocSingleRegisterTransfer,
            Version:  ARMVersion.V2,
            Override: VersionOverride.V5_Unconditional,

            Parameters:
            [
                new Parameter("Direction", ParameterKind.Templated, [ new BitRange(20, 20), ]),
                new Parameter("Opcode1", ParameterKind.Runtime, [ new BitRange(21, 23), ]),
                new Parameter("CRn", ParameterKind.Runtime, [ new BitRange(16, 19), ]),
                new Parameter("Rt", ParameterKind.Runtime, [ new BitRange(12, 15), ]),
                new Parameter("CoprocID", ParameterKind.Runtime, [ new BitRange(8, 11), ]),
                new Parameter("Opcode2", ParameterKind.Runtime, [ new BitRange(5, 7), ]),
                new Parameter("CRm", ParameterKind.Runtime, [ new BitRange(0, 3), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "arm_MCR", Mask: 0x0F100010, Expected: 0x0E000010),
                new Instruction(Name: "arm_MRC", Mask: 0x0F100010, Expected: 0x0E100010),
            ]
        ),
        new
        (
            Group:    ARMGroup.CoprocDataProcessing,
            Version:  ARMVersion.V2,
            Override: VersionOverride.V5_Unconditional,

            Parameters:
            [
                new Parameter("Opcode1", ParameterKind.Runtime, [ new BitRange(20, 23), ]),
                new Parameter("CRn", ParameterKind.Runtime, [ new BitRange(16, 19), ]),
                new Parameter("CRd", ParameterKind.Runtime, [ new BitRange(12, 15), ]),
                new Parameter("CoprocID", ParameterKind.Runtime, [ new BitRange(8, 11), ]),
                new Parameter("Opcode2", ParameterKind.Runtime, [ new BitRange(5, 7), ]),
                new Parameter("CRm", ParameterKind.Runtime, [ new BitRange(0, 3), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "arm_CDP", Mask: 0x0F000010, Expected: 0x0E000000),
            ]
        ),
        new
        (
            Group:    ARMGroup.SmallSignedTransfer,
            Version:  ARMVersion.V4,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("PreIndex", ParameterKind.Templated, [ new BitRange(24, 24), ]),
                new Parameter("AddOffset", ParameterKind.Templated, [ new BitRange(23, 23), ]),
                new Parameter("Immediate", ParameterKind.Templated, [ new BitRange(22, 22), ]),
                new Parameter("Writeback", ParameterKind.Templated, [ new BitRange(21, 21), ]),
                new Parameter("Load", ParameterKind.Templated, [ new BitRange(20, 20), ]),
                new Parameter("Opcode", ParameterKind.Templated, [ new BitRange(5, 6), ]),
                new Parameter("Rn", ParameterKind.Runtime, [ new BitRange(16, 19), ]),
                new Parameter("Rd", ParameterKind.Runtime, [ new BitRange(12, 15), ]),
                new Parameter("Value", ParameterKind.Runtime, [ new BitRange(8, 11), new BitRange(0, 3), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "arm_LDRSB_lit", Mask: 0x0F7F00F0, Expected: 0x015F00D0),
                new Instruction(Name: "arm_LDRSH_lit", Mask: 0x0F7F00F0, Expected: 0x015F00F0),
                new Instruction(Name: "arm_LDRH_lit", Mask: 0x0E5F00F0, Expected: 0x005F00B0),
                new Instruction(Name: "arm_LDRH_reg", Mask: 0x0E500FF0, Expected: 0x001000B0),
                new Instruction(Name: "arm_LDRSB_reg", Mask: 0x0E500FF0, Expected: 0x001000D0),
                new Instruction(Name: "arm_LDRSH_reg", Mask: 0x0E500FF0, Expected: 0x001000F0),
                new Instruction(Name: "arm_STRH_reg", Mask: 0x0E500FF0, Expected: 0x000000B0),
                new Instruction(Name: "arm_LDRH_imm", Mask: 0x0E5000F0, Expected: 0x005000B0),
                new Instruction(Name: "arm_LDRSB_imm", Mask: 0x0E5000F0, Expected: 0x005000D0),
                new Instruction(Name: "arm_LDRSH_imm", Mask: 0x0E5000F0, Expected: 0x005000F0),
                new Instruction(Name: "arm_STRH_imm", Mask: 0x0E5000F0, Expected: 0x004000B0),
            ]
        ),
        new
        (
            Group:    ARMGroup.StatusTransfer,
            Version:  ARMVersion.V3,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("Immediate", ParameterKind.Templated, [ new BitRange(25, 25), ]),
                new Parameter("TargetRegister", ParameterKind.Templated, [ new BitRange(22, 22), ]),
                new Parameter("Mode", ParameterKind.Templated, [ new BitRange(21, 21), ]),
                new Parameter("Mask", ParameterKind.Runtime, [ new BitRange(16, 19), ]),
                new Parameter("Rd", ParameterKind.Runtime, [ new BitRange(12, 15), ]),
                new Parameter("Value", ParameterKind.Runtime, [ new BitRange(0, 11), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "arm_MRS", Mask: 0x0FBF0FFF, Expected: 0x010F0000),
                new Instruction(Name: "arm_MSR_reg", Mask: 0x0FB0FFF0, Expected: 0x0120F000),
                new Instruction(Name: "arm_MSR_imm", Mask: 0x0FB0F000, Expected: 0x0320F000),
            ]
        ),
        new
        (
            Group:    ARMGroup.SupervisorCall,
            Version:  ARMVersion.V1,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("Immediate", ParameterKind.Runtime, [ new BitRange(0, 23), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "arm_SVC", Mask: 0x0F000000, Expected: 0x0F000000),
            ]
        ),
        new
        (
            Group:    ARMGroup.BranchWithLink,
            Version:  ARMVersion.V1,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("Link", ParameterKind.Templated, [ new BitRange(24, 24), ]),
                new Parameter("Offset", ParameterKind.Runtime, [ new BitRange(0, 23), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "arm_B", Mask: 0x0F000000, Expected: 0x0A000000),
                new Instruction(Name: "arm_BL", Mask: 0x0F000000, Expected: 0x0B000000),
            ]
        ),
        new
        (
            Group:    ARMGroup.CoprocLoadStore,
            Version:  ARMVersion.V2,
            Override: VersionOverride.V5_Unconditional,

            Parameters:
            [
                new Parameter("PreIndexed", ParameterKind.Templated, [ new BitRange(24, 24), ]),
                new Parameter("Add", ParameterKind.Templated, [ new BitRange(23, 23), ]),
                new Parameter("DoubleTransfer", ParameterKind.Templated, [ new BitRange(22, 22), ]),
                new Parameter("Writeback", ParameterKind.Templated, [ new BitRange(21, 21), ]),
                new Parameter("Load", ParameterKind.Templated, [ new BitRange(20, 20), ]),
                new Parameter("Rn", ParameterKind.Runtime, [ new BitRange(16, 19), ]),
                new Parameter("CRd", ParameterKind.Runtime, [ new BitRange(12, 15), ]),
                new Parameter("CoprocID", ParameterKind.Runtime, [ new BitRange(8, 11), ]),
                new Parameter("Value", ParameterKind.Runtime, [ new BitRange(0, 7), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "arm_LDC", Mask: 0x0E100000, Expected: 0x0C100000),
                new Instruction(Name: "arm_STC", Mask: 0x0E100000, Expected: 0x0C000000),
            ]
        ),
        new
        (
            Group:    ARMGroup.BlockDataTransfer,
            Version:  ARMVersion.V1,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("PreIndexed", ParameterKind.Templated, [ new BitRange(24, 24), ]),
                new Parameter("Add", ParameterKind.Templated, [ new BitRange(23, 23), ]),
                new Parameter("UserMode", ParameterKind.Templated, [ new BitRange(22, 22), ]),
                new Parameter("Writeback", ParameterKind.Templated, [ new BitRange(21, 21), ]),
                new Parameter("Load", ParameterKind.Templated, [ new BitRange(20, 20), ]),
                new Parameter("Rn", ParameterKind.Runtime, [ new BitRange(16, 19), ]),
                new Parameter("RegList", ParameterKind.Runtime, [ new BitRange(0, 15), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "arm_LDM", Mask: 0x0FD00000, Expected: 0x08900000),
                new Instruction(Name: "arm_LDMDA", Mask: 0x0FD00000, Expected: 0x08100000),
                new Instruction(Name: "arm_LDMDB", Mask: 0x0FD00000, Expected: 0x09100000),
                new Instruction(Name: "arm_LDMIB", Mask: 0x0FD00000, Expected: 0x09900000),
                new Instruction(Name: "arm_LDM_usr", Mask: 0x0E708000, Expected: 0x08500000),
                new Instruction(Name: "arm_STM", Mask: 0x0FD00000, Expected: 0x08800000),
                new Instruction(Name: "arm_STMDA", Mask: 0x0FD00000, Expected: 0x08000000),
                new Instruction(Name: "arm_STMDB", Mask: 0x0FD00000, Expected: 0x09000000),
                new Instruction(Name: "arm_STMIB", Mask: 0x0FD00000, Expected: 0x09800000),
                new Instruction(Name: "arm_LDM_eret", Mask: 0x0E508000, Expected: 0x08508000),
                new Instruction(Name: "arm_STM_usr", Mask: 0x0E700000, Expected: 0x08400000),
                new Instruction(Name: "arm_STM_usr_wb", Mask: 0x0E700000, Expected: 0x08600000),
            ]
        ),
        new
        (
            Group:    ARMGroup.SingleDataTransfer,
            Version:  ARMVersion.V1,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("RegisterOffset", ParameterKind.Templated, [ new BitRange(25, 25), ]),
                new Parameter("PreIndex", ParameterKind.Templated, [ new BitRange(24, 24), ]),
                new Parameter("AddOffset", ParameterKind.Templated, [ new BitRange(23, 23), ]),
                new Parameter("ByteMode", ParameterKind.Templated, [ new BitRange(22, 22), ]),
                new Parameter("Writeback", ParameterKind.Templated, [ new BitRange(21, 21), ]),
                new Parameter("Load", ParameterKind.Templated, [ new BitRange(20, 20), ]),
                new Parameter("Rn", ParameterKind.Runtime, [ new BitRange(16, 19), ]),
                new Parameter("Rd", ParameterKind.Runtime, [ new BitRange(12, 15), ]),
                new Parameter("Value", ParameterKind.Runtime, [ new BitRange(0, 11), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "arm_PLD_reg", Mask: 0xFF30F010, Expected: 0xF710F000),
                new Instruction(Name: "arm_PLD_imm", Mask: 0xFF30F000, Expected: 0xF510F000),
                new Instruction(Name: "arm_LDR_lit", Mask: 0x0F7F0000, Expected: 0x051F0000),
                new Instruction(Name: "arm_LDRB_lit", Mask: 0x0F7F0000, Expected: 0x055F0000),
                new Instruction(Name: "arm_LDR_reg", Mask: 0x0E500010, Expected: 0x06100000),
                new Instruction(Name: "arm_LDRB_reg", Mask: 0x0E500010, Expected: 0x06500000),
                new Instruction(Name: "arm_STR_reg", Mask: 0x0E500010, Expected: 0x06000000),
                new Instruction(Name: "arm_STRB_reg", Mask: 0x0E500010, Expected: 0x06400000),
                new Instruction(Name: "arm_LDR_imm", Mask: 0x0E500000, Expected: 0x04100000),
                new Instruction(Name: "arm_LDRB_imm", Mask: 0x0E500000, Expected: 0x04500000),
                new Instruction(Name: "arm_STR_imm", Mask: 0x0E500000, Expected: 0x04000000),
                new Instruction(Name: "arm_STRB_imm", Mask: 0x0E500000, Expected: 0x04400000),
            ]
        ),
        new
        (
            Group:    ARMGroup.DataProcessing,
            Version:  ARMVersion.V1,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("Immediate", ParameterKind.Templated, [ new BitRange(25, 25), ]),
                new Parameter("Opcode", ParameterKind.Templated, [ new BitRange(21, 24), ]),
                new Parameter("SetFlags", ParameterKind.Templated, [ new BitRange(20, 20), ]),
                new Parameter("Rn", ParameterKind.Runtime, [ new BitRange(16, 19), ]),
                new Parameter("Rd", ParameterKind.Runtime, [ new BitRange(12, 15), ]),
                new Parameter("Operand2", ParameterKind.Runtime, [ new BitRange(0, 11), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "arm_CMN_rsr", Mask: 0x0FF0F090, Expected: 0x01700010),
                new Instruction(Name: "arm_CMP_rsr", Mask: 0x0FF0F090, Expected: 0x01500010),
                new Instruction(Name: "arm_TEQ_rsr", Mask: 0x0FF0F090, Expected: 0x01300010),
                new Instruction(Name: "arm_TST_rsr", Mask: 0x0FF0F090, Expected: 0x01100010),
                new Instruction(Name: "arm_CMN_reg", Mask: 0x0FF0F010, Expected: 0x01700000),
                new Instruction(Name: "arm_CMP_reg", Mask: 0x0FF0F010, Expected: 0x01500000),
                new Instruction(Name: "arm_MOV_rsr", Mask: 0x0FEF0090, Expected: 0x01A00010),
                new Instruction(Name: "arm_MVN_rsr", Mask: 0x0FEF0090, Expected: 0x01E00010),
                new Instruction(Name: "arm_TEQ_reg", Mask: 0x0FF0F010, Expected: 0x01300000),
                new Instruction(Name: "arm_TST_reg", Mask: 0x0FF0F010, Expected: 0x01100000),
                new Instruction(Name: "arm_CMN_imm", Mask: 0x0FF0F000, Expected: 0x03700000),
                new Instruction(Name: "arm_CMP_imm", Mask: 0x0FF0F000, Expected: 0x03500000),
                new Instruction(Name: "arm_MOV_reg", Mask: 0x0FEF0010, Expected: 0x01A00000),
                new Instruction(Name: "arm_MVN_reg", Mask: 0x0FEF0010, Expected: 0x01E00000),
                new Instruction(Name: "arm_TEQ_imm", Mask: 0x0FF0F000, Expected: 0x03300000),
                new Instruction(Name: "arm_TST_imm", Mask: 0x0FF0F000, Expected: 0x03100000),
                new Instruction(Name: "arm_MOV_imm", Mask: 0x0FEF0000, Expected: 0x03A00000),
                new Instruction(Name: "arm_MVN_imm", Mask: 0x0FEF0000, Expected: 0x03E00000),
                new Instruction(Name: "arm_ADC_rsr", Mask: 0x0FE00090, Expected: 0x00A00010),
                new Instruction(Name: "arm_ADD_rsr", Mask: 0x0FE00090, Expected: 0x00800010),
                new Instruction(Name: "arm_AND_rsr", Mask: 0x0FE00090, Expected: 0x00000010),
                new Instruction(Name: "arm_BIC_rsr", Mask: 0x0FE00090, Expected: 0x01C00010),
                new Instruction(Name: "arm_EOR_rsr", Mask: 0x0FE00090, Expected: 0x00200010),
                new Instruction(Name: "arm_ORR_rsr", Mask: 0x0FE00090, Expected: 0x01800010),
                new Instruction(Name: "arm_RSB_rsr", Mask: 0x0FE00090, Expected: 0x00600010),
                new Instruction(Name: "arm_RSC_rsr", Mask: 0x0FE00090, Expected: 0x00E00010),
                new Instruction(Name: "arm_SBC_rsr", Mask: 0x0FE00090, Expected: 0x00C00010),
                new Instruction(Name: "arm_SUB_rsr", Mask: 0x0FE00090, Expected: 0x00400010),
                new Instruction(Name: "arm_ADC_reg", Mask: 0x0FE00010, Expected: 0x00A00000),
                new Instruction(Name: "arm_ADD_reg", Mask: 0x0FE00010, Expected: 0x00800000),
                new Instruction(Name: "arm_AND_reg", Mask: 0x0FE00010, Expected: 0x00000000),
                new Instruction(Name: "arm_BIC_reg", Mask: 0x0FE00010, Expected: 0x01C00000),
                new Instruction(Name: "arm_EOR_reg", Mask: 0x0FE00010, Expected: 0x00200000),
                new Instruction(Name: "arm_ORR_reg", Mask: 0x0FE00010, Expected: 0x01800000),
                new Instruction(Name: "arm_RSB_reg", Mask: 0x0FE00010, Expected: 0x00600000),
                new Instruction(Name: "arm_RSC_reg", Mask: 0x0FE00010, Expected: 0x00E00000),
                new Instruction(Name: "arm_SBC_reg", Mask: 0x0FE00010, Expected: 0x00C00000),
                new Instruction(Name: "arm_SUB_reg", Mask: 0x0FE00010, Expected: 0x00400000),
                new Instruction(Name: "arm_ADC_imm", Mask: 0x0FE00000, Expected: 0x02A00000),
                new Instruction(Name: "arm_ADD_imm", Mask: 0x0FE00000, Expected: 0x02800000),
                new Instruction(Name: "arm_AND_imm", Mask: 0x0FE00000, Expected: 0x02000000),
                new Instruction(Name: "arm_BIC_imm", Mask: 0x0FE00000, Expected: 0x03C00000),
                new Instruction(Name: "arm_EOR_imm", Mask: 0x0FE00000, Expected: 0x02200000),
                new Instruction(Name: "arm_ORR_imm", Mask: 0x0FE00000, Expected: 0x03800000),
                new Instruction(Name: "arm_RSB_imm", Mask: 0x0FE00000, Expected: 0x02600000),
                new Instruction(Name: "arm_RSC_imm", Mask: 0x0FE00000, Expected: 0x02E00000),
                new Instruction(Name: "arm_SBC_imm", Mask: 0x0FE00000, Expected: 0x02C00000),
                new Instruction(Name: "arm_SUB_imm", Mask: 0x0FE00000, Expected: 0x02400000),
            ]
        ),
    ];
}

internal enum ARMGroup
{
    Bitpack,
    BitReverse,
    BitwiseSaturation,
    BlockDataTransfer,
    BranchExchange,
    BranchExchangeImmediate,
    BranchWithLink,
    Breakpoint,
    ByteSelect,
    ChangeState,
    ClearExclusive,
    CoprocDataProcessing,
    CoprocDualRegisterTransfer,
    CoprocLoadStore,
    CoprocSingleRegisterTransfer,
    CountLeadingZeros,
    DataProcessing,
    DualDataTransfer,
    ExclusiveLoad,
    ExclusiveStore,
    Extension,
    HintStatusCollision,
    Multiply,
    MultiplyDual,
    MultiplyDualLong,
    MultiplyHalfword,
    MultiplyHalfwordLong,
    MultiplyLong,
    MultiplyLongDoubleAccumulate,
    MultiplySignedMSW,
    MultiplyWordHalfword,
    NoOperation,
    ParallelArithmetic,
    Preload,
    ReturnFromException,
    SaturatedArithmetic,
    SecureMonitorCall,
    SendEvent,
    SetEndianness,
    SingleDataTransfer,
    SmallSignedTransfer,
    StatusTransfer,
    StoreReturnState,
    SumAbsoluteDifferences,
    SupervisorCall,
    Swap,
    WaitForEvent,
    WaitForInterrupt,
    Yield
}