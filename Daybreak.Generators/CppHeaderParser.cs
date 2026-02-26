using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using static Daybreak.Generators.GenerateGWCABindings;

namespace Daybreak.Generators;

internal static class CppHeaderParser
{
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

    // Matches: constexpr Type Name = Value; // comment
    private static readonly Regex ConstexprRegex = new(
        @"^\s*constexpr\s+(\w+)\s+(\w+)\s*=\s*(.+?)\s*;\s*(?://\s*(.*))?$",
        RegexOptions.Compiled);

    // Matches: namespace Name {
    // Also matches: namespace Name1::Name2 { (but we split on ::)
    private static readonly Regex NamespaceRegex = new(
        @"^\s*namespace\s+([\w:]+)\s*\{",
        RegexOptions.Compiled);

    // Matches: static ... function declarations (to skip)
    private static readonly Regex StaticFuncRegex = new(
        @"^\s*static\s+",
        RegexOptions.Compiled);

    // Matches: constexpr std::array ... (to skip)
    private static readonly Regex ConstexprArrayRegex = new(
        @"^\s*constexpr\s+std::",
        RegexOptions.Compiled);

    // Matches: static_assert (to skip)
    private static readonly Regex StaticAssertRegex = new(
        @"^\s*static_assert\s*\(",
        RegexOptions.Compiled);

    // Matches: struct ... { (to skip)
    private static readonly Regex StructStartRegex = new(
        @"^\s*struct\s+\w+",
        RegexOptions.Compiled);

    // Matches closing brace with optional semicolon
    private static readonly Regex CloseBraceRegex = new(
        @"^\s*\}\s*;?\s*$",
        RegexOptions.Compiled);

    // Matches: inline ... (operator overloads etc, to skip)
    private static readonly Regex InlineRegex = new(
        @"^\s*inline\s+",
        RegexOptions.Compiled);

    // Matches: typedef ... (to skip)
    private static readonly Regex TypedefRegex = new(
        @"^\s*typedef\s+",
        RegexOptions.Compiled);

    public static void Parse(string headerText, ConstantsNode root)
    {
        var lines = headerText.Split('\n');
        var namespaceStack = new Stack<ConstantsNode>();
        namespaceStack.Push(root);

        bool inEnum = false;
        CppEnumDef? currentEnum = null;
        bool inSkipBlock = false;
        int skipBlockBraceCount = 0;
        bool inMultiLineConstexpr = false;
        int multiLineBraceCount = 0;

        for (int i = 0; i < lines.Length; i++)
        {
            var line = lines[i].TrimEnd('\r');

            // Skip preprocessor directives
            if (line.TrimStart().StartsWith("#"))
                continue;

            var trimmed = line.TrimStart();
            if (trimmed.Length == 0)
                continue;
            if (trimmed.StartsWith("//") && !inEnum)
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

            // Handle skip blocks (static functions, structs, etc.)
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
                // Skip comment-only lines inside enum
                if (trimmed.StartsWith("//"))
                    continue;

                // Check if this line contains the closing brace
                var closeBraceIdx = trimmed.IndexOf('}');
                if (closeBraceIdx >= 0)
                {
                    // Parse any members before the closing brace
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

                // Parse comma-separated members from this line
                ParseEnumMembersFromLine(trimmed, currentEnum);
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

            // ── Closing brace (namespace end) ──────────────────────
            if (CloseBraceRegex.IsMatch(trimmed))
            {
                if (namespaceStack.Count > 1)
                {
                    namespaceStack.Pop();
                }
                continue;
            }

            // ── Skip static_assert ─────────────────────────────────
            if (StaticAssertRegex.IsMatch(trimmed))
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

            // ── Skip static function declarations/definitions ──────
            if (StaticFuncRegex.IsMatch(trimmed))
            {
                int braces = CountChar(trimmed, '{') - CountChar(trimmed, '}');
                if (braces > 0)
                {
                    inSkipBlock = true;
                    skipBlockBraceCount = braces;
                }
                continue;
            }

            // ── Skip inline operators ──────────────────────────────
            if (InlineRegex.IsMatch(trimmed))
            {
                int braces = CountChar(trimmed, '{') - CountChar(trimmed, '}');
                if (braces > 0)
                {
                    inSkipBlock = true;
                    skipBlockBraceCount = braces;
                }
                continue;
            }

            // ── Skip typedef ───────────────────────────────────────
            if (TypedefRegex.IsMatch(trimmed))
                continue;

            // ── Skip constexpr std::array etc. ─────────────────────
            if (ConstexprArrayRegex.IsMatch(trimmed))
            {
                int braces = CountChar(trimmed, '{') - CountChar(trimmed, '}');
                if (braces > 0)
                {
                    inMultiLineConstexpr = true;
                    multiLineBraceCount = braces;
                }
                continue;
            }

            // ── Skip struct definitions ────────────────────────────
            if (StructStartRegex.IsMatch(trimmed) && trimmed.Contains("{"))
            {
                int braces = CountChar(trimmed, '{') - CountChar(trimmed, '}');
                if (braces > 0)
                {
                    inSkipBlock = true;
                    skipBlockBraceCount = braces;
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

                // Get the rest of the line after the opening brace
                var afterBrace = trimmed.Substring(enumMatch.Length);
                var closeBraceIdx = afterBrace.IndexOf('}');

                if (closeBraceIdx >= 0)
                {
                    // Single-line enum: body and closing brace on same line
                    var body = afterBrace.Substring(0, closeBraceIdx);
                    ParseEnumMembersFromLine(body, enumDef);
                    var node = namespaceStack.Peek();
                    node.Enums.Add(enumDef);
                }
                else
                {
                    // Multi-line enum: parse any members after { on this line
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
                // Look ahead for opening brace
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
                            // Single-line body on the brace line
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
    /// Parses comma-separated enum members from a single line of text.
    /// Handles multiple members per line, inline comments, and values.
    /// </summary>
    private static void ParseEnumMembersFromLine(string line, CppEnumDef enumDef)
    {
        // Strip trailing // comment (applies to last member on line)
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

            // Validate it looks like a C++ identifier
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

        // Attach line comment to the last member on this line
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
        // e.g., 144.f → 144.0f, 750.0f → 750.0f, 166.0f → 166.0f
        if (value.EndsWith("f", StringComparison.OrdinalIgnoreCase))
        {
            value = value.Substring(0, value.Length - 1);
            // If it ends with just '.', append '0' for valid C# (e.g., 144. → 144.0)
            if (value.EndsWith("."))
                value += "0";
            return value + "f";
        }

        // If it's a float-looking literal without suffix (has decimal point), add f
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
        // Allow optional leading minus for negative literals
        var start = s[0] == '-' ? 1 : 0;
        if (start >= s.Length) return false;
        return s.Substring(start).All(char.IsDigit);
    }
}