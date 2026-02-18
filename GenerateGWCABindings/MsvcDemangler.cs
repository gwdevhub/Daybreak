namespace GenerateGWCABindings;

/// <summary>
/// Minimal MSVC C++ decorated name demangler.
/// Handles the subset of mangling used by GWCA exports:
/// free functions (?name@ns@@Y...), static/virtual member functions,
/// basic types, pointers, references, enums, structs, function pointers,
/// templates (first arg only), and back-references (global name table).
/// </summary>
internal sealed class MsvcDemangler
{
    private static readonly Dictionary<char, string> BasicTypes = new()
    {
        ['X'] = "void",
        ['D'] = "byte",   // char
        ['E'] = "byte",   // unsigned char
        ['F'] = "short",
        ['G'] = "ushort",
        ['H'] = "int",
        ['I'] = "uint",
        ['J'] = "int",    // long (32-bit)
        ['K'] = "uint",   // unsigned long (32-bit)
        ['M'] = "float",
        ['N'] = "double",
    };

    private static readonly Dictionary<string, string> TwoCharBasicTypes = new()
    {
        ["_N"] = "bool",
        ["_J"] = "long",
        ["_K"] = "ulong",
        ["_W"] = "ushort",  // wchar_t (ushort is blittable, char is not)
    };

    /// <summary>
    /// Represents a demangled C++ type with enough detail to resolve to C# types.
    /// </summary>
    public record DemangledType(TypeKind Kind, string Name, string? QualifiedName = null, DemangledType? TemplateArg = null, string? UnderlyingType = null)
    {
        public override string ToString() => Kind switch
        {
            TypeKind.Primitive => Name,
            TypeKind.Pointer => TemplateArg is not null ? $"ptr:{Name}<{TemplateArg}>" : $"ptr:{QualifiedName ?? Name}",
            TypeKind.Enum => $"enum:{QualifiedName ?? Name}",
            TypeKind.Struct => TemplateArg is not null ? $"struct:{Name}<{TemplateArg}>" : $"struct:{QualifiedName ?? Name}",
            TypeKind.FuncPtr => "funcptr",
            _ => Name
        };
    }

    public enum TypeKind { Primitive, Pointer, Enum, Struct, FuncPtr }

    public record DemangledFunction(
        string[] NamespaceParts,
        string FunctionName,
        bool IsMember,
        DemangledType ReturnType,
        DemangledType[] ParameterTypes,
        bool IsVarArgs
    )
    {
        public string QualifiedCppName
        {
            get
            {
                var ns = string.Join("::", NamespaceParts);
                return ns.Length > 0 ? $"{ns}::{FunctionName}" : FunctionName;
            }
        }

        public string Region => NamespaceParts.Length > 0
            ? string.Join("::", NamespaceParts)
            : "GW";
    }

    public static DemangledFunction? Demangle(string mangled)
    {
        if (!mangled.StartsWith('?'))
            return null;

        var s = mangled.AsSpan()[1..]; // skip '?'
        int pos = 0;

        // Global name table — shared across all ReadQualifiedName calls
        var nameTable = new List<string>();

        // Read qualified name parts
        var parts = ReadQualifiedName(s, ref pos, nameTable);
        if (parts.Count == 0)
            return null;

        var funcName = parts[0];
        var nsParts = new string[parts.Count - 1];
        for (int i = 1; i < parts.Count; i++)
            nsParts[parts.Count - 1 - i] = parts[i];

        if (pos >= s.Length)
            return null;

        // Function type qualifier
        var c = s[pos];
        bool isMember = false;

        switch (c)
        {
            case 'Y': // non-member (free function)
                pos++;
                break;
            case 'S': // static member
                pos++;
                break;
            case 'Q' or 'A' or 'I' or 'M' or 'C': // member (various access)
                isMember = true;
                pos++;
                if (pos < s.Length && s[pos] is >= 'A' and <= 'M') pos++;
                if (pos < s.Length && s[pos] is >= 'A' and <= 'E') pos++;
                break;
            case 'U' or 'E': // virtual member
                isMember = true;
                pos++;
                if (pos < s.Length && s[pos] is >= 'A' and <= 'M') pos++;
                if (pos < s.Length && s[pos] is >= 'A' and <= 'E') pos++;
                break;
            default:
                pos++;
                if (pos < s.Length && s[pos] is >= 'A' and <= 'E') pos++;
                break;
        }

        if (pos >= s.Length)
            return null;

        // Calling convention (A=cdecl, E=thiscall, etc.)
        pos++;

        // Return type — may be prefixed with ?A (non-const) or ?B (const)
        if (pos + 1 < s.Length && s[pos] == '?' && s[pos + 1] is 'A' or 'B')
            pos += 2;

        var retType = ReadType(s, ref pos, nameTable);

        // Parameters
        bool isVarArgs = mangled.EndsWith("ZZ");
        var paramTypes = ReadParams(s, ref pos, nameTable);

        return new DemangledFunction(nsParts, funcName, isMember, retType, [.. paramTypes], isVarArgs);
    }

