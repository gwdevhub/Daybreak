using System.Text;
using System.Text.RegularExpressions;
using GenerateGWCABindings;
using PeNet;
using static GenerateGWCABindings.MsvcDemangler;

// ── Resolve paths ──────────────────────────────────────────────────

var repoRoot = FindRepoRoot(AppContext.BaseDirectory)
    ?? FindRepoRoot(Directory.GetCurrentDirectory())
    ?? throw new InvalidOperationException(
        "Could not find Daybreak repository root. " +
        "Run this tool from within the repository or pass the repo root as an argument.");

if (args.Length > 0 && Directory.Exists(args[0]))
    repoRoot = Path.GetFullPath(args[0]);

var dllPath = Path.Combine(repoRoot, "Daybreak.API", "Dependencies", "gwca.dll");
var outputPath = Path.Combine(repoRoot, "Daybreak.API", "Interop", "GWCA.cs");
var apiRoot = Path.Combine(repoRoot, "Daybreak.API");

if (!File.Exists(dllPath))
    throw new FileNotFoundException($"gwca.dll not found at {dllPath}");

Console.WriteLine($"Reading exports from: {dllPath}");

// ── Scan for [GWCAEquivalent] types ────────────────────────────────

// Maps GWCA C++ name → C# type name (unqualified, since we add a using directive)
var typeMap = new Dictionary<string, string>
{
    // Built-in mappings (System.Numerics)
    ["Vec2f"] = "Vector2",
    ["Vec3f"] = "Vector3",
};
ScanGWCAEquivalents(apiRoot, typeMap);
Console.WriteLine($"Found {typeMap.Count} type mappings ({typeMap.Count - 2} from [GWCAEquivalent])");

// ── Read PE exports ────────────────────────────────────────────────

var pe = new PeFile(dllPath);
var allExports = pe.ExportedFunctions?
    .Where(e => e.Name is not null)
    .Select(e => e.Name!)
    .ToList()
    ?? throw new InvalidOperationException("No exported functions found in gwca.dll");

// Split into mangled (MSVC decorated) and C exports
var mangledExports = allExports.Where(e => e.StartsWith('?')).ToList();
var cExports = allExports.Where(e => !e.StartsWith('?')).OrderBy(e => e).ToList();

Console.WriteLine($"Found {allExports.Count} total exports ({mangledExports.Count} mangled, {cExports.Count} C exports)");

// ── Demangle MSVC exports to build type-info lookup ────────────────

// Build a map: C++ function name → list of DemangledFunction (for overloads)
var mangledLookup = new Dictionary<string, List<(string MangledName, DemangledFunction Info)>>();

foreach (var mangled in mangledExports)
{
    var info = Demangle(mangled);
    if (info is null) continue;

    var key = info.FunctionName;
    if (!mangledLookup.TryGetValue(key, out var list))
    {
        list = [];
        mangledLookup[key] = list;
    }
    list.Add((mangled, info));
}

Console.WriteLine($"Demangled {mangledLookup.Values.Sum(l => l.Count)} MSVC exports into {mangledLookup.Count} unique function names");

// ── Fallback signatures for C exports that have no MSVC mangled counterpart ──
// These are sourced from the GWCA C interop headers.
// "data" entries are exported variables (not functions) and use NativeLibrary.

var fallbackSignatures = new Dictionary<string, FallbackExport>
{
    ["GWCAVersion"] = new(IsData: true, ReturnType: "byte*", Parameters: []),
    ["MoveItemToItem"] = new(IsData: false, ReturnType: "bool", Parameters: [("Item*", "from"), ("Item*", "to"), ("uint", "quantity")]),
    ["RemoveOfferedItem"] = new(IsData: false, ReturnType: "bool", Parameters: [("uint", "slot")]),
};

// ── Match C exports to mangled exports for type info ───────────────

var entries = new List<CExportEntry>();
int matched = 0;
int unmatched = 0;

foreach (var cName in cExports)
{
    var info = MatchCExportToMangled(cName, mangledLookup);
    if (info is not null)
    {
        matched++;
        entries.Add(new CExportEntry(cName, info.QualifiedCppName, info));
    }
    else if (fallbackSignatures.TryGetValue(cName, out var fallback))
    {
        matched++;
        entries.Add(new CExportEntry(cName, $"Fallback: {cName}", null, fallback));
        Console.WriteLine($"  Using fallback signature for C export: {cName}");
    }
    else
    {
        unmatched++;
        entries.Add(new CExportEntry(cName, $"Unmatched C export: {cName}", null));
        Console.WriteLine($"  WARNING: No mangled match for C export: {cName}");
    }
}

