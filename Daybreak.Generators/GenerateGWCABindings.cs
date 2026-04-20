using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CodeAnalysis;
using Sybil;
using static Daybreak.Generators.MsvcDemangler;
using TypeKind = Daybreak.Generators.MsvcDemangler.TypeKind;

namespace Daybreak.Generators;

/// <summary>
/// Incremental source generator that reads PE exports from gwca.dll,
/// demangles MSVC-decorated C++ names to recover type and namespace
/// information, and emits a <c>GWCA</c> class with nested static classes
/// mirroring the C++ namespace hierarchy (e.g. GW::Agents → GWCA.GW.Agents).
///
/// Additionally scans C++ header files under Constants/ for enum and constexpr
/// declarations, emitting matching C# enums and const fields.
///
/// Types annotated with <c>[GWCAEquivalent("CppName")]</c> are resolved
/// automatically so the generated signatures use the correct C# types.
/// </summary>
[Generator(LanguageNames.CSharp)]
public sealed class GenerateGWCABindings : IIncrementalGenerator
{
    private const string AttributeName = "GWCAEquivalentAttribute";
    private const string AttributeShortName = "GWCAEquivalent";
    private const string AttributeNamespace = "Daybreak.Generators";
    private const string PropertyName = "GWCAName";
    private const string Public = "public";
    private const string StringType = "string";
    private const string ParameterName = "gwcaName";

    // ── Built-in type mappings (always available) ──────────────────

    private static readonly Dictionary<string, string> BuiltInTypeMappings =
        new()
        {
            ["Vec2f"] = "global::System.Numerics.Vector2",
            ["Vec3f"] = "global::System.Numerics.Vector3",
            // UIInteractionCallback is a typedef for a function pointer - map to nint
            ["UIInteractionCallback"] = "nint",
            ["UI::UIInteractionCallback"] = "nint",
            ["GW::UI::UIInteractionCallback"] = "nint",
        };

    // ════════════════════════════════════════════════════════════════
    // IIncrementalGenerator
    // ════════════════════════════════════════════════════════════════

    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(context =>
        {
            var compilationUnitBuilder = SyntaxBuilder.CreateCompilationUnit()
                .WithNamespace(
                SyntaxBuilder.CreateFileScopedNamespace(AttributeNamespace)
                    .WithClass(SyntaxBuilder.CreateClass(AttributeName)
                        .WithModifier(Public)
                    .WithConstructor(SyntaxBuilder.CreateConstructor(AttributeName)
                        .WithModifier(Public)
                        .WithParameter(StringType, ParameterName)
                        .WithBody($"this.{PropertyName} = {ParameterName};"))
                    .WithProperty(SyntaxBuilder.CreateProperty(StringType, PropertyName)
                        .WithModifier(Public)
                        .WithAccessor(SyntaxBuilder.CreateGetter()))
                    .WithAttribute(SyntaxBuilder.CreateAttribute("AttributeUsage")
                        .WithRawArgument("AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Enum, AllowMultiple = false, Inherited = false"))
                    .WithBaseClass(nameof(Attribute))));
            var compilationUnitSyntax = compilationUnitBuilder.Build();
            var source = compilationUnitSyntax.ToFullString();
            context.AddSource($"{AttributeName}.g", source);
        });

        // Provider 1: DLL export names from AdditionalFiles
        var exportsProvider = context.AdditionalTextsProvider
            .Where(static f =>
                Path.GetFileName(f.Path)
                    .Equals("gwca.dll", StringComparison.OrdinalIgnoreCase))
            .Select(static (f, _) =>
            {
#pragma warning disable RS1035 // AdditionalText has no binary read API; file IO is required for PE parsing
                try { return (f.Path, Exports: PeExportReader.ReadExportNames(File.ReadAllBytes(f.Path))); }
                catch { return (f.Path, Exports: ImmutableArray<string>.Empty); }
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
                        return new TypeMapping(null!, null!, null!);
                    var attr = ctx.Attributes[0];
                    if (attr.ConstructorArguments.Length == 0)
                        return new TypeMapping(null!, null!, null!);
                    if (attr.ConstructorArguments[0].Value is not string gwcaName)
                        return new TypeMapping(null!, null!, null!);
                    var ns = ctx.TargetSymbol.ContainingNamespace?.ToDisplayString() ?? "";
                    return new TypeMapping(gwcaName, ctx.TargetSymbol.Name, ns);
                })
            .Where(static m => m.GwcaName is not null)
            .Collect();

        // Combine each matching DLL with all type mappings and generate
        var combined = exportsProvider.Combine(typeMappingsProvider);
        context.RegisterSourceOutput(combined, static (ctx, source) =>
            EmitSource(source.Left.Path, source.Left.Exports, source.Right));
    }

    // ════════════════════════════════════════════════════════════════
    // Code generation
    // ════════════════════════════════════════════════════════════════

    private static void EmitSource(
        string dllPath,
        ImmutableArray<string> exports,
        ImmutableArray<TypeMapping> mappings)
    {
        if (exports.IsDefaultOrEmpty)
            return;

        // Build type map (gwcaName → fully qualified C# name)
        var typeMap = new Dictionary<string, string>(BuiltInTypeMappings);
        foreach (var m in mappings)
        {
            var fqn = string.IsNullOrEmpty(m.CsNamespace)
                ? m.CsName
                : "global::" + m.CsNamespace + "." + m.CsName;
            typeMap[m.GwcaName] = fqn;
        }

        // Build set of C export names (non-mangled exports)
        var cExports = new HashSet<string>(StringComparer.Ordinal);
        foreach (var name in exports)
        {
            if (name.Length > 0 && name[0] != '?')
                cExports.Add(name);
        }

        // Build namespace tree from mangled exports
        var root = new NamespaceNode("GWCA");
        int totalExports = 0;
        int skippedExports = 0;

        foreach (var mangledName in exports)
        {
            if (mangledName.Length == 0 || mangledName[0] != '?')
                continue; // skip C exports

            var info = Demangle(mangledName);
            if (info is null)
            {
                skippedExports++;
                continue;
            }

            totalExports++;

            // Walk the namespace parts to find/create the correct node
            var node = root;
            foreach (var ns in info.NamespaceParts)
            {
                if (!node.Children.TryGetValue(ns, out var child))
                {
                    child = new NamespaceNode(ns);
                    node.Children[ns] = child;
                }

                node = child;
            }

            node.Functions.Add(new ExportEntry(mangledName, info));
        }

        // ── Parse C++ headers for enums, constexpr, and structs ───
        var headerRoot = new ConstantsNode("GWCA");
        var dllDir = Path.GetDirectoryName(dllPath)!;
        var gwcaIncludeDir = Path.Combine(dllDir, "Include", "GWCA");
#pragma warning disable RS1035
        if (Directory.Exists(gwcaIncludeDir))
        {
            // Scan all subdirectories: Constants, GameEntities, GameContainers, Context, Managers, Packets
            foreach (var subDir in Directory.GetDirectories(gwcaIncludeDir))
            {
                foreach (var headerFile in Directory.GetFiles(subDir, "*.h"))
                {
                    try
                    {
                        var headerText = File.ReadAllText(headerFile);
                        CppHeaderParser.Parse(headerText, headerRoot);
                    }
                    catch
                    {
                        // Skip headers that fail to parse
                    }
                }
            }
            // Also scan root GWCA headers
            foreach (var headerFile in Directory.GetFiles(gwcaIncludeDir, "*.h"))
            {
                try
                {
                    var headerText = File.ReadAllText(headerFile);
                    CppHeaderParser.Parse(headerText, headerRoot);
                }
                catch
                {
                    // Skip headers that fail to parse
                }
            }
        }
#pragma warning restore RS1035

        // Collect names of all namespace classes from the export tree
        // These will be skipped when emitting structs to avoid name collisions
        var namespaceClassNames = new HashSet<string>(StringComparer.Ordinal);
        CollectNamespaceClassNames(root, namespaceClassNames);

        // Register generated enums in the typeMap so demangled signatures
        // resolve to the generated C# enum types instead of falling back
        // to int/uint. GWCAEquivalent entries take priority (already in map).
        // IMPORTANT: Collect enums FIRST, then structs, so struct collision detection works.
        CollectEnumMappings(headerRoot, "GWCA", typeMap);
        CollectStructMappings(headerRoot, "GWCA", typeMap, namespaceClassNames, new HashSet<string>(StringComparer.Ordinal));

        // Collect diagnostic info about parsed structs
        var structDiagnostics = new List<string>();
        CollectStructDiagnostics(headerRoot, "", structDiagnostics);

        // Emit
        var sb = new StringBuilder(65536);
        EmitHeader(sb, totalExports, skippedExports);
        
        // Add diagnostic comment about parsed structs
        sb.AppendLine("    // ═══════════════════════════════════════════════════");
        sb.AppendLine("    // Parsed structs diagnostic:");
        foreach (var diag in structDiagnostics.OrderBy(d => d))
        {
            sb.AppendLine($"    // {diag}");
        }
        sb.AppendLine("    // ═══════════════════════════════════════════════════");
        sb.AppendLine();

        // Emit the tree recursively (starting from GWCA's children)
        foreach (var child in root.Children.Values.OrderBy(c => c.Name, StringComparer.Ordinal))
        {
            EmitNamespaceNode(sb, child, typeMap, cExports, 4);
        }

        // Emit constants from headers (merge into the same GWCA class)
        // The headerRoot is "GWCA" → children are "GW", etc.
        // Each node emits as a partial class, which C# merges with the
        // identically-named class already emitted by the export tree.
        // Skip any child named "GWCA" - its content should go at root level, not nested.
        foreach (var child in headerRoot.Children.Values.OrderBy(c => c.Name, StringComparer.Ordinal))
        {
            if (child.Name == "GWCA")
            {
                // Emit GWCA namespace content directly at root level (not as nested class)
                // But first emit its children normally
                foreach (var grandchild in child.Children.Values.OrderBy(c => c.Name, StringComparer.Ordinal))
                {
                    EmitConstantsNode(sb, grandchild, typeMap, namespaceClassNames, 4);
                }
                continue;
            }
            EmitConstantsNode(sb, child, typeMap, namespaceClassNames, 4);
        }

        sb.AppendLine("}"); // Close GWCA class
        sb.AppendLine("}"); // Close Daybreak.API.Interop namespace
        
        // Emit structs and enums into GuildWars namespace for consumer code compatibility
        // Consumer code uses `using Daybreak.API.Interop.GuildWars;` to access types
        sb.AppendLine();
        sb.AppendLine("namespace Daybreak.API.Interop.GuildWars");
        sb.AppendLine("{");
        var emittedNames = new HashSet<string>(StringComparer.Ordinal);
        
        // Collect inline array types needed by struct fields (for non-blittable array fields)
        var inlineArrayTypes = new HashSet<(string csType, int size)>();
        CollectInlineArrayTypes(headerRoot, typeMap, inlineArrayTypes);
        
        // Emit manually-defined helper structs first (TLink, etc.)
        EmitManualHelperStructs(sb, 4, emittedNames);
        
        // Emit inline array types for non-blittable arrays (e.g., enum arrays)
        EmitInlineArrayTypes(sb, 4, inlineArrayTypes, emittedNames);
        
        // Emit enums first - they're often referenced by structs (e.g. Attribute enum vs Attribute struct)
        EmitEnumsToGuildWarsNamespace(sb, headerRoot, 4, emittedNames);
        EmitStructsToGuildWarsNamespace(sb, headerRoot, typeMap, 4, emittedNames, inlineArrayTypes);
        sb.AppendLine();
        EmitTypeAliasesToGuildWarsNamespace(sb, headerRoot, typeMap, 4, emittedNames);
        sb.AppendLine("}");

        // Write to Daybreak.API/Interop/GWCA.cs so LibraryImport generator can process it
        // DLL is at Dependencies/GWCA/gwca.dll, so go up 2 levels to repo root
        var repoRoot = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(dllPath)));
        var outputPath = Path.Combine(repoRoot!, "Daybreak.API", "Interop", "GWCA.cs");