    private static List<string> ReadQualifiedName(ReadOnlySpan<char> s, ref int pos, List<string> nameTable)
    {
        // MSVC qualified names are encoded as Name1@Name2@...@NameN@@
        // The @@ terminator shares its first @ with the separator after the
        // last name component, so we must look ahead after each part.
        //
        // Back-reference digits refer to the GLOBAL name table, not local parts.
        var parts = new List<string>();
        while (pos < s.Length)
        {
            if (s[pos] == '@')
            {
                // Bare @ with no preceding name part — check for @@
                if (pos + 1 < s.Length && s[pos + 1] == '@')
                {
                    pos += 2;
                    return parts;
                }
                pos++;
                continue;
            }

            // Handle numeric back-references (single digit 0-9)
            // Unlike text name parts which need a trailing @ separator,
            // digit back-refs are self-delimiting — a single @ after a
            // digit terminates the qualified name.
            if (char.IsDigit(s[pos]))
            {
                int refIdx = s[pos] - '0';
                if (refIdx < nameTable.Count)
                    parts.Add(nameTable[refIdx]);
                else
                    parts.Add($"_backref{refIdx}");
                pos++;

                // Single @ terminates after a digit back-reference
                if (pos < s.Length && s[pos] == '@')
                {
                    pos++;
                    return parts;
                }
                continue;
            }

            // Handle template names: ?$ prefix
            if (s[pos] == '?' && pos + 1 < s.Length && s[pos + 1] == '$')
            {
                pos += 2;
                int nameEnd = s[pos..].IndexOf('@');
                if (nameEnd < 0) break;
                nameEnd += pos;
                var templateName = new string(s[pos..nameEnd]);
                pos = nameEnd + 1;

                // Add template name to the global name table
                if (nameTable.Count < 10)
                    nameTable.Add(templateName);

                // Parse the first template argument as a type
                DemangledType? templateArgType = null;
                if (pos < s.Length && s[pos] != '@')
                {
                    templateArgType = ReadType(s, ref pos, nameTable);
                }

                // Skip any remaining template arguments until @@
                while (pos < s.Length)
                {
                    if (s[pos] == '@')
                    {
                        if (pos + 1 < s.Length && s[pos + 1] == '@')
                        {
                            pos += 2;
                            break;
                        }
                        pos++;
                        continue;
                    }
                    pos++;
                }

                // Encode template info into the part string
                if (templateArgType is not null)
                    parts.Add($"{templateName}|{templateArgType.Kind}|{templateArgType.Name}|{templateArgType.QualifiedName ?? ""}");
                else
                    parts.Add(templateName);

                // Check if next char starts @@
                if (pos < s.Length && s[pos] == '@' && pos + 1 < s.Length && s[pos + 1] == '@')
                {
                    pos += 2;
                    return parts;
                }
                continue;
            }

            int end = s[pos..].IndexOf('@');
            if (end < 0) break;
            end += pos;
            var name = new string(s[pos..end]);
            parts.Add(name);

            // Add to the global name table (max 10 entries per MSVC spec)
            if (nameTable.Count < 10)
                nameTable.Add(name);

            pos = end + 1; // skip the @ separator

            // The @ we just consumed might be the first @ of the @@ terminator
            if (pos < s.Length && s[pos] == '@')
            {
                pos++; // skip the second @
                return parts;
            }
        }
        return parts;
    }

    /// <summary>
    /// Extracts the unqualified (leaf) name from a list of qualified name parts
    /// produced by ReadQualifiedName. Parts come in declaration order
    /// [Name, Parent, Grandparent...], so [0] is the leaf.
    /// </summary>
    private static (string unqualified, string qualified, DemangledType? templateArg) BuildTypeName(List<string> parts)
    {
        DemangledType? templateArg = null;

        // Process parts to extract template arg info (encoded as "Name|Kind|InnerName|QualName")
        var cleanParts = new List<string>(parts.Count);
        for (int i = 0; i < parts.Count; i++)
        {
            var p = parts[i];
            if (p.Contains('|'))
            {
                var segments = p.Split('|', 4);
                cleanParts.Add(segments[0]);
                if (i == 0 && segments.Length == 4) // only care about template arg on the leaf type
                {
                    var kind = Enum.Parse<TypeKind>(segments[1]);
                    var innerName = segments[2];
                    var qualName = segments[3].Length > 0 ? segments[3] : null;
                    templateArg = new DemangledType(kind, innerName, qualName);
                }
            }
            else
            {
                cleanParts.Add(p);
            }
        }

        var reversed = new List<string>(cleanParts);
        reversed.Reverse();
        var qualified = string.Join("::", reversed);
        var unqualified = cleanParts.Count > 0 ? cleanParts[0] : qualified;
        return (unqualified, qualified, templateArg);
    }

