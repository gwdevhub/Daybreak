using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using static Daybreak.Generators.GenerateGWCABindings;

namespace Daybreak.Generators;

internal static class CppHeaderParser
{
    // ════════════════════════════════════════════════════════════════
    // Regex patterns for enum parsing
    // ════════════════════════════════════════════════════════════════

    // Matches: enum class Name : Type {
    // Matches: enum class Name {
    // Matches: enum Name : Type {
    // Matches: enum {
    private static readonly Regex EnumStartRegex = new(
        @"^\s*enum\s+(?:class\s+)?(?:(\w+)\s*)?(?::\s*(\w+)\s*)?\{",
        RegexOptions.Compiled);

    // Matches: enum class Name : Type   (brace on next line)
    // Matches: enum class Name          (brace on next line)
    // Does NOT match forward declarations ending with ;
    private static readonly Regex EnumNobraceRegex = new(
        @"^\s*enum\s+(?:class\s+)?(?:(\w+)\s*)?(?::\s*(\w+)\s*)?$",
        RegexOptions.Compiled);

    // ════════════════════════════════════════════════════════════════
    // Regex patterns for struct parsing
    // ════════════════════════════════════════════════════════════════

    // Matches: struct Name { or struct Name : public Base { or struct Name : Base {
    // Captures: Name, optional BaseType
    private static readonly Regex StructStartRegex = new(
        @"^\s*struct\s+(\w+)\s*(?::\s*(?:public\s+)?(\w+(?:::\w+)*)\s*)?\{",
        RegexOptions.Compiled);

    // Matches: struct Name : public Base   (brace on next line)
    private static readonly Regex StructNobraceRegex = new(
        @"^\s*struct\s+(\w+)\s*(?::\s*(?:public\s+)?(\w+(?:::\w+)*)\s*)?$",
        RegexOptions.Compiled);

    // Matches field with offset comment: /* +h0024 */ Type field;
    private static readonly Regex FieldWithOffsetRegex = new(
        @"^\s*/\*\s*\+h([0-9A-Fa-f]{4})\s*\*/\s*(.+?)\s*;\s*(?://\s*(.*))?$",
        RegexOptions.Compiled);

    // Matches field without offset comment: Type field;
    private static readonly Regex FieldNoOffsetRegex = new(
        @"^\s*(?!(?:inline|static|virtual|GWCA_API|typedef|using|enum|struct|union|return|if|for|while|switch|\[\[|\}))([A-Za-z_][\w:*&<>\s,]+?)\s+(\w+)(?:\s*\[\s*([^\]]+)\s*\])?\s*(?:=\s*[^;]+)?\s*;\s*(?://\s*(.*))?$",
        RegexOptions.Compiled);

    // Matches static_assert(sizeof(Name) == 0xHEX or decimal)
    private static readonly Regex SizeofAssertRegex = new(
        @"static_assert\s*\(\s*sizeof\s*\(\s*(\w+)\s*\)\s*==\s*(0x[0-9A-Fa-f]+|\d+)",
        RegexOptions.Compiled);

    // ════════════════════════════════════════════════════════════════
    // Other patterns
    // ════════════════════════════════════════════════════════════════

    // Matches: constexpr Type Name = Value; // comment
    private static readonly Regex ConstexprRegex = new(
        @"^\s*constexpr\s+(\w+)\s+(\w+)\s*=\s*(.+?)\s*;\s*(?://\s*(.*))?$",
        RegexOptions.Compiled);

    // Matches: namespace Name {
    // Also matches: namespace Name1::Name2 { (but we split on ::)
    private static readonly Regex NamespaceRegex = new(
        @"^\s*namespace\s+([\w:]+)\s*\{",
        RegexOptions.Compiled);

    // Matches closing brace with optional semicolon
    private static readonly Regex CloseBraceRegex = new(
        @"^\s*\}\s*;?\s*$",
        RegexOptions.Compiled);

    // Matches: typedef ... (to skip)
    private static readonly Regex TypedefRegex = new(
        @"^\s*typedef\s+",
        RegexOptions.Compiled);

    // Matches lines to skip inside structs
    private static readonly Regex SkipLineRegex = new(
        @"^\s*(inline|static\s+const|GWCA_API|virtual|explicit|friend|\[\[nodiscard\]\]|constexpr|template\s*<|//|#|public:|private:|protected:|$)",
        RegexOptions.Compiled);

