using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Daybreak.Generators;

internal enum NativeTypeKind { Primitive, Pointer, Enum, Struct, FuncPtr }

/// <summary>
/// A C type taken from a GWCA export signature, resolved far enough to map to C#.
/// </summary>
internal record NativeType(
    NativeTypeKind Kind,
    string Name,
    string? QualifiedName = null,
    NativeType? TemplateArg = null,
    string? UnderlyingType = null);

internal record NativeFunction(
    string[] NamespaceParts,
    string FunctionName,
    NativeType ReturnType,
    NativeType[] ParameterTypes,
    string[] ParameterNames,
    bool IsVarArgs)
{
    public string QualifiedCppName => string.Join("::", this.NamespaceParts) + "::" + this.FunctionName;
}

/// <summary>
/// One entry of GWCA's exports.json: the C export name and its declaration.
/// </summary>
internal sealed record NativeExport(string Name, string Signature);

/// <summary>
/// Reads GWCA's exports.json and parses the C declarations it contains.
/// GWCA exports every function as <c>GW_[Namespace_]Function</c> with <c>__cdecl</c>,
/// so the export name gives the namespace and the signature gives the types.
/// </summary>
internal static class NativeSignature
{
    // Value types GWCA passes by value across the C ABI (GW::Runtime::OwnedRecord).
    private static readonly HashSet<string> ByValueRecords = new(StringComparer.Ordinal)
    {
        "Vec2f", "Vec3f", "GamePos", "GHKey", "SkillTemplate", "HookTarget",
    };

