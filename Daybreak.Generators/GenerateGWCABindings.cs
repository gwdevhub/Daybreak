using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using static Daybreak.Generators.MsvcDemangler;
using TypeKind = Daybreak.Generators.MsvcDemangler.TypeKind;

namespace Daybreak.Generators;

/// <summary>
/// Incremental source generator that reads PE exports from gwca.dll,
/// demangles the MSVC-decorated C++ names to recover type information,
/// and emits a C# <c>GWCA</c> static partial class containing
/// <c>[LibraryImport]</c> declarations for every C-linkage export.
///
/// Types annotated with <see cref="GWCAEquivalentAttributeGenerator"/>
/// (<c>[GWCAEquivalent("CppName")]</c>) are resolved automatically so
/// the generated signatures use the correct C# struct / enum types.
/// </summary>
[Generator(LanguageNames.CSharp)]
public sealed class GenerateGWCABindings : IIncrementalGenerator
{
    // ── Built-in type mappings (always available) ──────────────────

    private static readonly Dictionary<string, string> BuiltInTypeMappings =
        new()
        {
            ["Vec2f"] = "Vector2",
            ["Vec3f"] = "Vector3",
        };

    // ── Fallback signatures for C exports without a mangled counterpart ──

    private static readonly Dictionary<string, FallbackExport> FallbackSignatures =
        new()
        {
            ["GWCAVersion"] = new FallbackExport(true, "byte*", []),
            ["MoveItemToItem"] = new FallbackExport(false, "bool", [new FallbackParam("Item*", "from"), new FallbackParam("Item*", "to"), new FallbackParam("uint", "quantity")]),
            ["RemoveOfferedItem"] = new FallbackExport(false, "bool", [new FallbackParam("uint", "slot")]),
        };

    // ════════════════════════════════════════════════════════════════
    // IIncrementalGenerator
    // ════════════════════════════════════════════════════════════════

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        // Provider 1: DLL export names from AdditionalFiles
        var exportsProvider = context.AdditionalTextsProvider
            .Where(static f =>
                Path.GetFileName(f.Path)
                    .Equals("gwca.dll", StringComparison.OrdinalIgnoreCase))
            .Select(static (f, _) =>
            {
#pragma warning disable RS1035 // AdditionalText has no binary read API; file IO is required for PE parsing
                try { return PeExportReader.ReadExportNames(File.ReadAllBytes(f.Path)); }
                catch { return []; }
#pragma warning restore RS1035
            });