Console.WriteLine($"Matched {matched}/{cExports.Count} C exports to mangled signatures ({unmatched} unmatched)");

// ── Collect all referenced GWCA type names ─────────────────────────

var allReferencedTypes = new HashSet<string>();
foreach (var entry in entries)
{
    if (entry.Info is null) continue;
    CollectType(entry.Info.ReturnType, allReferencedTypes);
    foreach (var pt in entry.Info.ParameterTypes)
        CollectType(pt, allReferencedTypes);
}

var unmappedTypes = allReferencedTypes
    .Where(t => !typeMap.ContainsKey(t))
    .OrderBy(t => t)
    .ToList();

if (unmappedTypes.Count > 0)
{
    Console.WriteLine($"  {unmappedTypes.Count} unmapped GWCA types (structs → compile errors, enums → underlying int type):");
    foreach (var t in unmappedTypes)
        Console.WriteLine($"    - {t}");
}

// ── Emit C# ────────────────────────────────────────────────────────

var sb = new StringBuilder();
sb.AppendLine("// <auto-generated>");
sb.AppendLine("// This file was generated by the GenerateGWCABindings tool.");
sb.AppendLine("// Do not edit manually — re-run the generator instead.");
sb.AppendLine("// </auto-generated>");
sb.AppendLine();
sb.AppendLine("using System.Numerics;");
sb.AppendLine("using System.Runtime.CompilerServices;");
sb.AppendLine("using System.Runtime.InteropServices;");
sb.AppendLine("using Daybreak.API.Interop.GuildWars;");
sb.AppendLine("using Daybreak.API.Models;");
sb.AppendLine();
sb.AppendLine("[assembly: DisableRuntimeMarshalling]");
sb.AppendLine();
sb.AppendLine("namespace Daybreak.API.Interop;");
sb.AppendLine();
sb.AppendLine("/// <summary>");
sb.AppendLine($"/// P/Invoke bindings for {cExports.Count} C exports from gwca.dll.");
sb.AppendLine("/// Type signatures are resolved from the corresponding MSVC-decorated exports.");
sb.AppendLine("/// Types annotated with [GWCAEquivalent] are used in signatures where available.");
sb.AppendLine("/// Unmapped struct/enum types keep their C++ name to produce compile errors.");
sb.AppendLine("/// </summary>");
sb.AppendLine("internal static unsafe partial class GWCA");
sb.AppendLine("{");
sb.AppendLine("    private const string DllName = \"gwca.dll\";");

int funcCount = 0;
int skipCount = 0;

