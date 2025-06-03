using Daybreak.API.Services.Interop;
using Daybreak.Shared.Services.BuildTemplates;
using Daybreak.Shared.Services.MDns;
using System.Diagnostics.CodeAnalysis;

namespace Daybreak.API.Services;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder WithDaybreakServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IBuildTemplateManager, BuildTemplateManager>();
        builder.Services.AddSingleton<IMDnsService, MDnsService>();
        builder.Services.AddHostedService<ApiAdvertisingService>();
        builder.Services.AddSingleton<MemoryScanningService>();
        builder.Services.AddSingleton<ChatService>();
        builder.Services.AddSingleton<MainPlayerService>();
        builder.Services.AddSingleton<CharacterSelectService>();
        builder.Services.AddSingleton<PartyService>();
        builder.WithHookHostedService<GameThreadService>();
        builder.WithHookHostedService<UIHandlingService>();
        builder.WithHookHostedService<ChatHandlingService>();
        builder.WithHookHostedService<SkillbarContextService>();
        builder.WithAddressService<GameContextService>();
        builder.WithAddressService<InstanceContextService>();
        builder.WithAddressService<AgentContextService>();
        builder.WithAddressService<PlatformContextService>();
        return builder;
    }

    private static WebApplicationBuilder WithHookService<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(this WebApplicationBuilder builder)
        where T : class, IHookHealthService
    {
        builder.Services.AddSingleton<T>();
        builder.Services.AddSingleton<IHookHealthService>(sp => sp.GetRequiredService<T>());
        return builder;
    }

    private static WebApplicationBuilder WithAddressService<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(this WebApplicationBuilder builder)
        where T : class, IAddressHealthService
    {
        builder.Services.AddSingleton<T>();
        builder.Services.AddSingleton<IAddressHealthService>(sp => sp.GetRequiredService<T>());
        return builder;
    }

    private static WebApplicationBuilder WithHookHostedService<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(this WebApplicationBuilder builder)
        where T : class, IHookHealthService, IHostedService
    {
        builder.Services.AddSingleton<T>();
        builder.Services.AddSingleton<IHostedService>(sp => sp.GetRequiredService<T>());
        builder.Services.AddSingleton<IHookHealthService>(sp => sp.GetRequiredService<T>());
        return builder;
    }

    private static WebApplicationBuilder WithAddressHostedService<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(this WebApplicationBuilder builder)
        where T : class, IAddressHealthService, IHostedService
    {
        builder.Services.AddSingleton<T>();
        builder.Services.AddSingleton<IHostedService>(sp => sp.GetRequiredService<T>());
        builder.Services.AddSingleton<IAddressHealthService>(sp => sp.GetRequiredService<T>());
        return builder;
    }

    private static WebApplicationBuilder WithHookAddressService<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(this WebApplicationBuilder builder)
        where T : class, IHookHealthService, IAddressHealthService
    {
        builder.Services.AddSingleton<T>();
        builder.Services.AddSingleton<IAddressHealthService>(sp => sp.GetRequiredService<T>());
        builder.Services.AddSingleton<IHookHealthService>(sp => sp.GetRequiredService<T>());
        return builder;
    }

    private static WebApplicationBuilder WithHookAddressHostedService<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] T>(this WebApplicationBuilder builder)
        where T : class, IHookHealthService, IAddressHealthService, IHostedService
    {
        builder.Services.AddSingleton<T>();
        builder.Services.AddSingleton<IAddressHealthService>(sp => sp.GetRequiredService<T>());
        builder.Services.AddSingleton<IHookHealthService>(sp => sp.GetRequiredService<T>());
        builder.Services.AddSingleton<IHostedService>(sp => sp.GetRequiredService<T>());
        return builder;
    }
}
