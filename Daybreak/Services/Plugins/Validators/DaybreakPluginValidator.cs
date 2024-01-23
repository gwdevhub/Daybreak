using Daybreak.Models.Plugins;
using Plumsy.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;

namespace Daybreak.Services.Plugins.Validators;
internal sealed class DaybreakPluginValidator : IMetadataValidator, IEnvironmentVersionValidator, ITypeDefinitionsValidator
{
    public bool Validate(IEnumerable<TypeDefinition> typeDefinitions, MetadataReader metadataReader)
    {
        return metadataReader.TypeDefinitions
            .Select(metadataReader.GetTypeDefinition)
            .Where(typeDef => typeDef.Attributes.HasFlag(System.Reflection.TypeAttributes.Public))
            .Where(typeDef => IsOfTypePluginConfigurationBase(metadataReader, typeDef))
            .Any();
    }

    public bool Validate(Version currentVersion, Version pluginVersion)
    {
        return currentVersion.Major >= pluginVersion.Major;
    }

    public bool Validate(MetadataReader metadataReader)
    {
        return true;
    }

    private static bool IsOfTypePluginConfigurationBase(MetadataReader metadataReader, TypeDefinition typeDefinition)
    {
        if (typeDefinition.BaseType.Kind != HandleKind.TypeReference)
        {
            return false;
        }

        var baseTypeReference = metadataReader.GetTypeReference((TypeReferenceHandle)typeDefinition.BaseType);
        var baseTypeName = metadataReader.GetString(baseTypeReference.Name);
        return baseTypeName == nameof(PluginConfigurationBase);
    }
}