foreach (var entry in entries)
{
    sb.AppendLine();

    if (entry.Info is null && entry.Fallback is null)
    {
        sb.AppendLine($"    // {entry.Comment} — no type info available");
        sb.AppendLine($"    [LibraryImport(DllName, EntryPoint = \"{entry.CExportName}\")]");
        sb.AppendLine($"    internal static partial nint {entry.CExportName}();");
        funcCount++;
        continue;
    }

    if (entry.Fallback is { } fb)
    {
        if (fb.IsData)
        {
            // Data export — use NativeLibrary.GetExport
            sb.AppendLine($"    // {entry.Comment} (data export)");
            sb.AppendLine($"    internal static {fb.ReturnType} {entry.CExportName}");
            sb.AppendLine( "    {");
            sb.AppendLine( "        get");
            sb.AppendLine( "        {");
            sb.AppendLine( "            var lib = NativeLibrary.Load(DllName);");
            sb.AppendLine($"            var addr = NativeLibrary.GetExport(lib, \"{entry.CExportName}\");");
            sb.AppendLine($"            return ({fb.ReturnType})addr;");
            sb.AppendLine( "        }");
            sb.AppendLine( "    }");
        }
        else
        {
            // Function export with known signature from GWCA headers
            var fbParamStr = string.Join(", ", fb.Parameters.Select(p =>
            {
                if (p.Type == "bool")
                    return $"[MarshalAs(UnmanagedType.U1)] bool {p.Name}";
                return $"{p.Type} {p.Name}";
            }));
            var fbRetAttr = fb.ReturnType == "bool" ? "[return: MarshalAs(UnmanagedType.U1)] " : "";
            sb.AppendLine($"    // {entry.Comment} (from GWCA C interop header)");
            sb.AppendLine($"    [LibraryImport(DllName, EntryPoint = \"{entry.CExportName}\")]");
            sb.AppendLine($"    {fbRetAttr}internal static partial {fb.ReturnType} {entry.CExportName}({fbParamStr});");
        }
        funcCount++;
        continue;
    }

    if (entry.Info.IsVarArgs)
    {
        sb.AppendLine($"    // {entry.Comment} — varargs, requires manual interop");
        skipCount++;
        continue;
    }

    var info = entry.Info;
    var csRet = ResolveCsType(info.ReturnType, typeMap);
    var retComment = TypeComment(info.ReturnType, typeMap);

    // Check if this export references any unmapped struct types
    var hasUnmappedStruct = HasUnmappedStruct(info.ReturnType, typeMap);
    if (!hasUnmappedStruct)
        hasUnmappedStruct = info.ParameterTypes.Any(pt => HasUnmappedStruct(pt, typeMap));

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
        var name = MakeParamName(pt, typeMap, pIdx, info.ParameterTypes.Length + (info.IsMember ? 1 : 0));
        while (!usedNames.Add(name))
            name += $"_{pIdx}";

        var comment = TypeComment(pt, typeMap);
        if (comment is not null)
            paramComments.Add($"{name}: {comment}");

        if (csType == "bool")
            parms.Add($"[MarshalAs(UnmanagedType.U1)] bool {name}");
        else
            parms.Add($"{csType} {name}");
        pIdx++;
    }

    var paramStr = string.Join(", ", parms);
    var retAttr = csRet == "bool" ? "[return: MarshalAs(UnmanagedType.U1)] " : "";

    // Comment line: C++ qualified name + any unmapped type annotations
    var commentParts = new List<string> { entry.Comment };
    if (retComment is not null)
        commentParts.Add($"returns {retComment}");
    commentParts.AddRange(paramComments);

    if (hasUnmappedStruct)
    {
        // Comment out exports with unmapped struct types
        sb.AppendLine($"    // {string.Join(" | ", commentParts)}");
        sb.AppendLine($"    // [LibraryImport(DllName, EntryPoint = \"{entry.CExportName}\")]");
        sb.AppendLine($"    // {retAttr}internal static partial {csRet} {entry.CExportName}({paramStr});");
        skipCount++;
    }
    else
    {
        sb.AppendLine($"    // {string.Join(" | ", commentParts)}");
        sb.AppendLine($"    [LibraryImport(DllName, EntryPoint = \"{entry.CExportName}\")]");
        sb.AppendLine($"    {retAttr}internal static partial {csRet} {entry.CExportName}({paramStr});");
        funcCount++;
    }
}

sb.AppendLine("}");

// ── Write output ───────────────────────────────────────────────────

Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);
File.WriteAllText(outputPath, sb.ToString(), new UTF8Encoding(false));
Console.WriteLine($"Generated {outputPath}");
Console.WriteLine($"  {funcCount} functions");
Console.WriteLine($"  {skipCount} skipped (unmatched / varargs)");

// ════════════════════════════════════════════════════════════════════
// Helper functions
// ════════════════════════════════════════════════════════════════════