    // Matches: union { (to skip union blocks)
    private static readonly Regex UnionStartRegex = new(
        @"^\s*union\s*\{",
        RegexOptions.Compiled);

    // Matches: template<...> struct/class (to skip)
    private static readonly Regex TemplateStructRegex = new(
        @"^\s*template\s*<",
        RegexOptions.Compiled);

    // Matches inline function definitions (static, inline, GWCA_API, etc.) that have a body
    private static readonly Regex FunctionWithBodyRegex = new(
        @"^\s*(?:static|inline|GWCA_API|virtual|explicit|constexpr).*\([^)]*\)\s*(?:const)?\s*\{",
        RegexOptions.Compiled);

    // Matches function declarations with body on next line (no opening brace)
    private static readonly Regex FunctionNoBraceRegex = new(
        @"^\s*(?:static|inline|GWCA_API|virtual|explicit|constexpr).*\([^)]*\)\s*(?:const)?\s*$",
        RegexOptions.Compiled);

    // Matches standalone opening brace
    private static readonly Regex OpenBraceOnlyRegex = new(
        @"^\s*\{\s*$",
        RegexOptions.Compiled);

    public static void Parse(string headerText, ConstantsNode root)
    {
        var lines = headerText.Split('\n');
        var namespaceStack = new Stack<ConstantsNode>();
        namespaceStack.Push(root);

        // Parsing state
        bool inEnum = false;
        CppEnumDef? currentEnum = null;
        bool inStruct = false;
        CppStructDef? currentStruct = null;
        int structBraceDepth = 0;
        bool inSkipBlock = false;
        int skipBlockBraceCount = 0;
        bool inMultiLineConstexpr = false;
        int multiLineBraceCount = 0;
        int freeBraceDepth = 0;  // Track braces from functions with body on separate line

        // Track struct sizes from static_assert
        var structSizes = new Dictionary<string, int>(StringComparer.Ordinal);

        // First pass: collect sizeof assertions
        foreach (var line in lines)
        {
            var sizeMatch = SizeofAssertRegex.Match(line);
            if (sizeMatch.Success)
            {
                var name = sizeMatch.Groups[1].Value;
                var sizeStr = sizeMatch.Groups[2].Value;
                int size = sizeStr.StartsWith("0x", StringComparison.OrdinalIgnoreCase)
                    ? Convert.ToInt32(sizeStr, 16)
                    : int.Parse(sizeStr);
                structSizes[name] = size;
            }
        }

        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i].TrimEnd('\r');

            // Skip preprocessor directives
            if (line.TrimStart().StartsWith("#"))
                continue;

            var trimmed = line.TrimStart();
            if (trimmed.Length == 0)
                continue;
            if (trimmed.StartsWith("//") && !inEnum && !inStruct)
                continue;

            // Handle multi-line constexpr (like constexpr std::array)
            if (inMultiLineConstexpr)
            {
                multiLineBraceCount += CountChar(line, '{') - CountChar(line, '}');
                if (multiLineBraceCount <= 0 || line.TrimEnd().EndsWith(";"))
                {
                    inMultiLineConstexpr = false;
                }
                continue;
            }

            // Handle skip blocks (unions, template structs, etc.)
            if (inSkipBlock)
            {
                skipBlockBraceCount += CountChar(line, '{') - CountChar(line, '}');
                if (skipBlockBraceCount <= 0)
                {
                    inSkipBlock = false;
                }
                continue;
            }

            // ── Inside an enum body (multi-line) ───────────────────
            if (inEnum && currentEnum is not null)
            {
                if (trimmed.StartsWith("//"))
                    continue;

                var closeBraceIdx = trimmed.IndexOf('}');
                if (closeBraceIdx >= 0)
                {
                    if (closeBraceIdx > 0)
                    {
                        ParseEnumMembersFromLine(trimmed.Substring(0, closeBraceIdx), currentEnum);
                    }
                    inEnum = false;
                    var node = namespaceStack.Peek();
                    node.Enums.Add(currentEnum);
                    currentEnum = null;
                    continue;
                }

                ParseEnumMembersFromLine(trimmed, currentEnum);
                continue;
            }

