using Microsoft.CodeAnalysis;
using Sybil;
using System;

namespace Daybreak.Generators;

[Generator(LanguageNames.CSharp)]
public class GWCAEquivalentAttributeGenerator : IIncrementalGenerator
{
    public const string AttributeName = "GWCAEquivalentAttribute";
    public const string AttributeShortName = "GWCAEquivalent";

    private const string AttributeNamespace = "Daybreak.Generators";
    private const string PropertyName = "GWCAName";
    private const string Public = "public";
    private const string StringType = "string";
    private const string ParameterName = "gwcaName";

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
    }
}
