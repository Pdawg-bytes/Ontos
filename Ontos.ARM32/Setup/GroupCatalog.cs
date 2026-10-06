using System;
using System.Linq;
using Ontos.Core.ARM32;
using Ontos.ARM32.Databases;
using System.Collections.Generic;
using Microsoft.CodeAnalysis.CSharp;

namespace Ontos.ARM32.Setup;

internal enum InstructionSet : byte
{
    ARM,
    Thumb
}

internal sealed class ParameterInfo(string name, BitRange[] ranges)
{
    internal string Name       { get; } = name;
    internal BitRange[] Ranges { get; } = ranges;
    internal int Width         { get; } = ranges.Sum(r => r.Hi - r.Lo + 1);

    internal string ArgName { get; } = ToArgName(name);

    internal string TypeName => Width == 1 ? "bool" : Width <= 8 ? "byte" : Width <= 16 ? "ushort" : "uint";

    private static string ToArgName(string name)
    {
        string arg = char.ToLowerInvariant(name[0]) + name.Substring(1);
        return SyntaxFacts.GetKeywordKind(arg) != SyntaxKind.None ? "@" + arg : arg;
    }
}

internal sealed class GroupInfo(
    InstructionSet set,
    string name,
    ARMVersion version,
    VersionOverride versionOverride,
    ParameterInfo[] templated,
    ParameterInfo[] runtime,
    Instruction[] instructions)
{
    internal InstructionSet Set         { get; } = set;
    internal string Name                { get; } = name;
    internal ARMVersion Version         { get; } = version;
    internal VersionOverride Override   { get; } = versionOverride;
    internal ParameterInfo[] Templated  { get; } = templated;
    internal ParameterInfo[] Runtime    { get; } = runtime;
    internal Instruction[] Instructions { get; } = instructions;

    internal string Prefix         => PrefixOf(Set);
    internal string MethodName     => Prefix + "_" + Name;
    internal string TraitInterface => "I" + MethodName;
    internal string WrapperName    => "__wrap_" + MethodName;

    internal bool IsCollisionMarker => Set == InstructionSet.ARM && Name == "HintStatusCollision";

    internal static string PrefixOf(InstructionSet set) => set == InstructionSet.ARM ? "ARM" : "Thumb";
}

internal sealed record OverrideRule(ARMVersion Since, string? Parameter, bool NonZero);

internal static class GroupCatalog
{
    internal static readonly GroupInfo[] ARM   = Build(InstructionSet.ARM, InstructionDatabase.ARMDatabase);
    internal static readonly GroupInfo[] Thumb = Build(InstructionSet.Thumb, InstructionDatabase.ThumbDatabase);

    private static readonly Dictionary<(InstructionSet, string), GroupInfo> _byName =
        ARM.Concat(Thumb).ToDictionary(g => (g.Set, g.Name));

    internal static GroupInfo Find(InstructionSet set, string name) => _byName[(set, name)];
    internal static GroupInfo[] Of(InstructionSet set)              => set == InstructionSet.ARM ? ARM : Thumb;

    internal static GroupInfo[] GroupsFor(InstructionSet set, ARMVersion version) =>
        [..Of(set).Where(g => g.Version <= version && !g.IsCollisionMarker)];


    internal static OverrideRule? RuleFor(VersionOverride o) => o switch
    {
        VersionOverride.V5_Link           => new(ARMVersion.V5,  "Link", false),
        VersionOverride.V5_Unconditional  => new(ARMVersion.V5,  null,   false),
        VersionOverride.V6_Unconditional  => new(ARMVersion.V6,  null,   false),
        VersionOverride.V6K_SizeNotZero   => new(ARMVersion.V6K, "Size", true),
        VersionOverride.V5T_Thumb_LongBLX => new(ARMVersion.V5,  "H",    false),
        VersionOverride.V5T_Thumb_Link    => new(ARMVersion.V5,  "Link", false),
        _                                 => null
    };

    private static GroupInfo[] Build<T>(InstructionSet set, GroupDefinition<T>[] db) where T : Enum
    {
        return [..db.Select(d => new GroupInfo(
            set,
            d.Group.ToString(),
            d.Version,
            d.Override,
            [..d.Parameters.Where(p => p.Kind == ParameterKind.Templated).Select(p => new ParameterInfo(p.Name, p.Ranges))],
            [..d.Parameters.Where(p => p.Kind == ParameterKind.Runtime).Select(p => new ParameterInfo(p.Name, p.Ranges))],
            d.Instructions
        ))];
    }
}