            // ── Inside a struct body ───────────────────────────────
            if (inStruct && currentStruct is not null)
            {
                int openBraces = CountChar(line, '{');
                int closeBraces = CountChar(line, '}');
                structBraceDepth += openBraces - closeBraces;

                // If we hit the closing brace of the struct
                if (structBraceDepth <= 0)
                {
                    // Apply known size if available
                    if (structSizes.TryGetValue(currentStruct.Name, out var size))
                        currentStruct.Size = size;

                    var node = namespaceStack.Peek();
                    // Debug: capture namespace path
                    var pathParts = new List<string>();
                    foreach (var n in namespaceStack)
                        pathParts.Add(n.Name);
                    pathParts.Reverse();
                    currentStruct.DebugNamespacePath = string.Join(".", pathParts);
                    
                    node.Structs.Add(currentStruct);
                    currentStruct = null;
                    inStruct = false;
                    continue;
                }

                // Skip nested blocks (unions, nested structs inside the struct)
                if (structBraceDepth > 1)
                    continue;

                // Skip comment-only lines
                if (trimmed.StartsWith("//"))
                    continue;

                // Skip method/static/inline lines
                if (SkipLineRegex.IsMatch(trimmed))
                    continue;

                // Skip lines with function bodies or method declarations
                if (trimmed.Contains("(") && (trimmed.Contains(")") || trimmed.Contains("{")))
                    continue;

                // Skip bitfield declarations (e.g., DyeColor dye1 : 4;)
                if (trimmed.Contains(" : ") && Regex.IsMatch(trimmed, @":\s*\d+\s*;"))
                    continue;

                // Skip union starts
                if (UnionStartRegex.IsMatch(trimmed))
                    continue;

                // Try to parse field with offset
                var fieldOffsetMatch = FieldWithOffsetRegex.Match(trimmed);
                if (fieldOffsetMatch.Success)
                {
                    var offset = Convert.ToInt32(fieldOffsetMatch.Groups[1].Value, 16);
                    var typeAndName = fieldOffsetMatch.Groups[2].Value;
                    var comment = fieldOffsetMatch.Groups[3].Success ? fieldOffsetMatch.Groups[3].Value.Trim() : null;

                    var field = ParseFieldTypeAndName(typeAndName, offset, comment);
                    if (field is not null)
                        currentStruct.Fields.Add(field);
                    continue;
                }

                // Try to parse field without offset
                var fieldNoOffsetMatch = FieldNoOffsetRegex.Match(trimmed);
                if (fieldNoOffsetMatch.Success)
                {
                    var cppType = fieldNoOffsetMatch.Groups[1].Value.Trim();
                    var name = fieldNoOffsetMatch.Groups[2].Value;
                    var arraySize = fieldNoOffsetMatch.Groups[3].Success ? fieldNoOffsetMatch.Groups[3].Value.Trim() : null;
                    var comment = fieldNoOffsetMatch.Groups[4].Success ? fieldNoOffsetMatch.Groups[4].Value.Trim() : null;

                    var field = new CppStructField
                    {
                        Name = name,
                        CppType = cppType,
                        ArraySize = arraySize,
                        Comment = comment,
                    };
                    currentStruct.Fields.Add(field);
                }
                continue;
            }