    private static readonly Regex EntryRegex = new(
        @"""name""\s*:\s*""(?<name>[^""]+)""\s*,\s*""signature""\s*:\s*""(?<sig>[^""]+)""\s*,\s*""guards""\s*:\s*\[(?<guards>[^\]]*)\]",
        RegexOptions.Compiled);

    private static readonly Regex ValueRegex = new(@"""(?<key>abi|schema|validation_export)""\s*:\s*""(?<value>[^""]+)""", RegexOptions.Compiled);

    private static readonly Regex DeclarationRegex = new(
        @"^(?<ret>.+?)\s*__cdecl\s+(?<name>\w+)\s*\((?<params>.*)\)\s*(noexcept)?\s*$",
        RegexOptions.Compiled);

    private static readonly Regex ParameterRegex = new(
        @"^(?<type>.*?[\s\*&>])(?<name>[A-Za-z_]\w*)\s*(?<array>\[\s*\w*\s*\])?$",
        RegexOptions.Compiled);

    public static IReadOnlyList<NativeExport> ReadExports(string json)
    {
        var exports = new List<NativeExport>();
        foreach (Match match in EntryRegex.Matches(json))
        {
            // Guards are preprocessor conditions. Only "not WASM" guards exist for the Windows DLL.
            var guards = match.Groups["guards"].Value;
            if (guards.Contains("GWCA_WASM") && !guards.Contains("!"))
            {
                continue;
            }

            exports.Add(new NativeExport(match.Groups["name"].Value, match.Groups["sig"].Value));
        }

        return exports;
    }

    public static string? ReadValue(string json, string key)
    {
        foreach (Match match in ValueRegex.Matches(json))
        {
            if (match.Groups["key"].Value == key)
            {
                return match.Groups["value"].Value;
            }
        }

        return null;
    }

    /// <summary>
    /// Splits <c>GW_UI_ButtonClick</c> into <c>[GW, UI]</c> and <c>ButtonClick</c>.
    /// Functions without a namespace segment (<c>GW_GetGameContext</c>) live in <c>GW</c>.
    /// </summary>
    public static (string[] NamespaceParts, string FunctionName)? SplitExportName(string exportName)
    {
        const string prefix = "GW_";
        if (!exportName.StartsWith(prefix, StringComparison.Ordinal))
        {
            return null;
        }

        var rest = exportName.Substring(prefix.Length);
        var separator = rest.IndexOf('_');
        return separator > 0 && separator < rest.Length - 1
            ? (["GW", rest.Substring(0, separator)], rest.Substring(separator + 1))
            : (["GW"], rest);
    }

    /// <summary>
    /// Parses a declaration such as <c>bool __cdecl GW_UI_ButtonClick(Frame* btn_frame) noexcept</c>.
    /// <paramref name="typeMap"/> decides whether a by-value named type is an enum or a struct.
    /// </summary>
    public static NativeFunction? Parse(NativeExport export, Dictionary<string, string> typeMap)
    {
        var split = SplitExportName(export.Name);
        var match = DeclarationRegex.Match(export.Signature.Trim());
        if (split is null || !match.Success)
        {
            return null;
        }

        var returnType = ParseType(match.Groups["ret"].Value, typeMap);
        var types = new List<NativeType>();
        var names = new List<string>();
        var isVarArgs = false;
        foreach (var parameter in SplitParameters(match.Groups["params"].Value))
        {
            var (typeText, name) = SplitParameter(parameter);
            if (StripQualifiers(typeText) is "va_list" or "...")
            {
                isVarArgs = true;
                continue;
            }

            types.Add(ParseType(typeText, typeMap));
            names.Add(name ?? "arg" + (names.Count + 1));
        }

        return new NativeFunction(split.Value.NamespaceParts, split.Value.FunctionName, returnType, [.. types], [.. names], isVarArgs);
    }

    private static IEnumerable<string> SplitParameters(string parameters)
    {
        parameters = parameters.Trim();
        if (parameters.Length == 0 || parameters == "void")
        {
            yield break;
        }

        var depth = 0;
        var start = 0;
        for (var i = 0; i < parameters.Length; i++)
        {
            switch (parameters[i])
            {
                case '<' or '(' or '[':
                    depth++;
                    break;
                case '>' or ')' or ']':
                    depth--;
                    break;
                case ',' when depth == 0:
                    yield return parameters.Substring(start, i - start).Trim();
                    start = i + 1;
                    break;
            }
        }

        yield return parameters.Substring(start).Trim();
    }

    private static (string Type, string? Name) SplitParameter(string parameter)
    {
        var match = ParameterRegex.Match(parameter);
        if (!match.Success || StripQualifiers(match.Groups["type"].Value).Length == 0)
        {
            return (parameter, null);
        }

        // A trailing "[N]" makes the parameter a pointer, as in C.
        var type = match.Groups["type"].Value + (match.Groups["array"].Success ? "*" : string.Empty);
        return (type, match.Groups["name"].Value);
    }

    private static NativeType ParseType(string text, Dictionary<string, string> typeMap)
    {
        var type = StripQualifiers(text);
        var indirection = type.Count(c => c is '*' or '&');
        type = type.Replace("*", string.Empty).Replace("&", string.Empty).Trim();

        NativeType? templateArg = null;
        var templateStart = type.IndexOf('<');
        if (templateStart > 0)
        {
            templateArg = ParseType(type.Substring(templateStart + 1, type.LastIndexOf('>') - templateStart - 1), typeMap);
            type = type.Substring(0, templateStart).Trim();
        }

        var qualifiedName = type;
        var name = type.Substring(type.LastIndexOf(':') + 1);
        if (name is "BaseArray")
        {
            name = "Array";
        }

        if (indirection >= 2)
        {
            // Pointer-to-pointer: the pointee is itself a pointer, so expose it as nint*.
            return new NativeType(NativeTypeKind.Pointer, "nint", qualifiedName);
        }

        if (indirection == 1)
        {
            var pointee = name switch
            {
                "char" or "uint8_t" or "int8_t" => "byte",
                "wchar_t" or "uint16_t" => "ushort",
                "int16_t" => "short",
                "int" or "int32_t" => "int",
                "uint32_t" or "unsigned" or "DWORD" => "uint",
                "size_t" or "uintptr_t" => "nuint",
                _ => name,
            };
            return new NativeType(NativeTypeKind.Pointer, pointee, qualifiedName, templateArg);
        }

        var primitive = MapPrimitive(name);
        if (primitive is not null)
        {
            return new NativeType(NativeTypeKind.Primitive, primitive, qualifiedName);
        }

        if (name.EndsWith("Callback", StringComparison.Ordinal)
            || name.EndsWith("Handler", StringComparison.Ordinal)
            || name.EndsWith("_pt", StringComparison.Ordinal))
        {
            return new NativeType(NativeTypeKind.FuncPtr, name, qualifiedName);
        }

        if (templateArg is not null || ByValueRecords.Contains(name)
            || (typeMap.TryGetValue(name, out var mapped) && mapped.Contains("GuildWars.")))
        {
            return new NativeType(NativeTypeKind.Struct, name, qualifiedName, templateArg);
        }

        // Unknown by-value names are enums; GWCA enums default to a 32-bit underlying type.
        return new NativeType(NativeTypeKind.Enum, name, qualifiedName, UnderlyingType: "uint");
    }

    private static string? MapPrimitive(string name) => name switch
    {
        "void" => "void",
        "bool" => "bool",
        "float" => "float",
        "double" => "double",
        "char" or "uint8_t" or "unsigned char" => "byte",
        "int8_t" => "sbyte",
        "wchar_t" or "uint16_t" or "unsigned short" => "ushort",
        "int16_t" or "short" => "short",
        "int" or "int32_t" or "long" => "int",
        "uint32_t" or "unsigned" or "unsigned int" or "unsigned long" or "DWORD"
            or "AgentID" or "PlayerNumber" or "PlayerID" or "ItemID" or "Color" => "uint",
        "int64_t" => "long",
        "uint64_t" => "ulong",
        "size_t" or "uintptr_t" => "nuint",
        "intptr_t" or "HWND" or "HMODULE" or "HANDLE" => "nint",
        _ => null,
    };

    private static string StripQualifiers(string type)
    {
        foreach (var keyword in new[] { "const", "volatile", "struct", "enum", "class" })
        {
            type = Regex.Replace(type, $@"\b{keyword}\b", string.Empty);
        }

        return Regex.Replace(type, @"\s+", " ").Trim();
    }
}