/// <summary>
/// Matches a C export name to its corresponding MSVC-decorated export to
/// recover type information. Handles exact matches and common disambiguation
/// patterns (e.g. "ChangeTargetById" → "ChangeTarget" with uint param).
/// </summary>
static DemangledFunction? MatchCExportToMangled(
    string cName,
    Dictionary<string, List<(string MangledName, DemangledFunction Info)>> lookup)
{
    // 1. Exact match — C export name == C++ function name
    if (lookup.TryGetValue(cName, out var exact))
    {
        if (exact.Count == 1)
            return exact[0].Info;
        // Multiple overloads with the same name — pick the non-member non-varargs one
        var best = exact.FirstOrDefault(e => !e.Info.IsMember && !e.Info.IsVarArgs);
        return best.Info ?? exact[0].Info;
    }

    // 2. Suffix-based disambiguation — try stripping known suffixes
    //    e.g. "ChangeTargetByAgent" → base "ChangeTarget", suffix "ByAgent"
    //    e.g. "GetPreference_Enum" → base "GetPreference", suffix "_Enum"
    var suffixPatterns = new (string Suffix, Func<DemangledFunction, bool> Filter)[]
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

        // _Type suffix patterns (GetPreference_Enum, etc.)
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

        // ForAgent — typically takes an agent ID (uint) as first param
        ("ForAgent", f => !f.IsMember && f.ParameterTypes.Length >= 1 &&
            f.ParameterTypes[0] is { Kind: TypeKind.Primitive, Name: "uint" }),

        // String suffix for LoadSkillTemplate etc.
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

    foreach (var (suffix, filter) in suffixPatterns)
    {
        if (!cName.EndsWith(suffix, StringComparison.Ordinal))
            continue;

        var baseName = cName[..^suffix.Length];
        if (!lookup.TryGetValue(baseName, out var candidates))
            continue;

        var match = candidates.FirstOrDefault(e => filter(e.Info));
        if (match.Info is not null)
            return match.Info;
    }

    // 3. Check for "ChangeSecondProfession" which matches two different namespaces
    //    — prefer the one without an agent ID parameter (PlayerMgr version)
    if (lookup.TryGetValue(cName, out var multi) && multi.Count > 1)
    {
        // Already handled above; fallback to first
        return multi[0].Info;
    }

    return null;
}

static bool HasStringParam(DemangledFunction f)
{
    return f.ParameterTypes.Any(p =>
        p.Kind is TypeKind.Pointer && p.Name is "ushort") ||
        f.ParameterTypes.Any(p =>
        p.Name.Contains("String", StringComparison.OrdinalIgnoreCase));
}

static bool HasUIntParam(DemangledFunction f)
{
    // Has a uint* or uint param that isn't a string pointer
    return f.ParameterTypes.Any(p =>
        p.Kind is TypeKind.Pointer && p.Name is "uint") ||
        f.ParameterTypes.Any(p =>
        p is { Kind: TypeKind.Primitive, Name: "uint" });
}

static void ScanGWCAEquivalents(string apiRoot, Dictionary<string, string> map)
{
    // Regex matches: [GWCAEquivalent("SomeName")]  followed by struct/enum declaration
    var attrRegex = new Regex(
        @"\[GWCAEquivalent\(""([^""]+)""\)\]\s*(?:\[.*?\]\s*)*(public|internal)\s+(?:(?:readonly|unsafe|ref)\s+)*(?:struct|enum)\s+(\w+)",
        RegexOptions.Singleline);

    foreach (var csFile in Directory.EnumerateFiles(apiRoot, "*.cs", SearchOption.AllDirectories))
    {
        // Don't scan the generated file itself
        if (csFile.EndsWith("GWCA.cs", StringComparison.OrdinalIgnoreCase))
            continue;

        var content = File.ReadAllText(csFile);
        var attrMatches = attrRegex.Matches(content);
        if (attrMatches.Count == 0) continue;

        foreach (Match m in attrMatches)
        {
            var gwcaName = m.Groups[1].Value;
            var csTypeName = m.Groups[3].Value;
            map[gwcaName] = csTypeName;
            Console.WriteLine($"  Mapped: {gwcaName} → {csTypeName}");
        }
    }
}

static void CollectType(DemangledType dt, HashSet<string> types)
{
    if (dt.TemplateArg is not null)
        CollectType(dt.TemplateArg, types);

    if (dt.Name is "Array") return;

    if (dt.Kind is TypeKind.Struct)
    {
        types.Add(dt.Name);
    }
    else if (dt.Kind is TypeKind.Enum)
    {
        // Only collect mapped enums; unmapped ones resolve to their underlying int type
        types.Add(dt.Name);
    }
    else if (dt.Kind is TypeKind.Pointer &&
             dt.Name is not ("void" or "byte" or "ushort" or "short" or "int" or "uint"
                 or "long" or "ulong" or "float" or "double" or "bool" or "nint" or "funcptr"))
    {
        types.Add(dt.Name);
    }
}

