namespace Daybreak.API.Serialization;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder WithSerializationContext(this WebApplicationBuilder builder)
    {
        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.TypeInfoResolverChain.Insert(0, new ApiJsonSerializerContext());
        });

        return builder;
    }
}
