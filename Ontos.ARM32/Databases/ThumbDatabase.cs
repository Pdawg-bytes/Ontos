using Ontos.Core.ARM32;

namespace Ontos.ARM32.Databases;

internal static partial class InstructionDatabase
{
    internal static readonly GroupDefinition<ThumbGroup>[] ThumbDatabase =
    [
        new
        (
            Group:    ThumbGroup.SetEndianness,
            Version:  ARMVersion.V6,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("Endianness", ParameterKind.Runtime, [ new BitRange(3, 3), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "thumb_SETEND", Mask: 0xFFF7, Expected: 0xB650),
            ]
        ),
        new
        (
            Group:    ThumbGroup.ChangeState,
            Version:  ARMVersion.V6,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("Imod", ParameterKind.Runtime, [ new BitRange(4, 4), ]),
                new Parameter("A", ParameterKind.Runtime, [ new BitRange(2, 2), ]),
                new Parameter("I", ParameterKind.Runtime, [ new BitRange(1, 1), ]),
                new Parameter("F", ParameterKind.Runtime, [ new BitRange(0, 0), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "thumb_CPS", Mask: 0xFFE8, Expected: 0xB660),
            ]
        ),
        new
        (
            Group:    ThumbGroup.BranchExchange,
            Version:  ARMVersion.V4T,
            Override: VersionOverride.V5T_Thumb_Link,

            Parameters:
            [
                new Parameter("Link", ParameterKind.Templated, [ new BitRange(7, 7), ]),
                new Parameter("Rm", ParameterKind.Runtime, [ new BitRange(3, 6), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "thumb_BLX_2", Mask: 0xFF87, Expected: 0x4780),
                new Instruction(Name: "thumb_BX", Mask: 0xFF87, Expected: 0x4700),
            ]
        ),
        new
        (
            Group:    ThumbGroup.SoftwareInterrupt,
            Version:  ARMVersion.V4T,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("Immediate", ParameterKind.Runtime, [ new BitRange(0, 7), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "thumb_SWI", Mask: 0xFF00, Expected: 0xDF00),
            ]
        ),
        new
        (
            Group:    ThumbGroup.Breakpoint,
            Version:  ARMVersion.V5,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("Immediate", ParameterKind.Runtime, [ new BitRange(0, 7), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "thumb_BKPT", Mask: 0xFF00, Expected: 0xBE00),
            ]
        ),
        new
        (
            Group:    ThumbGroup.ByteReverse,
            Version:  ARMVersion.V6,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("Opcode", ParameterKind.Templated, [ new BitRange(6, 7), ]),
                new Parameter("Rn", ParameterKind.Runtime, [ new BitRange(3, 5), ]),
                new Parameter("Rd", ParameterKind.Runtime, [ new BitRange(0, 2), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "thumb_REV", Mask: 0xFFC0, Expected: 0xBA00),
                new Instruction(Name: "thumb_REV16", Mask: 0xFFC0, Expected: 0xBA40),
                new Instruction(Name: "thumb_REVSH", Mask: 0xFFC0, Expected: 0xBAC0),
            ]
        ),
        new
        (
            Group:    ThumbGroup.SignZeroExtend,
            Version:  ARMVersion.V6,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("Opcode", ParameterKind.Templated, [ new BitRange(6, 7), ]),
                new Parameter("Rm", ParameterKind.Runtime, [ new BitRange(3, 5), ]),
                new Parameter("Rd", ParameterKind.Runtime, [ new BitRange(0, 2), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "thumb_SXTB", Mask: 0xFFC0, Expected: 0xB240),
                new Instruction(Name: "thumb_SXTH", Mask: 0xFFC0, Expected: 0xB200),
                new Instruction(Name: "thumb_UXTB", Mask: 0xFFC0, Expected: 0xB2C0),
                new Instruction(Name: "thumb_UXTH", Mask: 0xFFC0, Expected: 0xB280),
            ]
        ),
        new
        (
            Group:    ThumbGroup.AdjustSP,
            Version:  ARMVersion.V4T,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("Subtract", ParameterKind.Templated, [ new BitRange(7, 7), ]),
                new Parameter("Immediate", ParameterKind.Runtime, [ new BitRange(0, 6), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "thumb_ADD_7", Mask: 0xFF80, Expected: 0xB000),
                new Instruction(Name: "thumb_SUB_4", Mask: 0xFF80, Expected: 0xB080),
            ]
        ),
        new
        (
            Group:    ThumbGroup.PushPop,
            Version:  ARMVersion.V4T,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("Load", ParameterKind.Templated, [ new BitRange(11, 11), ]),
                new Parameter("R", ParameterKind.Templated, [ new BitRange(8, 8), ]),
                new Parameter("RegisterList", ParameterKind.Runtime, [ new BitRange(0, 7), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "thumb_POP", Mask: 0xFE00, Expected: 0xBC00),
                new Instruction(Name: "thumb_PUSH", Mask: 0xFE00, Expected: 0xB400),
            ]
        ),
        new
        (
            Group:    ThumbGroup.HighRegisterOps,
            Version:  ARMVersion.V4T,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("Opcode", ParameterKind.Templated, [ new BitRange(8, 9), ]),
                new Parameter("Rm", ParameterKind.Runtime, [ new BitRange(3, 6), ]),
                new Parameter("Rd", ParameterKind.Runtime, [ new BitRange(7, 7), new BitRange(0, 2), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "thumb_ADD_4", Mask: 0xFF00, Expected: 0x4400),
                new Instruction(Name: "thumb_CMP_3", Mask: 0xFF00, Expected: 0x4500),
                new Instruction(Name: "thumb_CPY", Mask: 0xFF00, Expected: 0x4600),
                new Instruction(Name: "thumb_MOV_3", Mask: 0xFF00, Expected: 0x4600),
            ]
        ),
        new
        (
            Group:    ThumbGroup.DataProcessing,
            Version:  ARMVersion.V4T,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("Opcode", ParameterKind.Templated, [ new BitRange(6, 9), ]),
                new Parameter("Rms", ParameterKind.Runtime, [ new BitRange(3, 5), ]),
                new Parameter("Rdn", ParameterKind.Runtime, [ new BitRange(0, 2), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "thumb_ADC", Mask: 0xFFC0, Expected: 0x4140),
                new Instruction(Name: "thumb_AND", Mask: 0xFFC0, Expected: 0x4000),
                new Instruction(Name: "thumb_ASR_2", Mask: 0xFFC0, Expected: 0x4100),
                new Instruction(Name: "thumb_BIC", Mask: 0xFFC0, Expected: 0x4380),
                new Instruction(Name: "thumb_CMN", Mask: 0xFFC0, Expected: 0x42C0),
                new Instruction(Name: "thumb_CMP_2", Mask: 0xFFC0, Expected: 0x4280),
                new Instruction(Name: "thumb_EOR", Mask: 0xFFC0, Expected: 0x4040),
                new Instruction(Name: "thumb_LSL_2", Mask: 0xFFC0, Expected: 0x4080),
                new Instruction(Name: "thumb_LSR_2", Mask: 0xFFC0, Expected: 0x40C0),
                new Instruction(Name: "thumb_MUL", Mask: 0xFFC0, Expected: 0x4340),
                new Instruction(Name: "thumb_MVN", Mask: 0xFFC0, Expected: 0x43C0),
                new Instruction(Name: "thumb_NEG", Mask: 0xFFC0, Expected: 0x4240),
                new Instruction(Name: "thumb_ORR", Mask: 0xFFC0, Expected: 0x4300),
                new Instruction(Name: "thumb_ROR", Mask: 0xFFC0, Expected: 0x41C0),
                new Instruction(Name: "thumb_SBC", Mask: 0xFFC0, Expected: 0x4180),
                new Instruction(Name: "thumb_TST", Mask: 0xFFC0, Expected: 0x4200),
            ]
        ),
        new
        (
            Group:    ThumbGroup.UnconditionalBranch,
            Version:  ARMVersion.V4T,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("Immediate", ParameterKind.Runtime, [ new BitRange(0, 10), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "thumb_B_2", Mask: 0xF800, Expected: 0xE000),
            ]
        ),
        new
        (
            Group:    ThumbGroup.AddSubtract3Bit,
            Version:  ARMVersion.V4T,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("Immediate", ParameterKind.Templated, [ new BitRange(10, 10), ]),
                new Parameter("Subtract", ParameterKind.Templated, [ new BitRange(9, 9), ]),
                new Parameter("Operand", ParameterKind.Runtime, [ new BitRange(6, 8), ]),
                new Parameter("Rn", ParameterKind.Runtime, [ new BitRange(3, 5), ]),
                new Parameter("Rd", ParameterKind.Runtime, [ new BitRange(0, 2), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "thumb_MOV_2", Mask: 0xFFC0, Expected: 0x1C00),
                new Instruction(Name: "thumb_ADD_1", Mask: 0xFE00, Expected: 0x1C00),
                new Instruction(Name: "thumb_ADD_3", Mask: 0xFE00, Expected: 0x1800),
                new Instruction(Name: "thumb_SUB_1", Mask: 0xFE00, Expected: 0x1E00),
                new Instruction(Name: "thumb_SUB_3", Mask: 0xFE00, Expected: 0x1A00),
            ]
        ),
        new
        (
            Group:    ThumbGroup.PCRelativeLoad,
            Version:  ARMVersion.V4T,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("Rd", ParameterKind.Runtime, [ new BitRange(8, 10), ]),
                new Parameter("Immediate", ParameterKind.Runtime, [ new BitRange(0, 7), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "thumb_LDR_3", Mask: 0xF800, Expected: 0x4800),
            ]
        ),
        new
        (
            Group:    ThumbGroup.ConditionalBranch,
            Version:  ARMVersion.V4T,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("Condition", ParameterKind.Runtime, [ new BitRange(8, 11), ]),
                new Parameter("Immediate", ParameterKind.Runtime, [ new BitRange(0, 7), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "thumb_B_1", Mask: 0xF000, Expected: 0xD000),
            ]
        ),
        new
        (
            Group:    ThumbGroup.LoadAddress,
            Version:  ARMVersion.V4T,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("Source", ParameterKind.Templated, [ new BitRange(11, 11), ]),
                new Parameter("Rd", ParameterKind.Runtime, [ new BitRange(8, 10), ]),
                new Parameter("Immediate", ParameterKind.Runtime, [ new BitRange(0, 7), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "thumb_ADD_5", Mask: 0xF800, Expected: 0xA000),
                new Instruction(Name: "thumb_ADD_6", Mask: 0xF800, Expected: 0xA800),
            ]
        ),
        new
        (
            Group:    ThumbGroup.RegisterOffsetTransfer,
            Version:  ARMVersion.V4T,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("Opcode", ParameterKind.Templated, [ new BitRange(9, 11), ]),
                new Parameter("Rm", ParameterKind.Runtime, [ new BitRange(6, 8), ]),
                new Parameter("Rn", ParameterKind.Runtime, [ new BitRange(3, 5), ]),
                new Parameter("Rd", ParameterKind.Runtime, [ new BitRange(0, 2), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "thumb_LDR_2", Mask: 0xFE00, Expected: 0x5800),
                new Instruction(Name: "thumb_LDRB_2", Mask: 0xFE00, Expected: 0x5C00),
                new Instruction(Name: "thumb_LDRH_2", Mask: 0xFE00, Expected: 0x5A00),
                new Instruction(Name: "thumb_LDRSB", Mask: 0xFE00, Expected: 0x5600),
                new Instruction(Name: "thumb_LDRSH", Mask: 0xFE00, Expected: 0x5E00),
                new Instruction(Name: "thumb_STR_2", Mask: 0xFE00, Expected: 0x5000),
                new Instruction(Name: "thumb_STRB_2", Mask: 0xFE00, Expected: 0x5400),
                new Instruction(Name: "thumb_STRH_2", Mask: 0xFE00, Expected: 0x5200),
            ]
        ),
        new
        (
            Group:    ThumbGroup.StackRelativeTransfer,
            Version:  ARMVersion.V4T,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("Load", ParameterKind.Templated, [ new BitRange(11, 11), ]),
                new Parameter("Rd", ParameterKind.Runtime, [ new BitRange(8, 10), ]),
                new Parameter("Immediate", ParameterKind.Runtime, [ new BitRange(0, 7), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "thumb_LDR_4", Mask: 0xF800, Expected: 0x9800),
                new Instruction(Name: "thumb_STR_3", Mask: 0xF800, Expected: 0x9000),
            ]
        ),
        new
        (
            Group:    ThumbGroup.BlockDataTransfer,
            Version:  ARMVersion.V4T,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("Load", ParameterKind.Templated, [ new BitRange(11, 11), ]),
                new Parameter("Rn", ParameterKind.Runtime, [ new BitRange(8, 10), ]),
                new Parameter("RegisterList", ParameterKind.Runtime, [ new BitRange(0, 7), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "thumb_LDMIA", Mask: 0xF800, Expected: 0xC800),
                new Instruction(Name: "thumb_STMIA", Mask: 0xF800, Expected: 0xC000),
            ]
        ),
        new
        (
            Group:    ThumbGroup.HalfwordImmediateTransfer,
            Version:  ARMVersion.V4T,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("Load", ParameterKind.Templated, [ new BitRange(11, 11), ]),
                new Parameter("Immediate", ParameterKind.Runtime, [ new BitRange(6, 10), ]),
                new Parameter("Rn", ParameterKind.Runtime, [ new BitRange(3, 5), ]),
                new Parameter("Rd", ParameterKind.Runtime, [ new BitRange(0, 2), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "thumb_LDRH_1", Mask: 0xF800, Expected: 0x8800),
                new Instruction(Name: "thumb_STRH_1", Mask: 0xF800, Expected: 0x8000),
            ]
        ),
        new
        (
            Group:    ThumbGroup.LongBranchWithLink,
            Version:  ARMVersion.V4T,
            Override: VersionOverride.V5T_Thumb_LongBLX,

            Parameters:
            [
                new Parameter("H", ParameterKind.Templated, [ new BitRange(11, 12), ]),
                new Parameter("Offset", ParameterKind.Runtime, [ new BitRange(0, 10), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "thumb_BL_BLX_1", Mask: 0xE000, Expected: 0xE000),
            ]
        ),
        new
        (
            Group:    ThumbGroup.ImmediateOffsetTransfer,
            Version:  ARMVersion.V4T,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("ByteMode", ParameterKind.Templated, [ new BitRange(12, 12), ]),
                new Parameter("Load", ParameterKind.Templated, [ new BitRange(11, 11), ]),
                new Parameter("Immediate", ParameterKind.Runtime, [ new BitRange(6, 10), ]),
                new Parameter("Rn", ParameterKind.Runtime, [ new BitRange(3, 5), ]),
                new Parameter("Rd", ParameterKind.Runtime, [ new BitRange(0, 2), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "thumb_LDR_1", Mask: 0xF800, Expected: 0x6800),
                new Instruction(Name: "thumb_LDRB_1", Mask: 0xF800, Expected: 0x7800),
                new Instruction(Name: "thumb_STR_1", Mask: 0xF800, Expected: 0x6000),
                new Instruction(Name: "thumb_STRB_1", Mask: 0xF800, Expected: 0x7000),
            ]
        ),
        new
        (
            Group:    ThumbGroup.ImmediateALU,
            Version:  ARMVersion.V4T,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("Opcode", ParameterKind.Templated, [ new BitRange(11, 12), ]),
                new Parameter("Rdn", ParameterKind.Runtime, [ new BitRange(8, 10), ]),
                new Parameter("Immediate", ParameterKind.Runtime, [ new BitRange(0, 7), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "thumb_ADD_2", Mask: 0xF800, Expected: 0x3000),
                new Instruction(Name: "thumb_CMP_1", Mask: 0xF800, Expected: 0x2800),
                new Instruction(Name: "thumb_MOV_1", Mask: 0xF800, Expected: 0x2000),
                new Instruction(Name: "thumb_SUB_2", Mask: 0xF800, Expected: 0x3800),
            ]
        ),
        new
        (
            Group:    ThumbGroup.ShiftImmediate,
            Version:  ARMVersion.V4T,
            Override: VersionOverride.None,

            Parameters:
            [
                new Parameter("Opcode", ParameterKind.Templated, [ new BitRange(11, 12), ]),
                new Parameter("Immediate", ParameterKind.Runtime, [ new BitRange(6, 10), ]),
                new Parameter("Rm", ParameterKind.Runtime, [ new BitRange(3, 5), ]),
                new Parameter("Rd", ParameterKind.Runtime, [ new BitRange(0, 2), ]),
            ],
            Instructions:
            [
                new Instruction(Name: "thumb_ASR_1", Mask: 0xF800, Expected: 0x1000),
                new Instruction(Name: "thumb_LSL_1", Mask: 0xF800, Expected: 0x0000),
                new Instruction(Name: "thumb_LSR_1", Mask: 0xF800, Expected: 0x0800),
            ]
        ),
    ];
}

internal enum ThumbGroup
{
    AddSubtract3Bit,
    AdjustSP,
    BlockDataTransfer,
    BranchExchange,
    Breakpoint,
    ByteReverse,
    ChangeState,
    ConditionalBranch,
    DataProcessing,
    HalfwordImmediateTransfer,
    HighRegisterOps,
    ImmediateALU,
    ImmediateOffsetTransfer,
    LoadAddress,
    LongBranchWithLink,
    PCRelativeLoad,
    PushPop,
    RegisterOffsetTransfer,
    SetEndianness,
    ShiftImmediate,
    SignZeroExtend,
    SoftwareInterrupt,
    StackRelativeTransfer,
    UnconditionalBranch
}