        // Provider 2: [GWCAEquivalent] type mappings
        var typeMappingsProvider = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                "Daybreak.Generators.GWCAEquivalentAttribute",
                predicate: static (_, _) => true,
                transform: static (ctx, _) =>
                {
                    if (ctx.Attributes.Length == 0)
                        return new TypeMapping(null!, null!);
                    var attr = ctx.Attributes[0];
                    if (attr.ConstructorArguments.Length == 0)
                        return new TypeMapping(null!, null!);
                    if (attr.ConstructorArguments[0].Value is not string gwcaName)
                        return new TypeMapping(null!, null!);
                    return new TypeMapping(gwcaName, ctx.TargetSymbol.Name);
                })
            .Where(static m => m.GwcaName is not null)
            .Collect();

        // Combine each matching DLL with all type mappings and generate
        var combined = exportsProvider.Combine(typeMappingsProvider);
        context.RegisterSourceOutput(combined, static (ctx, source) =>
            EmitSource(ctx, source.Left, source.Right));
    }

    // ════════════════════════════════════════════════════════════════
    // Code generation
    // ════════════════════════════════════════════════════════════════

    private static void EmitSource(
        SourceProductionContext context,
        ImmutableArray<string> exports,
        ImmutableArray<TypeMapping> mappings)
    {
        if (exports.IsDefaultOrEmpty)
            return;

        // Build type map
        var typeMap = new Dictionary<string, string>(BuiltInTypeMappings);
        foreach (var m in mappings)
            typeMap[m.GwcaName] = m.CsName;

        // Split exports
        var mangledExports = new List<string>();
        var cExports = new List<string>();
        foreach (var e in exports)
        {
            if (e.Length > 0 && e[0] == '?')
                mangledExports.Add(e);
            else
                cExports.Add(e);
        }

        cExports.Sort(StringComparer.Ordinal);

        // Demangle and build function-name → overload list lookup
        var mangledLookup = new Dictionary<string, List<(string MangledName, DemangledFunction Info)>>();
        foreach (var mangled in mangledExports)
        {
            var info = Demangle(mangled);
            if (info is null) continue;
            if (!mangledLookup.TryGetValue(info.FunctionName, out var list))
            {
                list = [];
                mangledLookup[info.FunctionName] = list;
            }

            list.Add((mangled, info));
        }

        // Match C exports to their mangled counterparts
        var entries = new List<CExportEntry>();
        foreach (var cName in cExports)
        {
            var info = MatchCExportToMangled(cName, mangledLookup);
            if (info is not null)
            {
                entries.Add(new CExportEntry(cName, info.QualifiedCppName, info, null));
            }
            else if (FallbackSignatures.TryGetValue(cName, out var fallback))
            {
                entries.Add(new CExportEntry(cName, "Fallback: " + cName, null, fallback));
            }
            else
            {
                entries.Add(new CExportEntry(cName, "Unmatched C export: " + cName, null, null));
            }
        }

        // Emit
        var sb = new StringBuilder(32768);
        EmitHeader(sb, cExports.Count);
        EmitBindings(sb, entries, typeMap);
        sb.AppendLine("}");

        context.AddSource("GWCA.g.cs", sb.ToString());
    }

    // ── File header ────────────────────────────────────────────────

    private static void EmitHeader(StringBuilder sb, int exportCount)
    {
        sb.AppendLine("// <auto-generated>");
        sb.AppendLine("// This file was generated by the GenerateGWCABindings source generator.");
        sb.AppendLine("// Do not edit manually — changes will be overwritten on rebuild.");
        sb.AppendLine("// </auto-generated>");
        sb.AppendLine();
        sb.AppendLine("#nullable enable");
        sb.AppendLine();
        sb.AppendLine("using System.Numerics;");
        sb.AppendLine("using System.Runtime.CompilerServices;");
        sb.AppendLine("using System.Runtime.InteropServices;");
        sb.AppendLine("using Daybreak.API.Interop.GuildWars;");
        sb.AppendLine("using Daybreak.API.Models;");
        sb.AppendLine();
        sb.AppendLine("namespace Daybreak.API.Interop;");
        sb.AppendLine();
        sb.AppendLine("/// <summary>");
        sb.AppendLine($"/// P/Invoke bindings for {exportCount} C exports from gwca.dll.");
        sb.AppendLine("/// Type signatures are resolved from the corresponding MSVC-decorated exports.");
        sb.AppendLine("/// Types annotated with [GWCAEquivalent] are used in signatures where available.");
        sb.AppendLine("/// Unmapped struct/enum types keep their C++ name to produce compile errors.");
        sb.AppendLine("/// </summary>");
        sb.AppendLine("internal static unsafe class GWCA");
        sb.AppendLine("{");
        sb.AppendLine("    private const string DllName = \"gwca.dll\";");
    }

    // ── Binding declarations ───────────────────────────────────────

    private static void EmitBindings(
        StringBuilder sb,
        List<CExportEntry> entries,
        Dictionary<string, string> typeMap)
    {
        foreach (var entry in entries)
        {
            sb.AppendLine();

            // No type info at all — emit a raw nint stub
            if (entry.Info is null && entry.Fallback is null)
            {
                sb.AppendLine($"    // {entry.Comment} — no type info available");
                sb.AppendLine($"    [DllImport(DllName, EntryPoint = \"{entry.CExportName}\")]");
                sb.AppendLine($"    internal static extern nint {entry.CExportName}();");
                continue;
            }

            // Fallback export (data or known header signature)
            if (entry.Fallback is { } fb)
            {
                if (fb.IsData)
                {
                    sb.AppendLine($"    // {entry.Comment} (data export)");
                    sb.AppendLine($"    internal static {fb.ReturnType} {entry.CExportName}");
                    sb.AppendLine("    {");
                    sb.AppendLine("        get");
                    sb.AppendLine("        {");
                    sb.AppendLine("            var lib = NativeLibrary.Load(DllName);");
                    sb.AppendLine($"            var addr = NativeLibrary.GetExport(lib, \"{entry.CExportName}\");");
                    sb.AppendLine($"            return ({fb.ReturnType})addr;");
                    sb.AppendLine("        }");
                    sb.AppendLine("    }");
                }
                else
                {
                    var fbParamStr = string.Join(", ", fb.Parameters.Select(p =>
                    {
                        if (p.Type == "bool")
                            return "[MarshalAs(UnmanagedType.U1)] bool " + p.Name;
                        return p.Type + " " + p.Name;
                    }));
                    var fbRetAttr = fb.ReturnType == "bool" ? "[return: MarshalAs(UnmanagedType.U1)] " : "";
                    sb.AppendLine($"    // {entry.Comment} (from GWCA C interop header)");
                    sb.AppendLine($"    [DllImport(DllName, EntryPoint = \"{entry.CExportName}\")]");
                    sb.AppendLine($"    {fbRetAttr}internal static extern {fb.ReturnType} {entry.CExportName}({fbParamStr});");
                }

                continue;
            }

            var info = entry.Info!;

            // VarArgs — cannot be expressed with DllImport
            if (info.IsVarArgs)
            {
                sb.AppendLine($"    // {entry.Comment} — varargs, requires manual interop");
                continue;
            }

            var csRet = ResolveCsType(info.ReturnType, typeMap);
            var retComment = TypeComment(info.ReturnType, typeMap);

            // Check for unmapped struct types
            bool hasUnmapped = HasUnmappedStruct(info.ReturnType, typeMap)
                || info.ParameterTypes.Any(pt => HasUnmappedStruct(pt, typeMap));

            // Build parameter list
            var parms = new List<string>();
            var paramComments = new List<string>();
            int pIdx = 0;

            if (info.IsMember)
            {
                parms.Add("nint self");
                pIdx++;
            }

            var usedNames = new HashSet<string>();
            foreach (var pt in info.ParameterTypes)
            {
                var csType = ResolveCsType(pt, typeMap);
                var name = MakeParamName(pt, typeMap, pIdx,
                    info.ParameterTypes.Length + (info.IsMember ? 1 : 0));
                while (!usedNames.Add(name))
                    name += "_" + pIdx;

                var comment = TypeComment(pt, typeMap);
                if (comment is not null)
                    paramComments.Add(name + ": " + comment);

                if (csType == "bool")
                    parms.Add("[MarshalAs(UnmanagedType.U1)] bool " + name);
                else
                    parms.Add(csType + " " + name);
                pIdx++;
            }

            var paramStr = string.Join(", ", parms);
            var retAttr = csRet == "bool" ? "[return: MarshalAs(UnmanagedType.U1)] " : "";

            // Comment
            var commentParts = new List<string> { entry.Comment };
            if (retComment is not null)
                commentParts.Add("returns " + retComment);
            commentParts.AddRange(paramComments);

            if (hasUnmapped)
            {
                // Comment out exports with unmapped struct types
                sb.AppendLine($"    // {string.Join(" | ", commentParts)}");
                sb.AppendLine($"    // [DllImport(DllName, EntryPoint = \"{entry.CExportName}\")]");
                sb.AppendLine($"    // {retAttr}internal static extern {csRet} {entry.CExportName}({paramStr});");
            }
            else
            {
                sb.AppendLine($"    // {string.Join(" | ", commentParts)}");
                sb.AppendLine($"    [DllImport(DllName, EntryPoint = \"{entry.CExportName}\")]");
                sb.AppendLine($"    {retAttr}internal static extern {csRet} {entry.CExportName}({paramStr});");
            }
        }
    }

    // ════════════════════════════════════════════════════════════════
    // Export matching
    // ════════════════════════════════════════════════════════════════

    /// <summary>
    /// Matches a C export name to its corresponding MSVC-decorated export to
    /// recover type information.  Handles exact matches and common disambiguation
    /// patterns (e.g. "ChangeTargetById" → "ChangeTarget" with uint param).
    /// </summary>
    private static DemangledFunction? MatchCExportToMangled(
        string cName,
        Dictionary<string, List<(string MangledName, DemangledFunction Info)>> lookup)
    {
        // 1. Exact match
        if (lookup.TryGetValue(cName, out var exact))
        {
            if (exact.Count == 1)
                return exact[0].Info;
            var (MangledName, Info) = exact.FirstOrDefault(e => !e.Info.IsMember && !e.Info.IsVarArgs);
            return Info ?? exact[0].Info;
        }

        // 2. Suffix-based disambiguation
        foreach (var (suffix, filter) in SuffixPatterns)
        {
            if (!cName.EndsWith(suffix, StringComparison.Ordinal))
                continue;
            var baseName = cName.Substring(0, cName.Length - suffix.Length);
            if (!lookup.TryGetValue(baseName, out var candidates))
                continue;
            var (MangledName, Info) = candidates.FirstOrDefault(e => filter(e.Info));
            if (Info is not null)
                return Info;
        }

        // 3. Multi-overload fallback
        if (lookup.TryGetValue(cName, out var multi) && multi.Count > 1)
            return multi[0].Info;

        return null;
    }

    private static readonly (string Suffix, Func<DemangledFunction, bool> Filter)[] SuffixPatterns =
        new (string, Func<DemangledFunction, bool>)[]
        {
            // ById / ByAgent patterns
            ("ById", f => !f.IsMember && f.ParameterTypes.Length > 0 &&
                f.ParameterTypes[0] is { Kind: TypeKind.Primitive, Name: "uint" or "int" }),
            ("ByAgent", f => !f.IsMember && f.ParameterTypes.Length > 0 &&
                f.ParameterTypes[0].Kind is TypeKind.Pointer or TypeKind.Struct),
            ("ByName", f => !f.IsMember && f.ParameterTypes.Length > 0 &&
                f.ParameterTypes[0].Kind is TypeKind.Pointer && f.ParameterTypes[0].Name is "ushort"),
            ("ByUuid", f => !f.IsMember && f.ParameterTypes.Length > 0 &&
                f.ParameterTypes[0].Kind is TypeKind.Pointer && f.ParameterTypes[0].Name is "byte"),
            ("ByIndex", f => !f.IsMember && f.ParameterTypes.Length > 0 &&
                f.ParameterTypes[0] is { Kind: TypeKind.Primitive, Name: "uint" or "int" }),

            // _Type suffix patterns
            ("_Enum", f => !f.IsMember && f.ParameterTypes.Length > 0 &&
                f.ParameterTypes[0].Name.Contains("Enum")),
            ("_Flag", f => !f.IsMember && f.ParameterTypes.Length > 0 &&
                f.ParameterTypes[0].Name.Contains("Flag")),
            ("_Number", f => !f.IsMember && f.ParameterTypes.Length > 0 &&
                f.ParameterTypes[0].Name.Contains("Number")),
            ("_String", f => !f.IsMember && f.ParameterTypes.Length > 0 &&
                HasStringParam(f)),
            ("_UInt", f => !f.IsMember && f.ParameterTypes.Length > 0 &&
                HasUIntParam(f)),

            // ToItem / ToBag patterns
            ("ToItem", f => !f.IsMember && f.ParameterTypes.Length >= 2 &&
                f.ParameterTypes[1].Kind is TypeKind.Pointer or TypeKind.Struct &&
                f.ParameterTypes[1].Name is "Item"),
            ("ToBag", f => !f.IsMember && f.ParameterTypes.Length >= 2 &&
                (f.ParameterTypes[1].Name is "Bag" ||
                 (f.ParameterTypes.Length >= 3 && f.ParameterTypes[1].Kind is TypeKind.Enum))),

            // ForAgent
            ("ForAgent", f => !f.IsMember && f.ParameterTypes.Length >= 1 &&
                f.ParameterTypes[0] is { Kind: TypeKind.Primitive, Name: "uint" }),

            // String suffix
            ("String", f => !f.IsMember && f.ParameterTypes.Length > 0 &&
                f.ParameterTypes[0].Kind is TypeKind.Pointer && f.ParameterTypes[0].Name is "byte"),

            // FromChar / FromWChar
            ("FromChar", f => !f.IsMember && f.ParameterTypes.Length > 0 &&
                f.ParameterTypes[0] is { Kind: TypeKind.Primitive, Name: "byte" }),
            ("FromWChar", f => !f.IsMember && f.ParameterTypes.Length > 0 &&
                f.ParameterTypes[0] is { Kind: TypeKind.Primitive, Name: "ushort" }),

            // ByDistrict / ByRegion
            ("ByDistrict", f => !f.IsMember && f.ParameterTypes.Length >= 2 &&
                f.ParameterTypes[1].Name.Contains("District")),
            ("ByRegion", f => !f.IsMember && f.ParameterTypes.Length >= 2 &&
                f.ParameterTypes[1].Name.Contains("Region")),
        };

    // ════════════════════════════════════════════════════════════════
    // Type helpers
    // ════════════════════════════════════════════════════════════════

    private static bool HasStringParam(DemangledFunction f)
    {
        return f.ParameterTypes.Any(p =>
                   p.Kind is TypeKind.Pointer && p.Name is "ushort")
            || f.ParameterTypes.Any(p =>
                   p.Name.IndexOf("String", StringComparison.OrdinalIgnoreCase) >= 0);
    }

    private static bool HasUIntParam(DemangledFunction f)
    {
        return f.ParameterTypes.Any(p =>
                   p.Kind is TypeKind.Pointer && p.Name is "uint")
            || f.ParameterTypes.Any(p =>
                   p is { Kind: TypeKind.Primitive, Name: "uint" });
    }

    /// <summary>
    /// Returns true if the type references a struct that is not in the typeMap.
    /// Enums are excluded — they fall back to their underlying integer type.
    /// </summary>
    private static bool HasUnmappedStruct(DemangledType dt, Dictionary<string, string> typeMap)
    {
        if (dt.TemplateArg is not null && HasUnmappedStruct(dt.TemplateArg, typeMap))
            return true;

        if (dt.Kind is TypeKind.Struct)
        {
            if (dt.Name is "Array") return false;
            return !typeMap.ContainsKey(dt.Name);
        }

        if (dt.Kind is TypeKind.Pointer)
        {
            if (dt.Name is "void" or "byte" or "ushort" or "short" or "int" or "uint"
                or "long" or "ulong" or "float" or "double" or "bool" or "funcptr")
                return false;
            if (dt.Name is "Array") return false;
            return !typeMap.ContainsKey(dt.Name);
        }

        return false;
    }

    /// <summary>
    /// Resolves a <see cref="DemangledType"/> to a C# type string, using the
    /// GWCAEquivalent type map for struct/enum types.
    /// </summary>
    private static string ResolveCsType(DemangledType dt, Dictionary<string, string> typeMap)
    {
        switch (dt.Kind)
        {
            case TypeKind.Primitive:
                return dt.Name;

            case TypeKind.FuncPtr:
                return "nint";

            case TypeKind.Pointer:
                {
                    if (dt.Name == "Array" && dt.TemplateArg is not null)
                    {
                        var innerType = ResolveTemplateArgType(dt.TemplateArg, typeMap);
                        return "GuildWarsArray<" + innerType + ">*";
                    }

                    var inner = dt.Name switch
                    {
                        "void" => "void",
                        "byte" => "byte",
                        "ushort" => "ushort",
                        "short" => "short",
                        "int" => "int",
                        "uint" => "uint",
                        "long" => "long",
                        "ulong" => "ulong",
                        "float" => "float",
                        "double" => "double",
                        "bool" => "byte", // bool* not blittable
                        _ => typeMap.TryGetValue(dt.Name, out var mapped) ? mapped : dt.Name,
                    };
                    return inner + "*";
                }

            case TypeKind.Enum:
                if (typeMap.TryGetValue(dt.Name, out var enumCs))
                    return enumCs;
                return dt.UnderlyingType ?? "int";

            case TypeKind.Struct:
                if (dt.Name == "Array" && dt.TemplateArg is not null)
                {
                    var innerType = ResolveTemplateArgType(dt.TemplateArg, typeMap);
                    return "GuildWarsArray<" + innerType + ">";
                }

                if (typeMap.TryGetValue(dt.Name, out var structCs))
                    return structCs;
                return dt.Name;

            default:
                return "nint";
        }
    }

    private static string ResolveTemplateArgType(DemangledType arg, Dictionary<string, string> typeMap)
    {
        return arg.Kind switch
        {
            TypeKind.Primitive => arg.Name,
            TypeKind.Enum => typeMap.TryGetValue(arg.Name, out var e) ? e : arg.Name,
            TypeKind.Struct => typeMap.TryGetValue(arg.Name, out var s) ? s : arg.Name,
            TypeKind.Pointer => ResolveCsType(arg, typeMap),
            _ => "nint",
        };
    }

    /// <summary>
    /// Returns a comment annotation for unmapped struct/enum types, or null.
    /// </summary>
    private static string? TypeComment(DemangledType dt, Dictionary<string, string> typeMap)
    {
        switch (dt.Kind)
        {
            case TypeKind.Pointer:
                if (dt.Name is "void" or "byte" or "ushort" or "short" or "int" or "uint"
                    or "long" or "ulong" or "float" or "double" or "bool")
                    return null;
                if (typeMap.ContainsKey(dt.Name))
                    return null;
                return "TODO: map struct " + (dt.QualifiedName ?? dt.Name);

            case TypeKind.Enum:
                if (typeMap.ContainsKey(dt.Name))
                    return null;
                return "enum " + (dt.QualifiedName ?? dt.Name) + " (as " + (dt.UnderlyingType ?? "int") + ")";

            case TypeKind.Struct:
                if (typeMap.ContainsKey(dt.Name))
                    return null;
                return "TODO: map struct " + (dt.QualifiedName ?? dt.Name);

            default:
                return null;
        }
    }

    // ════════════════════════════════════════════════════════════════
    // Parameter naming
    // ════════════════════════════════════════════════════════════════

    private static string MakeParamName(
        DemangledType dt, Dictionary<string, string> typeMap, int idx, int total)
    {
        string baseName;
        if (dt.Kind is TypeKind.Pointer)
        {
            baseName = dt.Name is "void" or "byte" or "ushort" or "short" or "int" or "uint"
                or "long" or "ulong" or "float" or "double" or "bool"
                ? "ptr"
                : CamelCase(typeMap.TryGetValue(dt.Name, out var mapped) ? mapped : dt.Name);
        }
        else if (dt.Kind is TypeKind.FuncPtr)
        {
            baseName = "callback";
        }
        else
        {
            var csType = ResolveCsType(dt, typeMap);
            baseName = csType switch
            {
                "uint" => "value",
                "int" => "value",
                "nint" => "ptr",
                "bool" => "flag",
                "float" => "value",
                "double" => "value",
                "short" => "value",
                "ushort" => "value",
                "byte" => "value",
                "long" => "value",
                "ulong" => "value",
                _ => CamelCase(csType),
            };
        }

        return total > 1 ? baseName + (idx + 1) : baseName;
    }

    private static string CamelCase(string name)
    {
        if (name.Length == 0) return "arg";
        return char.ToLowerInvariant(name[0]) + name.Substring(1);
    }

    // ════════════════════════════════════════════════════════════════
    // Internal types
    // ════════════════════════════════════════════════════════════════

    /// <summary>Type mapping for the incremental pipeline.  Implements
    /// <see cref="IEquatable{T}"/> for proper caching.</summary>
    internal readonly struct TypeMapping(string gwcaName, string csName) : IEquatable<TypeMapping>
    {
        public readonly string GwcaName = gwcaName;
        public readonly string CsName = csName;

        public bool Equals(TypeMapping other)
            => this.GwcaName == other.GwcaName && this.CsName == other.CsName;

        public override bool Equals(object? obj)
            => obj is TypeMapping other && this.Equals(other);

        public override int GetHashCode()
            => (this.GwcaName?.GetHashCode() ?? 0) * (397 ^ (this.CsName?.GetHashCode() ?? 0));
    }

    private sealed class CExportEntry(string cExportName, string comment,
DemangledFunction? info, FallbackExport? fallback)
    {
        public string CExportName { get; } = cExportName;
        public string Comment { get; } = comment;
        public DemangledFunction? Info { get; } = info;
        public FallbackExport? Fallback { get; } = fallback;
    }

    internal sealed class FallbackExport(bool isData, string returnType, FallbackParam[] parameters)
    {
        public bool IsData { get; } = isData;
        public string ReturnType { get; } = returnType;
        public FallbackParam[] Parameters { get; } = parameters;
    }

    internal readonly struct FallbackParam(string type, string name)
    {
        public readonly string Type = type;
        public readonly string Name = name;
    }
}