/// <summary>
/// Returns true if the type references a struct that is not in the typeMap.
/// Enums are excluded — they fall back to their underlying integer type.
/// </summary>
static bool HasUnmappedStruct(DemangledType dt, Dictionary<string, string> typeMap)
{
    // Template arg check
    if (dt.TemplateArg is not null && HasUnmappedStruct(dt.TemplateArg, typeMap))
        return true;

    if (dt.Kind is TypeKind.Struct)
    {
        if (dt.Name is "Array") return false; // Array → GuildWarsArray, handled
        return !typeMap.ContainsKey(dt.Name);
    }

    if (dt.Kind is TypeKind.Pointer)
    {
        // Primitive pointers are fine
        if (dt.Name is "void" or "byte" or "ushort" or "short" or "int" or "uint"
            or "long" or "ulong" or "float" or "double" or "bool" or "funcptr")
            return false;
        if (dt.Name is "Array") return false;
        return !typeMap.ContainsKey(dt.Name);
    }

    return false;
}

/// <summary>
/// Resolves a DemangledType to a C# type string, using the GWCAEquivalent
/// type map for struct/enum types. Unmapped enums/structs keep their name
/// so the compiler shows errors for types that still need implementing.
/// </summary>
static string ResolveCsType(DemangledType dt, Dictionary<string, string> typeMap)
{
    switch (dt.Kind)
    {
        case TypeKind.Primitive:
            return dt.Name;

        case TypeKind.FuncPtr:
            return "nint";

        case TypeKind.Pointer:
        {
            // Template pointer: Array<T>* → GuildWarsArray<ResolvedT>*
            if (dt.Name == "Array" && dt.TemplateArg is not null)
            {
                var innerType = ResolveTemplateArgType(dt.TemplateArg, typeMap);
                return $"GuildWarsArray<{innerType}>*";
            }

            // Resolve the pointee type name
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
                "bool" => "byte", // bool* not blittable, use byte*
                _ => typeMap.TryGetValue(dt.Name, out var mapped) ? mapped : dt.Name,
            };
            return $"{inner}*";
        }

        case TypeKind.Enum:
            if (typeMap.TryGetValue(dt.Name, out var enumCs))
                return enumCs;
            return dt.UnderlyingType ?? "int"; // unmapped enum → underlying integer type

        case TypeKind.Struct:
            // Template struct by value: Array<T> → GuildWarsArray<ResolvedT>
            if (dt.Name == "Array" && dt.TemplateArg is not null)
            {
                var innerType = ResolveTemplateArgType(dt.TemplateArg, typeMap);
                return $"GuildWarsArray<{innerType}>";
            }
            if (typeMap.TryGetValue(dt.Name, out var structCs))
                return structCs;
            return dt.Name; // unmapped — will produce a compile error

        default:
            return "nint";
    }
}

/// <summary>
/// Resolves a template argument type to a C# type name.
/// </summary>
static string ResolveTemplateArgType(DemangledType arg, Dictionary<string, string> typeMap)
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
/// Returns a comment annotation for unmapped struct/enum types.
/// </summary>
static string? TypeComment(DemangledType dt, Dictionary<string, string> typeMap)
{
    switch (dt.Kind)
    {
        case TypeKind.Pointer:
            if (dt.Name is "void" or "byte" or "ushort" or "short" or "int" or "uint"
                or "long" or "ulong" or "float" or "double" or "bool")
                return null;
            if (typeMap.ContainsKey(dt.Name))
                return null;
            return $"TODO: map struct {dt.QualifiedName ?? dt.Name}";

        case TypeKind.Enum:
            if (typeMap.ContainsKey(dt.Name))
                return null;
            return $"enum {dt.QualifiedName ?? dt.Name} (as {dt.UnderlyingType ?? "int"})";

        case TypeKind.Struct:
            if (typeMap.ContainsKey(dt.Name))
                return null;
            return $"TODO: map struct {dt.QualifiedName ?? dt.Name}";

        default:
            return null;
    }
}

static string? FindRepoRoot(string startDir)
{
    var dir = startDir;
    while (dir is not null)
    {
        if (File.Exists(Path.Combine(dir, "Daybreak.slnx")))
            return dir;
        dir = Path.GetDirectoryName(dir);
    }
    return null;
}

static string MakeParamName(DemangledType dt, Dictionary<string, string> typeMap, int idx, int total)
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
    return total > 1 ? $"{baseName}{idx + 1}" : baseName;
}

static string CamelCase(string name)
{
    if (name.Length == 0) return "arg";
    return char.ToLowerInvariant(name[0]) + name[1..];
}

// ── Types ──────────────────────────────────────────────────────────

record CExportEntry(string CExportName, string Comment, DemangledFunction? Info, FallbackExport? Fallback = null);
record FallbackExport(bool IsData, string ReturnType, (string Type, string Name)[] Parameters);
