using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Sybil;
using static Daybreak.Generators.MsvcDemangler;
using TypeKind = Daybreak.Generators.MsvcDemangler.TypeKind;

namespace Daybreak.Generators;

/// <summary>
/// Incremental source generator that reads PE exports from gwca.dll,
/// demangles MSVC-decorated C++ names to recover type and namespace
/// information, and emits a <c>GWCA</c> class with nested static classes
/// mirroring the C++ namespace hierarchy.
///
/// For example, <c>GW::Agents::ChangeTarget(uint)</c> becomes
/// <c>GWCA.GW.Agents.ChangeTarget(uint)</c>.
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

        // Build type map (gwcaName → fully qualified C# name)
        var typeMap = new Dictionary<string, string>(BuiltInTypeMappings);
        foreach (var m in mappings)
        {
            var fqn = string.IsNullOrEmpty(m.CsNamespace)
                ? m.CsName
                : "global::" + m.CsNamespace + "." + m.CsName;
            typeMap[m.GwcaName] = fqn;
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

        // Emit
        var sb = new StringBuilder(65536);
        EmitHeader(sb, totalExports, skippedExports);

        // Emit the tree recursively (starting from GWCA's children)
        foreach (var child in root.Children.Values.OrderBy(c => c.Name, StringComparer.Ordinal))
        {
            EmitNamespaceNode(sb, child, typeMap, 4);
        }

        sb.AppendLine("}");

        context.AddSource("GWCA.g.cs", sb.ToString());
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
        sb.AppendLine();
        sb.AppendLine("namespace Daybreak.Generators;");
        sb.AppendLine();
        sb.AppendLine("/// <summary>");
        sb.AppendLine($"/// P/Invoke bindings for {exportCount} C++ exports from gwca.dll ({skippedCount} skipped).");
        sb.AppendLine("/// Nested classes mirror the C++ namespace hierarchy (e.g. GW::Agents → GWCA.GW.Agents).");
        sb.AppendLine("/// Types annotated with [GWCAEquivalent] are used in signatures where available.");
        sb.AppendLine("/// </summary>");
        sb.AppendLine("internal static unsafe class GWCA");
        sb.AppendLine("{");
        sb.AppendLine("    private const string DllName = \"gwca.dll\";");
    }

    // ── Recursive namespace/class emission ─────────────────────────

    private static void EmitNamespaceNode(
        StringBuilder sb,
        NamespaceNode node,
        Dictionary<string, string> typeMap,
        int indent)
    {
        var pad = new string(' ', indent);

        sb.AppendLine();
        sb.AppendLine($"{pad}internal static class {SanitizeIdentifier(node.Name)}");
        sb.AppendLine($"{pad}{{");

        // Emit functions in this namespace, disambiguating overloads that
        // would produce identical C# signatures
        var signatureCounts = new Dictionary<string, int>(StringComparer.Ordinal);
        foreach (var entry in node.Functions.OrderBy(e => e.Info.FunctionName, StringComparer.Ordinal))
        {
            EmitFunction(sb, entry, typeMap, indent + 4, signatureCounts);
        }

        // Emit child namespaces
        foreach (var child in node.Children.Values.OrderBy(c => c.Name, StringComparer.Ordinal))
        {
            EmitNamespaceNode(sb, child, typeMap, indent + 4);
        }

        sb.AppendLine($"{pad}}}");
    }

    // ── Single function emission ───────────────────────────────────

    private static void EmitFunction(
        StringBuilder sb,
        ExportEntry entry,
        Dictionary<string, string> typeMap,
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

        // Calling convention: ThisCall for member functions, Cdecl for free/static
        var convention = info.IsMember
            ? ", CallingConvention = CallingConvention.ThisCall"
            : "";

        // Comment lines
        sb.AppendLine();
        var commentParts = new List<string> { info.QualifiedCppName };
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

        var prefix = hasUnmapped ? "// " : "";
        sb.AppendLine($"{pad}// {string.Join(" | ", commentParts)}");
        sb.AppendLine($"{pad}{prefix}[DllImport(DllName, EntryPoint = \"{entry.MangledName}\"{convention})]");
        sb.AppendLine($"{pad}{prefix}{retAttr}internal static extern {csRet} {methodName}({paramStr});");
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
        if (CsKeywords.Contains(name))
            return "@" + name;
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
                    return "global::Daybreak.API.Interop.GuildWars.GuildWarsArray<" + innerType + ">";
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
            TypeKind.Pointer => "nint", // pointer types cannot be generic type arguments
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
    private sealed class NamespaceNode(string name)
    {
        public string Name { get; } = name;
        public Dictionary<string, NamespaceNode> Children { get; } = new(StringComparer.Ordinal);
        public List<ExportEntry> Functions { get; } = [];
    }

    /// <summary>A single mangled export with its demangled info.</summary>
    private sealed class ExportEntry(string mangledName, DemangledFunction info)
    {
        public string MangledName { get; } = mangledName;
        public DemangledFunction Info { get; } = info;
    }
}