            // ── Try to match namespace ─────────────────────────────
            var nsMatch = NamespaceRegex.Match(trimmed);
            if (nsMatch.Success)
            {
                var nsName = nsMatch.Groups[1].Value;
                var parts = nsName.Split(new[] { "::" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var part in parts)
                {
                    var current = namespaceStack.Peek();
                    var child = current.GetOrCreateChild(part);
                    namespaceStack.Push(child);
                }
                continue;
            }

            // ── Closing brace (function body end or namespace end) ─
            if (CloseBraceRegex.IsMatch(trimmed))
            {
                // If we have unclosed function bodies, close those first
                if (freeBraceDepth > 0)
                {
                    freeBraceDepth--;
                    continue;
                }
                
                if (namespaceStack.Count > 1)
                {
                    // Debug: Track where namespace was popped
                    var poppedNode = namespaceStack.Pop();
                    poppedNode.DebugPopLine = i + 1;
                }
                continue;
            }

            // ── Skip static_assert ─────────────────────────────────
            if (trimmed.StartsWith("static_assert"))
            {
                if (!trimmed.Contains(";"))
                {
                    for (int j = i + 1; j < lines.Length; j++)
                    {
                        if (lines[j].Contains(";"))
                        {
                            i = j;
                            break;
                        }
                    }
                }
                continue;
            }

            // ── Handle typedef struct Name { (parse as regular struct) ─
            var typedefStructMatch = Regex.Match(trimmed, @"^\s*typedef\s+struct\s+(\w+)\s*\{");
            if (typedefStructMatch.Success)
            {
                var structName = typedefStructMatch.Groups[1].Value;
                currentStruct = new CppStructDef
                {
                    Name = structName,
                };
                inStruct = true;
                structBraceDepth = 1;
                continue;
            }

            // ── Skip other typedef ─────────────────────────────────
            if (TypedefRegex.IsMatch(trimmed))
                continue;

            // ── Skip template struct/class ─────────────────────────
            if (TemplateStructRegex.IsMatch(trimmed))
            {
                int braces = CountChar(trimmed, '{') - CountChar(trimmed, '}');
                if (braces > 0)
                {
                    inSkipBlock = true;
                    skipBlockBraceCount = braces;
                }
                continue;
            }

            // ── Skip inline function bodies ────────────────────────
            if (FunctionWithBodyRegex.IsMatch(trimmed))
            {
                int braces = CountChar(trimmed, '{') - CountChar(trimmed, '}');
                if (braces > 0)
                {
                    inSkipBlock = true;
                    skipBlockBraceCount = braces;
                }
                continue;
            }

            // ── Track function declarations with body on separate line ─
            if (FunctionNoBraceRegex.IsMatch(trimmed))
            {
                // The next meaningful line should be { which starts the body
                // We'll handle this by looking for standalone { and tracking brace depth
                continue;
            }

            // ── Track standalone opening braces (function bodies with { on separate line)
            if (OpenBraceOnlyRegex.IsMatch(trimmed))
            {
                freeBraceDepth++;
                continue;
            }

            // ── Skip constexpr std::array etc. ─────────────────────
            if (trimmed.StartsWith("constexpr std::"))
            {
                int braces = CountChar(trimmed, '{') - CountChar(trimmed, '}');
                if (braces > 0)
                {
                    inMultiLineConstexpr = true;
                    multiLineBraceCount = braces;
                }
                continue;
            }

            // ── Try to match struct start ──────────────────────────
            var structMatch = StructStartRegex.Match(trimmed);
            if (structMatch.Success)
            {
                var structName = structMatch.Groups[1].Value;
                var baseType = structMatch.Groups[2].Success ? structMatch.Groups[2].Value : null;

                // Skip some special cases
                if (structName.StartsWith("__") || structName == "Packet")
                {
                    int braces = CountChar(trimmed, '{') - CountChar(trimmed, '}');
                    if (braces > 0)
                    {
                        inSkipBlock = true;
                        skipBlockBraceCount = braces;
                    }
                    continue;
                }

                currentStruct = new CppStructDef
                {
                    Name = structName,
                    BaseType = baseType,
                };
                inStruct = true;
                structBraceDepth = 1;
                continue;
            }

            // ── Struct without opening brace on same line ──────────
            var structNobraceMatch = StructNobraceRegex.Match(trimmed);
            if (structNobraceMatch.Success)
            {
                var structName = structNobraceMatch.Groups[1].Value;
                
                // Skip forward declarations (just struct Name;)
                if (trimmed.EndsWith(";"))
                    continue;

                // Skip special cases
                if (structName.StartsWith("__") || structName == "Packet")
                    continue;

                // Look ahead for opening brace
                for (int j = i + 1; j < lines.Length; j++)
                {
                    var nextTrimmed = lines[j].TrimStart().TrimEnd('\r');
                    if (nextTrimmed.Length == 0 || nextTrimmed.StartsWith("//"))
                        continue;

                    if (nextTrimmed.Contains("{"))
                    {
                        var baseType = structNobraceMatch.Groups[2].Success ? structNobraceMatch.Groups[2].Value : null;
                        currentStruct = new CppStructDef
                        {
                            Name = structName,
                            BaseType = baseType,
                        };
                        inStruct = true;
                        structBraceDepth = 1;
                        i = j;
                        break;
                    }
                    break; // unexpected content
                }
                continue;
            }

            // ── Try to match enum start (with opening brace on same line) ──
            var enumMatch = EnumStartRegex.Match(trimmed);
            if (enumMatch.Success)
            {
                var enumDef = new CppEnumDef
                {
                    Name = enumMatch.Groups[1].Success && enumMatch.Groups[1].Value.Length > 0
                        ? enumMatch.Groups[1].Value : null,
                    UnderlyingType = enumMatch.Groups[2].Success && enumMatch.Groups[2].Value.Length > 0
                        ? enumMatch.Groups[2].Value : null,
                };

                var afterBrace = trimmed.Substring(enumMatch.Length);
                var closeBraceIdx = afterBrace.IndexOf('}');

                if (closeBraceIdx >= 0)
                {
                    var body = afterBrace.Substring(0, closeBraceIdx);
                    ParseEnumMembersFromLine(body, enumDef);
                    var node = namespaceStack.Peek();
                    node.Enums.Add(enumDef);
                }
                else
                {
                    var rest = afterBrace.Trim();
                    if (rest.Length > 0)
                    {
                        ParseEnumMembersFromLine(rest, enumDef);
                    }
                    currentEnum = enumDef;
                    inEnum = true;
                }
                continue;
            }

            // ── Enum without opening brace on same line ────────────
            var enumNobraceMatch = EnumNobraceRegex.Match(trimmed);
            if (enumNobraceMatch.Success)
            {
                for (int j = i + 1; j < lines.Length; j++)
                {
                    var nextTrimmed = lines[j].TrimStart().TrimEnd('\r');
                    if (nextTrimmed.Length == 0 || nextTrimmed.StartsWith("//"))
                        continue;

                    if (nextTrimmed.Contains("{"))
                    {
                        var enumDef = new CppEnumDef
                        {
                            Name = enumNobraceMatch.Groups[1].Success && enumNobraceMatch.Groups[1].Value.Length > 0
                                ? enumNobraceMatch.Groups[1].Value : null,
                            UnderlyingType = enumNobraceMatch.Groups[2].Success && enumNobraceMatch.Groups[2].Value.Length > 0
                                ? enumNobraceMatch.Groups[2].Value : null,
                        };

                        var braceIdx = nextTrimmed.IndexOf('{');
                        var afterBrace = nextTrimmed.Substring(braceIdx + 1);
                        var closeBraceIdx = afterBrace.IndexOf('}');

                        if (closeBraceIdx >= 0)
                        {
                            var body = afterBrace.Substring(0, closeBraceIdx);
                            ParseEnumMembersFromLine(body, enumDef);
                            var node = namespaceStack.Peek();
                            node.Enums.Add(enumDef);
                        }
                        else
                        {
                            var rest = afterBrace.Trim();
                            if (rest.Length > 0)
                            {
                                ParseEnumMembersFromLine(rest, enumDef);
                            }
                            currentEnum = enumDef;
                            inEnum = true;
                        }
                        i = j;
                        break;
                    }
                    break; // unexpected content
                }
                continue;
            }

            // ── Try to match constexpr field ───────────────────────
            var constMatch = ConstexprRegex.Match(trimmed);
            if (constMatch.Success)
            {
                var cppType = constMatch.Groups[1].Value;
                var name = constMatch.Groups[2].Value;
                var value = constMatch.Groups[3].Value.Trim();
                var comment = constMatch.Groups[4].Success ? constMatch.Groups[4].Value.Trim() : null;

                // Skip complex expressions with :: references or casts
                if (value.Contains("::") || value.Contains("(") || value.Contains("static_cast"))
                    continue;

                var field = new CppConstField
                {
                    Name = name,
                    CppType = cppType,
                    Value = NormalizeCppValue(value),
                    Comment = comment,
                };
                var node = namespaceStack.Peek();
                node.Constants.Add(field);
                continue;
            }
        }
    }

    /// <summary>
    /// Parses a combined "Type Name" or "Type Name[Size]" string into a field.
    /// </summary>
    private static CppStructField? ParseFieldTypeAndName(string typeAndName, int offset, string? comment)
    {
        typeAndName = typeAndName.Trim();

        // Handle array notation: uint32_t name[7]
        string? arraySize = null;
        var bracketIdx = typeAndName.IndexOf('[');
        if (bracketIdx >= 0)
        {
            var closeBracket = typeAndName.IndexOf(']', bracketIdx);
            if (closeBracket > bracketIdx)
            {
                arraySize = typeAndName.Substring(bracketIdx + 1, closeBracket - bracketIdx - 1).Trim();
                typeAndName = typeAndName.Substring(0, bracketIdx).Trim();
            }
        }

        // Split type and name - name is the last token
        var tokens = typeAndName.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        if (tokens.Length < 2)
            return null;

        var name = tokens[tokens.Length - 1].TrimStart('*');
        var cppType = string.Join(" ", tokens.Take(tokens.Length - 1));

        // Handle pointer attached to name: Item *weapon -> Item*, weapon
        if (tokens[tokens.Length - 1].StartsWith("*"))
        {
            cppType += "*";
        }
        // Handle pointer attached to type: Item* weapon
        else if (cppType.EndsWith("*"))
        {
            // Already correct
        }
        // Handle pointer in middle: Item * weapon
        else if (tokens.Any(t => t == "*"))
        {
            cppType = cppType.Replace(" *", "*").Replace("* ", "*");
            if (!cppType.EndsWith("*"))
                cppType += "*";
        }

        return new CppStructField
        {
            Name = name,
            CppType = cppType.Trim(),
            Offset = offset,
            ArraySize = arraySize,
            Comment = comment,
        };
    }

    /// <summary>
    /// Parses comma-separated enum members from a single line of text.
    /// </summary>
    private static void ParseEnumMembersFromLine(string line, CppEnumDef enumDef)
    {
        string? lineComment = null;
        var commentIdx = line.IndexOf("//");
        if (commentIdx >= 0)
        {
            lineComment = line.Substring(commentIdx + 2).Trim();
            line = line.Substring(0, commentIdx);
        }

        var parts = line.Split(',');
        CppEnumMember? lastMember = null;

        foreach (var part in parts)
        {
            var trimPart = part.Trim();
            if (trimPart.Length == 0)
                continue;

            var eqIdx = trimPart.IndexOf('=');
            string name;
            string? value = null;

            if (eqIdx >= 0)
            {
                name = trimPart.Substring(0, eqIdx).Trim();
                value = trimPart.Substring(eqIdx + 1).Trim();
            }
            else
            {
                name = trimPart;
            }

            if (name.Length > 0 && (char.IsLetter(name[0]) || name[0] == '_'))
            {
                var member = new CppEnumMember
                {
                    Name = name,
                    Value = value,
                };
                enumDef.Members.Add(member);
                lastMember = member;
            }
        }

        if (lastMember is not null && lineComment is not null)
        {
            lastMember.Comment = lineComment;
        }
    }

    private static int CountChar(string s, char c)
    {
        int count = 0;
        foreach (var ch in s)
            if (ch == c) count++;
        return count;
    }

    /// <summary>
    /// Normalizes a C++ value literal to a C# compatible one.
    /// </summary>
    private static string NormalizeCppValue(string value)
    {
        value = value.Trim().TrimEnd(',');

        // Remove 'u' or 'U' suffix from integer literals
        if (value.EndsWith("u", StringComparison.OrdinalIgnoreCase) &&
            !value.EndsWith("0xu", StringComparison.OrdinalIgnoreCase))
        {
            var candidate = value.Substring(0, value.Length - 1);
            if (IsIntegerLiteral(candidate))
                return candidate;
        }

        // Normalize C++ float suffix to valid C# float literal
        if (value.EndsWith("f", StringComparison.OrdinalIgnoreCase))
        {
            value = value.Substring(0, value.Length - 1);
            if (value.EndsWith("."))
                value += "0";
            return value + "f";
        }

        // If it's a float-looking literal without suffix, add f
        if (value.Contains(".") && !value.Contains("\""))
            return value + "f";

        return value;
    }

    private static bool IsIntegerLiteral(string s)
    {
        s = s.Trim();
        if (s.Length == 0) return false;
        if (s.StartsWith("0x", StringComparison.OrdinalIgnoreCase))
            return s.Length > 2 && s.Substring(2).All(c => "0123456789abcdefABCDEF".Contains(c));
        var start = s[0] == '-' ? 1 : 0;
        if (start >= s.Length) return false;
        return s.Substring(start).All(char.IsDigit);
    }
}