#pragma warning disable RS1035 // File IO required to emit source that LibraryImport generator can process
        File.WriteAllText(outputPath, sb.ToString());
#pragma warning restore RS1035
    }

    // ── File header ────────────────────────────────────────────────

    private static void EmitHeader(StringBuilder sb, int exportCount, int skippedCount)
    {
        sb.AppendLine("// <auto-generated>");
        sb.AppendLine("// This file was generated by the GenerateGWCABindings source generator.");
        sb.AppendLine("// Do not edit manually — changes will be overwritten on rebuild.");
        sb.AppendLine("// </auto-generated>");
        sb.AppendLine();
        sb.AppendLine("#nullable enable");
        sb.AppendLine();
        sb.AppendLine("using System.Runtime.CompilerServices;");
        sb.AppendLine("using System.Runtime.InteropServices;");
        sb.AppendLine("using System.Runtime.InteropServices.Marshalling;");
        sb.AppendLine();
        sb.AppendLine("namespace Daybreak.API.Interop");
        sb.AppendLine("{");
        sb.AppendLine();
        sb.AppendLine("/// <summary>");
        sb.AppendLine($"/// P/Invoke bindings for {exportCount} C++ exports from gwca.dll ({skippedCount} skipped).");
        sb.AppendLine("/// Nested classes mirror the C++ namespace hierarchy (e.g. GW::Agents → GWCA.GW.Agents).");
        sb.AppendLine("/// Types annotated with [GWCAEquivalent] are used in signatures where available.");
        sb.AppendLine("/// </summary>");
        sb.AppendLine("public static unsafe partial class GWCA");
        sb.AppendLine("{");
        sb.AppendLine("    private const string DllName = \"gwca.dll\";");
    }

    // ── Recursive namespace/class emission ─────────────────────────

    private static void EmitNamespaceNode(
        StringBuilder sb,
        NamespaceNode node,
        Dictionary<string, string> typeMap,
        HashSet<string> cExports,
        int indent)
    {
        var pad = new string(' ', indent);

        sb.AppendLine();
        sb.AppendLine($"{pad}public static partial class {SanitizeIdentifier(node.Name)}");
        sb.AppendLine($"{pad}{{");

        // Emit functions in this namespace, disambiguating overloads that
        // would produce identical C# signatures
        var signatureCounts = new Dictionary<string, int>(StringComparer.Ordinal);
        foreach (var entry in node.Functions.OrderBy(e => e.Info.FunctionName, StringComparer.Ordinal))
        {
            EmitFunction(sb, entry, typeMap, cExports, indent + 4, signatureCounts);
        }

        // Emit child namespaces
        foreach (var child in node.Children.Values.OrderBy(c => c.Name, StringComparer.Ordinal))
        {
            EmitNamespaceNode(sb, child, typeMap, cExports, indent + 4);
        }

        sb.AppendLine($"{pad}}}");
    }

    // ── Single function emission ───────────────────────────────────

    private static void EmitFunction(
        StringBuilder sb,
        ExportEntry entry,
        Dictionary<string, string> typeMap,
        HashSet<string> cExports,
        int indent,
        Dictionary<string, int> signatureCounts)
    {
        var pad = new string(' ', indent);
        var info = entry.Info;

        // VarArgs — cannot be expressed with DllImport
        if (info.IsVarArgs)
        {
            sb.AppendLine();
            sb.AppendLine($"{pad}// {info.QualifiedCppName} — varargs, requires manual interop");
            return;
        }

        var csRet = ResolveCsType(info.ReturnType, typeMap);
        var retComment = TypeComment(info.ReturnType, typeMap);

        // Check for unmapped struct types
        bool hasUnmapped = HasUnmappedStruct(info.ReturnType, typeMap)
            || info.ParameterTypes.Any(pt => HasUnmappedStruct(pt, typeMap));

        // Check if this function has std::function params and a matching C export exists
        bool hasStdFunction = HasStdFunction(info.ReturnType) || info.ParameterTypes.Any(HasStdFunction);
        bool useCExport = hasStdFunction && cExports.Contains(info.FunctionName);
        if (useCExport)
        {
            // Re-evaluate hasUnmapped ignoring std::function (which becomes nint in C export)
            hasUnmapped = HasUnmappedStructIgnoringStdFunction(info.ReturnType, typeMap)
                || info.ParameterTypes.Any(pt => HasUnmappedStructIgnoringStdFunction(pt, typeMap));
        }

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
            // When using C export, std::function becomes a raw function pointer (nint)
            var csType = (useCExport && IsStdFunction(pt))
                ? "nint"
                : ResolveCsType(pt, typeMap);
            var name = MakeParamName(pt, typeMap, pIdx,
                info.ParameterTypes.Length + (info.IsMember ? 1 : 0));
            while (!usedNames.Add(name))
                name += "_" + pIdx;

            var comment = (useCExport && IsStdFunction(pt))
                ? null  // Don't add "unmapped" comment for std::function when using C export
                : TypeComment(pt, typeMap);
            if (comment is not null)
                paramComments.Add(name + ": " + comment);

            if (csType == "bool")
                parms.Add("[MarshalAs(UnmanagedType.U1)] bool " + name);
            else
                parms.Add(csType + " " + name);
            pIdx++;
        }

        var paramStr = string.Join(", ", parms);

        // Comment lines
        sb.AppendLine();
        var commentParts = new List<string> { info.QualifiedCppName };
        if (useCExport)
            commentParts.Add("via C export");
        if (retComment is not null)
            commentParts.Add("returns " + retComment);
        commentParts.AddRange(paramComments);

        // Disambiguate overloads that collapse to the same C# signature
        var sigKey = info.FunctionName + "(" + string.Join(",", parms.Select(p => p.Split(' ').First())) + ")";
        if (!signatureCounts.TryGetValue(sigKey, out var count))
            count = 0;
        signatureCounts[sigKey] = count + 1;
        var methodName = count == 0
            ? SanitizeIdentifier(info.FunctionName)
            : SanitizeIdentifier(info.FunctionName) + "_" + count;

        // Use C export name when available for std::function params
        var entryPoint = useCExport ? info.FunctionName : entry.MangledName;

        // Build LibraryImport attribute and calling convention
        var conventionAttr = info.IsMember
            ? $"[UnmanagedCallConv(CallConvs = [typeof(CallConvThiscall)])]"
            : "";

        // Bool return type needs explicit marshalling
        var retAttr = csRet == "bool" ? "[return: MarshalAs(UnmanagedType.U1)]" : "";

        var prefix = hasUnmapped ? "// " : "";
        sb.AppendLine($"{pad}// {string.Join(" | ", commentParts)}");
        sb.AppendLine($"{pad}{prefix}[LibraryImport(DllName, EntryPoint = \"{entryPoint}\")]");
        if (conventionAttr.Length > 0)
            sb.AppendLine($"{pad}{prefix}{conventionAttr}");
        if (retAttr.Length > 0)
            sb.AppendLine($"{pad}{prefix}{retAttr}");
        sb.AppendLine($"{pad}{prefix}public static partial {csRet} {methodName}({paramStr});");
    }

    // ════════════════════════════════════════════════════════════════
    // Constants / Enum emission from parsed headers
    // ════════════════════════════════════════════════════════════════

    /// <summary>
    /// Emits a <see cref="ConstantsNode"/> tree as nested static classes
    /// containing enums and const fields. If a namespace node already
    /// exists in the export tree, the constants are merged into it.
    /// </summary>
    private static void EmitConstantsNode(
        StringBuilder sb,
        ConstantsNode node,
        Dictionary<string, string> typeMap,
        HashSet<string> namespaceClassNames,
        int indent,
        string? emitNameOverride = null)
    {
        var pad = new string(' ', indent);

        // If the node has nothing to emit (no enums, no consts, no children with content), skip
        if (!HasAnyContent(node))
            return;

        var emitName = emitNameOverride ?? SanitizeIdentifier(node.Name);

        // Always emit a partial class wrapper — C# allows multiple partial
        // declarations for the same nested class, so this merges cleanly
        // with any class already emitted by the export tree.
        sb.AppendLine();
        sb.AppendLine($"{pad}public static partial class {emitName}");
        sb.AppendLine($"{pad}{{");

        var innerIndent = indent + 4;
        var innerPad = new string(' ', innerIndent);

        // Emit enums
        foreach (var enumDef in node.Enums.OrderBy(e => e.Name, StringComparer.Ordinal))
        {
            sb.AppendLine();
            if (enumDef.Name is not null)
            {
                var baseType = MapCppTypeToCs(enumDef.UnderlyingType);
                sb.AppendLine($"{innerPad}public enum {SanitizeIdentifier(enumDef.Name)} : {baseType}");
                sb.AppendLine($"{innerPad}{{");
                foreach (var member in enumDef.Members)
                {
                    var comment = member.Comment is not null ? $" // {member.Comment}" : "";
                    if (member.Value is not null)
                        sb.AppendLine($"{innerPad}    {SanitizeIdentifier(member.Name)} = {member.Value},{comment}");
                    else
                        sb.AppendLine($"{innerPad}    {SanitizeIdentifier(member.Name)},{comment}");
                }
                sb.AppendLine($"{innerPad}}}");
            }
            else
            {
                // Anonymous enum → emit as const fields
                foreach (var member in enumDef.Members)
                {
                    var csType = MapCppTypeToCs(enumDef.UnderlyingType);
                    var memberName = SanitizeIdentifier(member.Name);
                    // CS0542: member names cannot be the same as their enclosing type
                    if (memberName == emitName)
                        memberName += "_";
                    var comment = member.Comment is not null ? $" // {member.Comment}" : "";
                    sb.AppendLine($"{innerPad}internal const {csType} {memberName} = {member.Value};{comment}");
                }
            }
        }

        // Emit constexpr fields
        foreach (var field in node.Constants.OrderBy(f => f.Name, StringComparer.Ordinal))
        {
            var csType = MapCppTypeToCs(field.CppType);
            var value = field.Value;
            var fieldName = SanitizeIdentifier(field.Name);
            // CS0542: member names cannot be the same as their enclosing type
            if (fieldName == emitName)
                fieldName += "_";
            var comment = field.Comment is not null ? $" // {field.Comment}" : "";
            sb.AppendLine($"{innerPad}internal const {csType} {fieldName} = {value};{comment}");
        }

        // Type aliases are now emitted to GuildWars namespace, not nested GWCA classes
        // Skip type alias emission here - see EmitTypeAliasesToGuildWarsNamespace

        // Structs are now emitted to GuildWars namespace, not nested classes
        // Skip struct emission here - see EmitStructsToGuildWarsNamespace

        // Emit children
        foreach (var child in node.Children.Values.OrderBy(c => c.Name, StringComparer.Ordinal))
        {
            // CS0542: nested class name cannot be the same as enclosing type
            string? childNameOverride = null;
            if (SanitizeIdentifier(child.Name) == emitName)
                childNameOverride = SanitizeIdentifier(child.Name) + "_";
            EmitConstantsNode(sb, child, typeMap, namespaceClassNames, innerIndent, childNameOverride);
        }

        sb.AppendLine($"{pad}}}");
    }

    private static bool HasAnyContent(ConstantsNode node)
    {
        // Note: TypeAliases and Structs are emitted to GuildWars namespace, not counted here
        if (node.Enums.Count > 0 || node.Constants.Count > 0)
            return true;
        return node.Children.Values.Any(HasAnyContent);
    }

    /// <summary>
    /// Walks the constants tree and adds each named enum to the type map.
    /// Enums take priority over structs - this must be called first.
    /// Existing entries (from <c>[GWCAEquivalent]</c>) are never overwritten.
    /// </summary>
    private static void CollectEnumMappings(
        ConstantsNode node,
        string csPath,
        Dictionary<string, string> typeMap)
    {
        foreach (var enumDef in node.Enums)
        {
            if (enumDef.Name is null)
                continue; // skip anonymous enums

            if (!typeMap.ContainsKey(enumDef.Name))
                typeMap[enumDef.Name] = "global::Daybreak.API.Interop." + csPath + "." + SanitizeIdentifier(enumDef.Name);
        }

        foreach (var child in node.Children.Values)
        {
            CollectEnumMappings(child, csPath + "." + SanitizeIdentifier(child.Name), typeMap);
        }
    }

    /// <summary>
    /// Walks the constants tree and adds each struct to the type map.
    /// Must be called AFTER CollectEnumMappings so struct collision detection works.
    /// </summary>
    private static void CollectStructMappings(
        ConstantsNode node,
        string csPath,
        Dictionary<string, string> typeMap,
        HashSet<string> namespaceClassNames,
        HashSet<string> usedGuildWarsNames)
    {
        // First, collect all struct names in this node to avoid collisions
        var structNamesInNode = new HashSet<string>(node.Structs.Select(s => s.Name), StringComparer.Ordinal);

        foreach (var structDef in node.Structs)
        {
            // Only add structs that can actually be emitted
            if (!CanEmitStruct(structDef))
                continue;
                
            // Determine the C# name, handling collisions with namespace classes or enums
            var csName = SanitizeIdentifier(structDef.Name);
            
            // Check if name collides with a namespace class, enum, or ANOTHER struct already using this name
            if (namespaceClassNames.Contains(structDef.Name))
            {
                // Try "Data" suffix first, then "Struct" if that also collides
                if (!structNamesInNode.Contains(structDef.Name + "Data") && !usedGuildWarsNames.Contains(csName + "Data"))
                    csName += "Data";
                else if (!structNamesInNode.Contains(structDef.Name + "Struct") && !usedGuildWarsNames.Contains(csName + "Struct"))
                    csName += "Struct";
                else
                    csName += "_"; // Last resort
            }
            // Check if name collides with an enum already in typeMap (different namespace, e.g. Constants::Attribute vs GW::Attribute)
            // OR another struct already using this name in GuildWars namespace
            else if (typeMap.ContainsKey(structDef.Name) || usedGuildWarsNames.Contains(csName))
            {
                // An enum or other type with this name exists - suffix the struct
                if (!structNamesInNode.Contains(structDef.Name + "Struct") && !usedGuildWarsNames.Contains(csName + "Struct"))
                    csName += "Struct";
                else if (!structNamesInNode.Contains(structDef.Name + "Data") && !usedGuildWarsNames.Contains(csName + "Data"))
                    csName += "Data";
                else
                    csName += "_";
            }
            
            usedGuildWarsNames.Add(csName);
            
            // Use a qualified key that includes the C++ path to distinguish structs with the same name
            // from different namespaces (e.g., GW::Attribute vs GW::SkillbarMgr::Attribute)
            var mapKey = structDef.Name;
            var fqCsType = "global::Daybreak.API.Interop.GuildWars." + csName;
            var simpleStructKey = "struct " + structDef.Name;
            
            if (typeMap.ContainsKey(mapKey))
            {
                // Use qualified key to distinguish this struct from enum or another struct
                // Include the csPath to make it unique (e.g., "struct GW.Attribute" vs "struct GW.SkillbarMgr.Attribute")
                mapKey = "struct " + csPath.Replace("GWCA.", "") + "." + structDef.Name;
                
                // Also add simple "struct X" key if not already taken (for field type resolution)
                // This allows MapCppFieldTypeToCs to find the struct without knowing the full path
                if (!typeMap.ContainsKey(simpleStructKey))
                {
                    typeMap[simpleStructKey] = fqCsType;
                }
            }
            
            // Structs go into GuildWars namespace (flat), not nested GWCA classes
            typeMap[mapKey] = fqCsType;
        }

        foreach (var child in node.Children.Values)
        {
            CollectStructMappings(child, csPath + "." + SanitizeIdentifier(child.Name), typeMap, namespaceClassNames, usedGuildWarsNames);
        }
    }

    /// <summary>
    /// Collects all namespace class names from the export tree recursively.
    /// Used to avoid name collisions when emitting structs.
    /// </summary>
    private static void CollectNamespaceClassNames(NamespaceNode node, HashSet<string> names)
    {
        names.Add(node.Name);
        foreach (var child in node.Children.Values)
        {
            CollectNamespaceClassNames(child, names);
        }
    }

    /// <summary>
    /// Collects diagnostic information about parsed structs.
    /// </summary>
    private static void CollectStructDiagnostics(ConstantsNode node, string path, List<string> diagnostics)
    {
        var currentPath = string.IsNullOrEmpty(path) ? node.Name : path + "." + node.Name;
        
        // Show namespace pop line info
        if (node.DebugPopLine > 0)
        {
            diagnostics.Add($"[NAMESPACE] {currentPath} popped at line {node.DebugPopLine}");
        }
        
        foreach (var structDef in node.Structs)
        {
            var (canEmit, reason) = CanEmitStructWithReason(structDef);
            var status = canEmit ? "OK" : $"SKIP: {reason}";
            diagnostics.Add($"{currentPath}.{structDef.Name}: {structDef.Fields.Count} fields [{status}]");
        }
        foreach (var child in node.Children.Values)
        {
            CollectStructDiagnostics(child, currentPath, diagnostics);
        }
    }

    /// <summary>
    /// Checks if a struct can be emitted (has no complex template fields or unresolved types).
    /// </summary>
    private static (bool canEmit, string? reason) CanEmitStructWithReason(CppStructDef structDef)
    {
        // Skip structs with no fields (usually forward declarations that got parsed)
        if (structDef.Fields.Count == 0)
            return (false, "no fields");
        
        // Check for mixed offset/no-offset fields (can't use Explicit layout if not all have offsets)
        bool hasAnyOffset = structDef.Fields.Any(f => f.Offset.HasValue);
        bool allHaveOffsets = structDef.Fields.All(f => f.Offset.HasValue);
        if (hasAnyOffset && !allHaveOffsets)
            return (false, "mixed offset fields");

        // Check for unresolvable fields (template types, missing types)
        foreach (var field in structDef.Fields)
        {
            var cppType = field.CppType;
            // Skip structs with template type parameters (T, etc.)
            if (Regex.IsMatch(cppType, @"\bT\b") && !cppType.Contains("Array"))
                return (false, $"template param T in field {field.Name}");
            // Skip structs with complex template containers we can't represent
            // Note: TLink<T> is handled separately in MapCppFieldTypeToCs (8-byte linked list node)
            if (cppType.Contains("TList<") || 
                cppType.Contains("PrioQ<") || cppType.Contains("PrioQLink<") ||
                cppType.Contains("BaseArray<"))
                return (false, $"complex template in field {field.Name}: {cppType}");
            // Skip if field type contains unresolved typedef names
            // Note: UIInteractionCallback is now mapped to nint in BuiltInTypeMappings
            // Note: FrameRelation* (pointer) maps to nint automatically, but embedded FrameRelation
            //       is a problem because the struct itself is skipped (contains TList<FrameRelation>)
            if (cppType.Contains("FriendsListArray") ||
                cppType.Contains("PathNodeArray") || cppType.Contains("PathingMapArray") ||
                cppType.Contains("BlockedPlaneArray") || cppType.Contains("AgentSummaryInfoSub") ||
                cppType.Contains("EffectData") ||
                cppType.Contains("SkillbarSkillData") || cppType.Contains("SkillbarData") ||
                cppType == "FrameRelation") // Embedded FrameRelation - struct is skipped due to TList<>
                return (false, $"unresolved typedef in field {field.Name}: {cppType}");
            // Skip structs with function pointer fields (complex vtable types)
            if (cppType.Contains("__fastcall") || cppType.Contains("__stdcall") ||
                cppType.Contains("(__cdecl") || Regex.IsMatch(cppType, @"\(\s*\*\s*\w+\s*\)"))
                return (false, $"function pointer in field {field.Name}: {cppType}");
            // Skip structs with inline initializers in field declarations (e.g., "uint32_t k[4]{}")
            if (cppType.Contains("{}"))
                return (false, $"inline initializer in field {field.Name}: {cppType}");
            // Skip fields referencing complex struct types we can't emit
            // These are structs that have vtables, function pointers, or complex layouts
            if (cppType.Contains("EquipmentVTable") || cppType.Contains("VTable"))
                return (false, $"vtable in field {field.Name}: {cppType}");
        }
        return (true, null);
    }
    
    private static bool CanEmitStruct(CppStructDef structDef) => CanEmitStructWithReason(structDef).canEmit;

    /// <summary>
    /// Emits a C# struct from a parsed C++ struct definition.
    /// </summary>
    private static void EmitStruct(StringBuilder sb, CppStructDef structDef, Dictionary<string, string> typeMap, int indent, HashSet<(string csType, int size)>? inlineArrayTypes = null, string? csStructName = null)
    {
        var pad = new string(' ', indent);

        // Skip structs that can't be emitted
        if (!CanEmitStruct(structDef))
            return;

        // Use provided name or default to sanitized original name
        var structName = csStructName ?? SanitizeIdentifier(structDef.Name);

        sb.AppendLine();

        // Determine layout strategy based on whether we have offsets
        bool hasOffsets = structDef.Fields.Any(f => f.Offset.HasValue);
        var layoutKind = hasOffsets ? "Explicit" : "Sequential";

        // Build the struct attributes
        if (structDef.Size.HasValue)
            sb.AppendLine($"{pad}[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.{layoutKind}, Pack = 1, Size = 0x{structDef.Size.Value:X})]");
        else
            sb.AppendLine($"{pad}[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.{layoutKind}, Pack = 1)]");

        sb.AppendLine($"{pad}public unsafe struct {structName}");
        sb.AppendLine($"{pad}{{");

        var innerPad = new string(' ', indent + 4);

        foreach (var field in structDef.Fields)
        {
            var csType = MapCppFieldTypeToCs(field.CppType, typeMap);
            var fieldName = SanitizeIdentifier(ToPascalCase(field.Name));
            var comment = field.Comment is not null ? $" // {field.Comment}" : "";

            // Emit FieldOffset if we have explicit layout
            if (hasOffsets && field.Offset.HasValue)
                sb.AppendLine($"{innerPad}[global::System.Runtime.InteropServices.FieldOffset(0x{field.Offset.Value:X4})]");

            // Handle fixed-size arrays
            if (field.ArraySize is not null)
            {
                var size = ParseArraySize(field.ArraySize);
                // Skip zero-length arrays (flexible array members in C++) - just add a comment
                if (size <= 0)
                {
                    sb.AppendLine($"{innerPad}// Flexible array member: {csType} {fieldName}[0]");
                    continue;
                }
                // For fixed arrays, we need a fixed buffer or explicit FieldOffset per element
                // Use fixed buffer for blittable types
                if (IsBlittableForFixed(csType))
                {
                    sb.AppendLine($"{innerPad}public fixed {csType} {fieldName}[{size}];{comment}");
                }
                else if (inlineArrayTypes is not null && inlineArrayTypes.Contains((csType, size)))
                {
                    // Use the inline array type we generated
                    var inlineArrayTypeName = GetInlineArrayTypeName(csType, size);
                    sb.AppendLine($"{innerPad}public {inlineArrayTypeName} {fieldName};{comment}");
                }
                else
                {
                    // Fallback: use single element with comment (shouldn't happen if inlineArrayTypes is properly populated)
                    sb.AppendLine($"{innerPad}public {csType} {fieldName}; // [{field.ArraySize}]{comment}");
                }
            }
            else
            {
                sb.AppendLine($"{innerPad}public {csType} {fieldName};{comment}");
            }
        }

        sb.AppendLine($"{pad}}}");
    }

    /// <summary>
    /// Emits all emittable structs from the constants tree into the GuildWars namespace.
    /// This provides compatibility for consumer code that uses `using Daybreak.API.Interop.GuildWars;`
    /// </summary>
    private static void EmitStructsToGuildWarsNamespace(StringBuilder sb, ConstantsNode node, Dictionary<string, string> typeMap, int indent, HashSet<string> emittedNames, HashSet<(string csType, int size)> inlineArrayTypes)
    {
        // Start with the node's own name as the path base (GWCA for headerRoot)
        EmitStructsRecursive(sb, node, node.Name, typeMap, indent, emittedNames, inlineArrayTypes);
    }
    
    private static void EmitStructsRecursive(StringBuilder sb, ConstantsNode node, string currentPath, Dictionary<string, string> typeMap, int indent, HashSet<string> emittedNames, HashSet<(string csType, int size)> inlineArrayTypes)
    {
        foreach (var structDef in node.Structs.OrderBy(s => s.Name, StringComparer.Ordinal))
        {
            // Skip structs that can't be emitted
            if (!CanEmitStruct(structDef))
                continue;
            
            // Get the C# name from typeMap (which includes collision-resolution suffixes like "Struct")
            // Try direct name first, then qualified struct key (for collision cases)
            string? fqCsName;
            var directLookup = typeMap.TryGetValue(structDef.Name, out fqCsName);
            var containsGuildWars = fqCsName?.Contains("GuildWars.") ?? false;
            
            if (!directLookup || !containsGuildWars)
            {
                // If direct lookup failed or returned an enum (not in GuildWars namespace), 
                // try qualified struct key: "struct GW.StructName"
                var qualifiedKey = "struct " + currentPath.Replace("GWCA.", "") + "." + structDef.Name;
                typeMap.TryGetValue(qualifiedKey, out fqCsName);
            }
            
            if (fqCsName is null)
                continue;
                
            // Extract just the struct name from the fully-qualified name
            // e.g. "global::Daybreak.API.Interop.GuildWars.ItemStruct" -> "ItemStruct"
            var csName = fqCsName.Substring(fqCsName.LastIndexOf('.') + 1);
            
            // Skip duplicates (including if an enum with the same name was emitted)
            if (emittedNames.Contains(csName))
                continue;
                
            emittedNames.Add(csName);
            EmitStruct(sb, structDef, typeMap, indent, inlineArrayTypes, csName);
        }
        
        // Recurse into children, appending the child's name to the current path
        foreach (var child in node.Children.Values)
        {
            EmitStructsRecursive(sb, child, currentPath + "." + child.Name, typeMap, indent, emittedNames, inlineArrayTypes);
        }
    }

    /// <summary>
    /// Emits manually-defined helper structs that can't be parsed from headers.
    /// These are typically C++ template containers with known fixed layouts.
    /// </summary>
    private static void EmitManualHelperStructs(StringBuilder sb, int indent, HashSet<string> emittedNames)
    {
        var pad = new string(' ', indent);
        
        // TLink<T> - doubly-linked list node used in Agent and other structs
        // C++ definition: struct TLink { TLink* prev_link; T* next_node; }
        // Size: 8 bytes (2 pointers on x86)
        if (emittedNames.Add("TLink"))
        {
            sb.AppendLine($"{pad}/// <summary>");
            sb.AppendLine($"{pad}/// TLink&lt;T&gt; - doubly-linked list node (8 bytes: 2 pointers).");
            sb.AppendLine($"{pad}/// Used in Agent structs for linked list chaining.");
            sb.AppendLine($"{pad}/// </summary>");
            sb.AppendLine($"{pad}[global::System.Runtime.InteropServices.StructLayout(global::System.Runtime.InteropServices.LayoutKind.Sequential, Pack = 1)]");
            sb.AppendLine($"{pad}public struct TLink");
            sb.AppendLine($"{pad}{{");
            sb.AppendLine($"{pad}    public nint PrevLink;");
            sb.AppendLine($"{pad}    public nint NextNode;");
            sb.AppendLine($"{pad}}}");
            sb.AppendLine();
        }
    }

    /// <summary>
    /// Collects all inline array types needed for non-blittable array fields in emittable structs.
    /// </summary>
    private static void CollectInlineArrayTypes(ConstantsNode node, Dictionary<string, string> typeMap, HashSet<(string csType, int size)> inlineArrayTypes)
    {
        foreach (var structDef in node.Structs)
        {
            if (!CanEmitStruct(structDef))
                continue;
                
            foreach (var field in structDef.Fields)
            {
                if (field.ArraySize is null)
                    continue;
                    
                var size = ParseArraySize(field.ArraySize);
                if (size <= 0)
                    continue;
                    
                var csType = MapCppFieldTypeToCs(field.CppType, typeMap);
                if (!IsBlittableForFixed(csType))
                {
                    inlineArrayTypes.Add((csType, size));
                }
            }
        }
        
        foreach (var child in node.Children.Values)
        {
            CollectInlineArrayTypes(child, typeMap, inlineArrayTypes);
        }
    }

    /// <summary>
    /// Emits InlineArray types for non-blittable array fields.
    /// Uses [InlineArray(N)] attribute introduced in .NET 8.
    /// </summary>
    private static void EmitInlineArrayTypes(StringBuilder sb, int indent, HashSet<(string csType, int size)> inlineArrayTypes, HashSet<string> emittedNames)
    {
        var pad = new string(' ', indent);
        
        foreach (var (csType, size) in inlineArrayTypes.OrderBy(x => x.csType).ThenBy(x => x.size))
        {
            // Generate a name like "AttributeArray12" or "SkillIDArray8"
            var simpleName = GetSimpleTypeName(csType);
            var arrayTypeName = $"{simpleName}Array{size}";
            
            if (!emittedNames.Add(arrayTypeName))
                continue;
            
            sb.AppendLine($"{pad}/// <summary>");
            sb.AppendLine($"{pad}/// Inline array of {size} {simpleName} elements.");
            sb.AppendLine($"{pad}/// </summary>");
            sb.AppendLine($"{pad}[global::System.Runtime.CompilerServices.InlineArray({size})]");
            sb.AppendLine($"{pad}public unsafe struct {arrayTypeName}");
            sb.AppendLine($"{pad}{{");
            sb.AppendLine($"{pad}    private {csType} _element0;");
            sb.AppendLine($"{pad}}}");
            sb.AppendLine();
        }
    }

    /// <summary>
    /// Extracts the simple type name from a fully-qualified type name.
    /// e.g., "global::Daybreak.API.Interop.GuildWars.Attribute" -> "Attribute"
    /// Handles pointer types: "ChatMessage*" -> "ChatMessagePtr"
    /// </summary>
    private static string GetSimpleTypeName(string csType)
    {
        // Handle pointer types
        var isPointer = csType.EndsWith("*");
        if (isPointer)
            csType = csType.TrimEnd('*');
            
        var lastDot = csType.LastIndexOf('.');
        var name = lastDot >= 0 ? csType.Substring(lastDot + 1) : csType;
        
        // Append Ptr for pointer types to make valid identifier
        return isPointer ? name + "Ptr" : name;
    }

    /// <summary>
    /// Gets the inline array type name for a given element type and size.
    /// </summary>
    private static string GetInlineArrayTypeName(string csType, int size)
    {
        var simpleName = GetSimpleTypeName(csType);
        return $"{simpleName}Array{size}";
    }

    /// <summary>
    /// Emits all enums from the constants tree into the GuildWars namespace.
    /// This provides compatibility for consumer code that uses `using Daybreak.API.Interop.GuildWars;`
    /// </summary>
    private static void EmitEnumsToGuildWarsNamespace(StringBuilder sb, ConstantsNode node, int indent, HashSet<string> emittedNames)
    {
        EmitEnumsRecursive(sb, node, indent, emittedNames);
    }
    
    private static void EmitEnumsRecursive(StringBuilder sb, ConstantsNode node, int indent, HashSet<string> emittedNames)
    {
        var pad = new string(' ', indent);
        
        foreach (var enumDef in node.Enums.OrderBy(e => e.Name, StringComparer.Ordinal))
        {
            // Skip anonymous enums
            if (enumDef.Name is null)
                continue;
            
            // Skip duplicates (including if a struct with the same name was emitted)
            if (emittedNames.Contains(enumDef.Name))
                continue;
                
            emittedNames.Add(enumDef.Name);
            
            // Determine base type and map C++ types to C#
            var baseType = MapCppEnumBaseType(enumDef.UnderlyingType ?? "int");
            
            sb.AppendLine();
            sb.AppendLine($"{pad}public enum {SanitizeIdentifier(enumDef.Name)} : {baseType}");
            sb.AppendLine($"{pad}{{");
            
            var innerPad = new string(' ', indent + 4);
            foreach (var member in enumDef.Members)
            {
                if (member.Value is not null)
                    sb.AppendLine($"{innerPad}{SanitizeIdentifier(member.Name)} = {member.Value},");
                else
                    sb.AppendLine($"{innerPad}{SanitizeIdentifier(member.Name)},");
            }
            sb.AppendLine($"{pad}}}");
        }
        
        // Recurse into children
        foreach (var child in node.Children.Values)
        {
            EmitEnumsRecursive(sb, child, indent, emittedNames);
        }
    }

    /// <summary>
    /// Emits all type aliases (typedef Array<T>) from the constants tree into the GuildWars namespace.
    /// </summary>
    private static void EmitTypeAliasesToGuildWarsNamespace(StringBuilder sb, ConstantsNode node, Dictionary<string, string> typeMap, int indent, HashSet<string> emittedNames)
    {
        EmitTypeAliasesRecursive(sb, node, typeMap, indent, emittedNames);
    }
    
    private static void EmitTypeAliasesRecursive(StringBuilder sb, ConstantsNode node, Dictionary<string, string> typeMap, int indent, HashSet<string> emittedNames)
    {
        var pad = new string(' ', indent);
        
        foreach (var alias in node.TypeAliases.OrderBy(a => a.AliasName, StringComparer.Ordinal))
        {
            // Skip duplicates
            if (emittedNames.Contains(alias.AliasName))
                continue;
                
            emittedNames.Add(alias.AliasName);
            
            var innerType = alias.TemplateArg.Replace("::", ".").Trim();
            // Handle pointer types (e.g., "Item *" -> "nint")
            if (innerType.EndsWith("*"))
                innerType = "nint";
            else if (typeMap.TryGetValue(innerType, out var mapped))
                innerType = mapped;
            else
            {
                // Try primitive type mapping
                var primitiveMapping = MapPrimitiveType(innerType);
                if (primitiveMapping != innerType)
                    innerType = primitiveMapping;
                else
                    // Unmapped struct type - use nint as fallback
                    innerType = "nint";
            }
            
            sb.AppendLine($"{pad}public unsafe struct {alias.AliasName} {{ public global::Daybreak.API.Interop.GuildWars.GuildWarsArray<{innerType}> Value; }}");
        }
        
        // Recurse into children
        foreach (var child in node.Children.Values)
        {
            EmitTypeAliasesRecursive(sb, child, typeMap, indent, emittedNames);
        }
    }

    /// <summary>
    /// Maps a C++ enum underlying type to C# type.
    /// </summary>
    private static string MapCppEnumBaseType(string cppType)
    {
        return cppType switch
        {
            "uint8_t" or "unsigned char" => "byte",
            "int8_t" or "char" or "signed char" => "sbyte",
            "uint16_t" or "unsigned short" => "ushort",
            "int16_t" or "short" => "short",
            "uint32_t" or "unsigned int" or "unsigned long" => "uint",
            "int32_t" or "int" or "long" => "int",
            "uint64_t" or "unsigned long long" => "ulong",
            "int64_t" or "long long" => "long",
            _ => "int"
        };
    }

    /// <summary>
    /// Maps a C++ field type to a C# type for struct fields.
    /// Handles pointers, qualified types, etc.
    /// </summary>
    private static string MapCppFieldTypeToCs(string cppType, Dictionary<string, string> typeMap)
    {
        cppType = cppType.Trim();

        // Strip leading 'const' keyword
        if (cppType.StartsWith("const "))
            cppType = cppType.Substring(6).Trim();

        // Strip leading 'struct' keyword (C++ forward decl style)
        if (cppType.StartsWith("struct "))
            cppType = cppType.Substring(7).Trim();

        // Handle 'struct' inside template arguments (e.g., Array<struct Node*>)
        cppType = Regex.Replace(cppType, @"<\s*struct\s+", "<");

        // Strip namespace prefix before template detection (GW::Array<> -> Array<>)
        // But preserve the inner type for recursive processing
        if (cppType.StartsWith("GW::"))
            cppType = cppType.Substring(4);

        // Handle Array<T> - GWCA's Array template which is a {T* data, uint size, uint capacity} (12 bytes)
        if (cppType.StartsWith("Array<"))
        {
            // Extract the inner type from Array<T>
            var innerStart = cppType.IndexOf('<') + 1;
            var innerEnd = cppType.LastIndexOf('>');
            if (innerEnd > innerStart)
            {
                var innerCpp = cppType.Substring(innerStart, innerEnd - innerStart).Trim();
                // Handle pointer inner types (e.g., Array<Agent*>)
                // C# does NOT allow pointer types as generic type arguments (CS0306), so use nint
                if (innerCpp.EndsWith("*"))
                {
                    return "global::Daybreak.API.Interop.GuildWars.GuildWarsArray<nint>";
                }
                // Handle non-pointer inner types
                // Strip "struct " prefix
                if (innerCpp.StartsWith("struct "))
                    innerCpp = innerCpp.Substring(7).Trim();
                // Strip namespace
                if (innerCpp.Contains("::"))
                {
                    var parts = innerCpp.Split(new[] { "::" }, StringSplitOptions.RemoveEmptyEntries);
                    innerCpp = parts[parts.Length - 1];
                }
                // Map primitives
                var csInner = MapPrimitiveType(innerCpp);
                if (csInner != innerCpp)
                    return "global::Daybreak.API.Interop.GuildWars.GuildWarsArray<" + csInner + ">";
                // Check typeMap for structs/enums
                if (typeMap.TryGetValue(innerCpp, out var mappedInner))
                    return "global::Daybreak.API.Interop.GuildWars.GuildWarsArray<" + mappedInner + ">";
                // Fallback for unmapped types
                return "global::Daybreak.API.Interop.GuildWars.GuildWarsArray<nint>";
            }
            return "global::Daybreak.API.Interop.GuildWars.GuildWarsArray<nint>";
        }

        // Handle TLink<T> - linked list node, always 8 bytes (2 pointers)
        if (cppType.StartsWith("TLink<"))
        {
            return "global::Daybreak.API.Interop.GuildWars.TLink";
        }

        // Handle pointers
        bool isPointer = cppType.EndsWith("*");
        if (isPointer)
        {
            var inner = cppType.TrimEnd('*').Trim();
            // Strip namespace qualifiers before lookup
            var innerStripped = inner;
            if (inner.Contains("::"))
            {
                var parts = inner.Split(new[] { "::" }, StringSplitOptions.RemoveEmptyEntries);
                innerStripped = parts[parts.Length - 1];
            }
            
            var csInner = MapCppFieldTypeToCs(inner, typeMap);
            // Special case: void* and char* and wchar_t* map to nint
            if (csInner is "void" or "char" or "byte")
                return "nint";
            // If the inner type is unmapped (csInner equals the stripped name), use nint for the pointer
            // A mapped type will have a fully-qualified name or be a known primitive
            if (!IsKnownCsType(csInner) && csInner == innerStripped)
                return "nint";
            return csInner + "*";
        }

        // Check if original type is from Constants namespace (typically enums)
        // In this case, prefer enum mapping over struct mapping
        var isFromConstantsNamespace = cppType.Contains("Constants::");
        
        // Strip namespace qualifiers (GW::, GW::Constants::, etc.)
        if (cppType.Contains("::"))
        {
            var parts = cppType.Split(new[] { "::" }, StringSplitOptions.RemoveEmptyEntries);
            cppType = parts[parts.Length - 1];
        }

        // Check if this type is in our generated enums/structs map
        if (typeMap.TryGetValue(cppType, out var mapped))
        {
            // If original was from Constants:: namespace, it's an enum - use enum mapping
            if (isFromConstantsNamespace)
                return mapped;
            // Otherwise, if it's a GuildWars type (struct), use it
            if (mapped.Contains("GuildWars."))
                return mapped;
        }
        // Try struct-prefixed key only if NOT from Constants namespace
        // (for collision cases like Attribute struct vs enum)
        if (!isFromConstantsNamespace && typeMap.TryGetValue("struct " + cppType, out var structAlt))
            return structAlt;
        // Fall back to original mapping (could be an enum)
        if (mapped is not null)
            return mapped;

        // Handle common C++ types
        return cppType switch
        {
            "uint32_t" => "uint",
            "int32_t" => "int",
            "uint16_t" => "ushort",
            "int16_t" => "short",
            "uint8_t" => "byte",
            "int8_t" => "sbyte",
            "uint64_t" => "ulong",
            "int64_t" => "long",
            "size_t" => "nuint",
            "int" => "int",
            "unsigned" => "uint",
            "unsigned int" => "uint",
            "short" => "short",
            "unsigned short" => "ushort",
            "long" => "int",
            "unsigned long" => "uint",
            "float" => "float",
            "double" => "double",
            "bool" => "byte", // C++ bool is 1 byte
            "wchar_t" => "char",
            "char" => "byte",
            "void" => "void",
            "DWORD" => "uint",
            "FILETIME" => "ulong", // FILETIME is two DWORDs
            "HMODULE" => "nint",
            "HANDLE" => "nint",
            "HWND" => "nint",
            "uintptr_t" => "nuint",
            "intptr_t" => "nint",
            "Vec2f" => "global::System.Numerics.Vector2",
            "Vec3f" => "global::System.Numerics.Vector3",
            "Color" => "uint", // GWCA Color is usually uint32
            // Known GWCA types that are typedefs to uint32_t (not enums)
            "AgentID" => "uint",
            "PlayerNumber" => "uint",
            "ItemID" => "uint",
            "PlayerID" => "uint",
            // Everything else: assume it's a struct/enum name we'll reference directly
            _ => cppType,
        };
    }

    /// <summary>
    /// Checks if a type name is a known C# primitive or built-in type.
    /// </summary>
    private static bool IsKnownCsType(string csType)
    {
        return csType is "byte" or "sbyte" or "short" or "ushort" or "int" or "uint"
            or "long" or "ulong" or "float" or "double" or "char" or "nint" or "nuint"
            or "void" or "bool" or "string" or "object"
            or "global::System.Numerics.Vector2" or "global::System.Numerics.Vector3";
    }

    private static bool IsBlittableForFixed(string csType)
    {
        // Note: nint and nuint are NOT valid for fixed buffers in C#
        return csType is "byte" or "sbyte" or "short" or "ushort" or "int" or "uint"
            or "long" or "ulong" or "float" or "double" or "char";
    }

    private static int ParseArraySize(string sizeExpr)
    {
        sizeExpr = sizeExpr.Trim();
        if (sizeExpr.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            return Convert.ToInt32(sizeExpr, 16);
        if (int.TryParse(sizeExpr, out var size))
            return size;
        // If it's a named constant or expression, default to 1
        return 1;
    }

    private static string ToPascalCase(string name)
    {
        if (string.IsNullOrEmpty(name))
            return name;

        // Handle snake_case: split by underscores and capitalize each part
        if (name.Contains("_"))
        {
            var parts = name.Split('_');
            var result = new StringBuilder();
            foreach (var part in parts)
            {
                if (part.Length > 0)
                {
                    result.Append(char.ToUpperInvariant(part[0]));
                    if (part.Length > 1)
                        result.Append(part.Substring(1));
                }
            }
            return result.ToString();
        }

        // Just capitalize first letter
        return char.ToUpperInvariant(name[0]) + name.Substring(1);
    }

    private static string MapCppTypeToCs(string? cppType)
    {
        if (cppType is null)
            return "int";
        return cppType switch
        {
            "uint32_t" => "uint",
            "int32_t" => "int",
            "uint16_t" => "ushort",
            "int16_t" => "short",
            "uint8_t" => "byte",
            "int8_t" => "sbyte",
            "int" => "int",
            "unsigned int" => "uint",
            "size_t" => "uint",
            "float" => "float",
            "double" => "double",
            _ => "int",
        };
    }

    // ════════════════════════════════════════════════════════════════
    // Identifier helpers
    // ════════════════════════════════════════════════════════════════

    private static readonly HashSet<string> CsKeywords =
    [
        "abstract", "as", "base", "bool", "break", "byte", "case", "catch",
        "char", "checked", "class", "const", "continue", "decimal", "default",
        "delegate", "do", "double", "else", "enum", "event", "explicit",
        "extern", "false", "finally", "fixed", "float", "for", "foreach",
        "goto", "if", "implicit", "in", "int", "interface", "internal", "is",
        "lock", "long", "namespace", "new", "null", "object", "operator",
        "out", "override", "params", "private", "protected", "public",
        "readonly", "ref", "return", "sbyte", "sealed", "short", "sizeof",
        "stackalloc", "static", "string", "struct", "switch", "this", "throw",
        "true", "try", "typeof", "uint", "ulong", "unchecked", "unsafe",
        "ushort", "using", "virtual", "void", "volatile", "while",
    ];

    private static string SanitizeIdentifier(string name)
    {
        if (string.IsNullOrEmpty(name))
            return "_";
        if (CsKeywords.Contains(name))
            return "@" + name;
        // If identifier starts with a digit, prefix with underscore
        if (char.IsDigit(name[0]))
            return "_" + name;
        return name;
    }

    // ════════════════════════════════════════════════════════════════
    // Type helpers
    // ════════════════════════════════════════════════════════════════

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
    /// Returns true if the type is a std::function (struct named "function").
    /// </summary>
    private static bool IsStdFunction(DemangledType dt)
    {
        // std::function is demangled as a Struct with name "function"
        if (dt.Kind is TypeKind.Struct && dt.Name == "function")
            return true;
        // Also check pointer to std::function (const ref)
        if (dt.Kind is TypeKind.Pointer && dt.Name == "function")
            return true;
        return false;
    }

    /// <summary>
    /// Returns true if any parameter or return type involves std::function.
    /// </summary>
    private static bool HasStdFunction(DemangledType dt)
    {
        if (IsStdFunction(dt))
            return true;
        if (dt.TemplateArg is not null && HasStdFunction(dt.TemplateArg))
            return true;
        return false;
    }

    /// <summary>
    /// Like HasUnmappedStruct but ignores std::function types (which will be converted to nint via C export).
    /// </summary>
    private static bool HasUnmappedStructIgnoringStdFunction(DemangledType dt, Dictionary<string, string> typeMap)
    {
        // Skip std::function types entirely
        if (IsStdFunction(dt))
            return false;

        if (dt.TemplateArg is not null && HasUnmappedStructIgnoringStdFunction(dt.TemplateArg, typeMap))
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
                        return "global::Daybreak.API.Interop.GuildWars.GuildWarsArray<" + innerType + ">*";
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
                        _ => ResolveStructOrEnumPointerInner(dt.Name, typeMap),
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
                    return "global::Daybreak.API.Interop.GuildWars.GuildWarsArray<" + innerType + ">";
                }

                // First try direct lookup, but ensure we get a struct (GuildWars namespace), not an enum
                if (typeMap.TryGetValue(dt.Name, out var structCs))
                {
                    // If it's a GuildWars type, use it (it's the struct)
                    if (structCs.Contains("GuildWars."))
                        return structCs;
                }
                // If direct lookup returned an enum, try struct-prefixed key
                if (typeMap.TryGetValue("struct " + dt.Name, out var structCsAlt))
                    return structCsAlt;
                // Fall back to direct lookup result or name
                if (structCs is not null)
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
            TypeKind.Primitive => MapPrimitiveType(arg.Name),
            TypeKind.Enum => typeMap.TryGetValue(arg.Name, out var e) ? e : arg.Name,
            TypeKind.Struct => ResolveStructType(arg.Name, typeMap),
            TypeKind.Pointer => "nint", // C# does NOT allow pointer types as generic type arguments (CS0306)
            _ => "nint",
        };
    }
    
    /// <summary>
    /// Resolves a struct name to its C# type, handling name collisions with enums.
    /// </summary>
    private static string ResolveStructType(string name, Dictionary<string, string> typeMap)
    {
        // Try direct lookup - if it's in GuildWars namespace, it's the struct
        if (typeMap.TryGetValue(name, out var direct) && direct.Contains("GuildWars."))
            return direct;
        // Try struct-prefixed key (for collision cases like Attribute struct vs enum)
        if (typeMap.TryGetValue("struct " + name, out var structAlt))
            return structAlt;
        // Unmapped struct -> nint
        return "nint";
    }
    
    /// <summary>
    /// Resolves a pointer inner type (the pointee) for struct or enum types,
    /// handling name collisions between structs and enums.
    /// </summary>
    private static string ResolveStructOrEnumPointerInner(string name, Dictionary<string, string> typeMap)
    {
        // Try direct lookup - if it's in GuildWars namespace, it's a struct
        if (typeMap.TryGetValue(name, out var direct) && direct.Contains("GuildWars."))
            return direct;
        // Try struct-prefixed key (for collision cases like Attribute struct vs enum)
        if (typeMap.TryGetValue("struct " + name, out var structAlt))
            return structAlt;
        // Fall back to direct lookup result (could be an enum) or just the name
        return direct ?? name;
    }
    
    /// <summary>
    /// Resolves a pointer type used as a template argument (e.g., Agent* in Array&lt;Agent*&gt;).
    /// In unsafe C#, pointer types can be used as generic type arguments for unmanaged structs.
    /// </summary>
    private static string ResolvePointerTemplateArg(DemangledType arg, Dictionary<string, string> typeMap)
    {
        // arg.Name is the inner type (e.g., "Agent" for Agent*)
        var innerName = arg.Name;
        
        // Check if the inner type is mapped (prefer struct mapping for GuildWars types)
        var resolved = ResolveStructOrEnumPointerInner(innerName, typeMap);
        if (resolved != innerName)
            return resolved + "*";
        
        // Check for primitives
        var primitive = MapPrimitiveType(innerName);
        if (primitive != innerName)
            return primitive + "*";
        
        // Special cases
        return innerName switch
        {
            "void" => "nint", // void* -> nint
            _ => "nint", // unmapped pointer -> nint
        };
    }
    
    /// <summary>
    /// Maps C/C++ primitive type names to C# primitive types.
    /// </summary>
    private static string MapPrimitiveType(string cppType)
    {
        return cppType switch
        {
            "uint32_t" => "uint",
            "int32_t" => "int",
            "uint16_t" => "ushort",
            "int16_t" => "short",
            "uint8_t" => "byte",
            "int8_t" => "sbyte",
            "uint64_t" => "ulong",
            "int64_t" => "long",
            "size_t" => "nuint",
            "uintptr_t" => "nuint",
            "intptr_t" => "nint",
            "unsigned int" or "unsigned" => "uint",
            "unsigned short" => "ushort",
            "unsigned long" => "uint",
            "unsigned char" => "byte",
            "char" => "byte",
            "wchar_t" => "char",
            "bool" => "byte",
            // GWCA-specific typedefs 
            "AgentID" => "uint",
            "PlayerNumber" => "uint",
            "ItemID" => "uint",
            "PlayerID" => "uint",
            "Color" => "uint",
            _ => cppType, // pass through already-correct types like int, uint, etc
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
                : CamelCase(ShortName(typeMap.TryGetValue(dt.Name, out var mapped) ? mapped : dt.Name));
        }
        else if (dt.Kind is TypeKind.FuncPtr)
        {
            baseName = "callback";
        }
        else
        {
            var csType = ResolveCsType(dt, typeMap);
            var shortType = ShortName(csType);
            baseName = shortType switch
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
                _ => CamelCase(shortType),
            };
        }

        return total > 1 ? baseName + (idx + 1) : baseName;
    }

    private static string CamelCase(string name)
    {
        if (name.Length == 0) return "arg";
        return char.ToLowerInvariant(name[0]) + name.Substring(1);
    }

    /// <summary>Extracts the short type name from a potentially fully-qualified name.</summary>
    private static string ShortName(string fqn)
    {
        var idx = fqn.LastIndexOf('.');
        return idx >= 0 ? fqn.Substring(idx + 1) : fqn;
    }

    // ════════════════════════════════════════════════════════════════
    // Internal types
    // ════════════════════════════════════════════════════════════════

    /// <summary>Type mapping for the incremental pipeline.  Implements
    /// <see cref="IEquatable{T}"/> for proper caching.</summary>
    internal readonly struct TypeMapping(string gwcaName, string csName, string csNamespace) : IEquatable<TypeMapping>
    {
        public readonly string GwcaName = gwcaName;
        public readonly string CsName = csName;
        public readonly string CsNamespace = csNamespace;

        public bool Equals(TypeMapping other)
            => this.GwcaName == other.GwcaName && this.CsName == other.CsName && this.CsNamespace == other.CsNamespace;

        public override bool Equals(object? obj)
            => obj is TypeMapping other && this.Equals(other);

        public override int GetHashCode()
            => (this.GwcaName?.GetHashCode() ?? 0) * 397
             ^ (this.CsName?.GetHashCode() ?? 0) * 17
             ^ (this.CsNamespace?.GetHashCode() ?? 0);
    }

    /// <summary>A node in the namespace tree, representing a C++ namespace
    /// as a nested static class.</summary>
    internal sealed class NamespaceNode(string name)
    {
        public string Name { get; } = name;
        public Dictionary<string, NamespaceNode> Children { get; } = new(StringComparer.Ordinal);
        public List<ExportEntry> Functions { get; } = [];
    }

    /// <summary>A single mangled export with its demangled info.</summary>
    internal sealed class ExportEntry(string mangledName, DemangledFunction info)
    {
        public string MangledName { get; } = mangledName;
        public DemangledFunction Info { get; } = info;
    }

    // ════════════════════════════════════════════════════════════════
    // Constants tree types (parsed from C++ headers)
    // ════════════════════════════════════════════════════════════════

    internal sealed class ConstantsNode(string name)
    {
        public string Name { get; } = name;
        public Dictionary<string, ConstantsNode> Children { get; } = new(StringComparer.Ordinal);
        public List<CppEnumDef> Enums { get; } = [];
        public List<CppConstField> Constants { get; } = [];
        public List<CppStructDef> Structs { get; } = [];
        public List<CppTypeAlias> TypeAliases { get; } = [];
        public int DebugPopLine { get; set; } // Debug: line number where this namespace was popped

        public ConstantsNode GetOrCreateChild(string childName)
        {
            if (!this.Children.TryGetValue(childName, out var child))
            {
                child = new ConstantsNode(childName);
                this.Children[childName] = child;
            }
            return child;
        }
    }

    internal sealed class CppEnumDef
    {
        public string? Name { get; set; } // null for anonymous enums
        public string? UnderlyingType { get; set; } // e.g. "uint32_t", null → int
        public List<CppEnumMember> Members { get; } = [];
    }

    internal sealed class CppEnumMember
    {
        public string Name { get; set; } = "";
        public string? Value { get; set; }
        public string? Comment { get; set; }
    }

    internal sealed class CppConstField
    {
        public string Name { get; set; } = "";
        public string CppType { get; set; } = "int";
        public string Value { get; set; } = "0";
        public string? Comment { get; set; }
    }

    internal sealed class CppStructDef
    {
        public string Name { get; set; } = "";
        public string? BaseType { get; set; }
        public int? Size { get; set; } // from static_assert(sizeof(...))
        public List<CppStructField> Fields { get; } = [];
        public string DebugNamespacePath { get; set; } = ""; // Debug: namespace path when parsed
    }

    internal sealed class CppStructField
    {
        public string Name { get; set; } = "";
        public string CppType { get; set; } = "int";
        public int? Offset { get; set; }
        public string? ArraySize { get; set; } // e.g., "7", "0x10", "20"
        public string? Comment { get; set; }
    }

    /// <summary>
    /// Represents a C++ typedef alias like: typedef Array&lt;T&gt; AliasName;
    /// </summary>
    internal sealed class CppTypeAlias
    {
        public string AliasName { get; set; } = "";
        public string SourceType { get; set; } = ""; // e.g., "Array"
        public string? TemplateArg { get; set; } // e.g., "Buff"
    }
}