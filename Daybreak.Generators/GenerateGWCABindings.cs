using System;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Daybreak.Generators;

#nullable enable
[Generator(LanguageNames.CSharp)]
public sealed class GenerateGWCABindings : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        var declarations = context.SyntaxProvider.CreateSyntaxProvider(
                predicate: static (s, _) => s is ClassDeclarationSyntax or StructDeclarationSyntax or EnumDeclarationSyntax,
                transform: static (ctx, _) => GetTypeDeclarationSyntax(ctx))
            .Where(static c => c is not null);

        var compilationWithContext = context.CompilationProvider.Combine(declarations.Collect());

        context.RegisterSourceOutput(compilationWithContext, GenerateBindings);
    }

    private static (TypeDeclarationSyntax, AttributeSyntax)? GetTypeDeclarationSyntax(GeneratorSyntaxContext context)
    {
        if (context.Node is not TypeDeclarationSyntax typeDeclarationSyntax)
        {
            return default;
        }

        if (typeDeclarationSyntax.AttributeLists
                .SelectMany(l => l.Attributes)
                .OfType<AttributeSyntax>()
                .FirstOrDefault(s => s.Name.ToString() is
                    GWCAEquivalentAttributeGenerator.AttributeName or GWCAEquivalentAttributeGenerator.AttributeShortName) is AttributeSyntax attr)
        {
            return (typeDeclarationSyntax, attr);
        }

        return default;
    }

    private static void GenerateBindings(
            SourceProductionContext context,
            (Compilation, ImmutableArray<(TypeDeclarationSyntax, AttributeSyntax)?>) compilationWithContext)
    {
        var (compilation, nullableTypes) = compilationWithContext;
        nullableTypes
            .OfType<(TypeDeclarationSyntax, AttributeSyntax)>()
            .Select();

    }
}
#nullable disable