    private static DemangledType ReadType(ReadOnlySpan<char> s, ref int pos, List<string> nameTable)
    {
        if (pos >= s.Length) return new(TypeKind.Primitive, "void");

        // Two-char basic types
        if (pos + 1 < s.Length)
        {
            var twoChar = new string(s[pos..(pos + 2)]);
            if (TwoCharBasicTypes.TryGetValue(twoChar, out var tc))
            {
                pos += 2;
                return new(TypeKind.Primitive, tc);
            }
        }

        var c = s[pos];

        // Single-char basic types
        if (BasicTypes.TryGetValue(c, out var basic))
        {
            pos++;
            return new(TypeKind.Primitive, basic);
        }

        // W0-W7 = enum (digit encodes underlying type width)
        if (c == 'W' && pos + 1 < s.Length && s[pos + 1] is >= '0' and <= '7')
        {
            var enumWidth = s[pos + 1];
            var underlying = enumWidth switch
            {
                '0' => "byte",   // char
                '1' => "byte",   // unsigned char
                '2' => "short",
                '3' => "ushort",
                '4' => "int",
                '5' => "uint",
                '6' => "int",    // long (32-bit)
                '7' => "uint",   // unsigned long (32-bit)
                _ => "int"
            };
            pos += 2;
            var parts = ReadQualifiedName(s, ref pos, nameTable);
            var (unqual, qual, _) = BuildTypeName(parts);
            return new(TypeKind.Enum, unqual, qual, UnderlyingType: underlying);
        }

        // Pointers / references
        if (c is 'P' or 'Q' or 'A')
        {
            pos++;
            if (pos >= s.Length) return new(TypeKind.Pointer, "void");

            // Function pointer: P6...
            if (s[pos] == '6')
            {
                pos++;
                ReadFuncPtr(s, ref pos, nameTable);
                return new(TypeKind.FuncPtr, "funcptr");
            }

            // Const qualifier (A=non-const, B=const)
            if (pos < s.Length && s[pos] is 'A' or 'B')
                pos++;

            if (pos >= s.Length) return new(TypeKind.Pointer, "void");

            var inner = s[pos];

            if (inner == 'X') { pos++; return new(TypeKind.Pointer, "void"); } // void*

            // Two-char under pointer
            if (inner == '_' && pos + 1 < s.Length)
            {
                var twoInner = new string(s[pos..(pos + 2)]);
                if (TwoCharBasicTypes.TryGetValue(twoInner, out var tc))
                {
                    pos += 2;
                    return new(TypeKind.Pointer, tc);
                }
            }

            if (BasicTypes.TryGetValue(inner, out var basicInner))
            {
                pos++;
                return new(TypeKind.Pointer, basicInner);
            }

            if (inner is 'U' or 'V') // struct/class pointer
            {
                pos++;
                var parts = ReadQualifiedName(s, ref pos, nameTable);
                var (unqual, qual, templateArg) = BuildTypeName(parts);
                return new(TypeKind.Pointer, unqual, qual, templateArg);
            }

            if (inner is 'P' or 'Q' or 'A') // nested pointer
            {
                ReadType(s, ref pos, nameTable);
                return new(TypeKind.Pointer, "void");
            }

            // Back-reference digit
            if (char.IsDigit(inner)) { pos++; return new(TypeKind.Pointer, "void"); }

            pos++;
            return new(TypeKind.Pointer, "void");
        }

        // Struct/class by value
        if (c is 'U' or 'V')
        {
            pos++;
            var parts = ReadQualifiedName(s, ref pos, nameTable);
            var (unqual, qual, templateArg) = BuildTypeName(parts);
            return new(TypeKind.Struct, unqual, qual, templateArg);
        }

        // Back-reference digit
        if (char.IsDigit(c)) { pos++; return new(TypeKind.Primitive, "nint"); }

        pos++;
        return new(TypeKind.Primitive, "nint");
    }

    private static void ReadFuncPtr(ReadOnlySpan<char> s, ref int pos, List<string> nameTable)
    {
        // Calling convention
        if (pos < s.Length && char.IsAsciiLetterUpper(s[pos])) pos++;
        // Return type
        ReadType(s, ref pos, nameTable);
        // Params until @ or Z
        while (pos < s.Length)
        {
            if (s[pos] is '@' or 'Z') { pos++; break; }
            ReadType(s, ref pos, nameTable);
        }
    }

    private static List<DemangledType> ReadParams(ReadOnlySpan<char> s, ref int pos, List<string> nameTable)
    {
        var parms = new List<DemangledType>();
        while (pos < s.Length)
        {
            if (s[pos] == 'Z') { pos++; return parms; }
            if (s[pos] == '@')
            {
                if (pos + 1 < s.Length && s[pos + 1] == 'Z') { pos += 2; return parms; }
                pos++;
                continue;
            }
            if (s[pos] == 'X') { pos++; return parms; } // void = no params

            parms.Add(ReadType(s, ref pos, nameTable));
        }
        return parms;
    }
}
