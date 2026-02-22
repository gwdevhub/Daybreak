using Daybreak.API.Services.Interop;
using Daybreak.Shared.Services.BuildTemplates;
using System.Diagnostics.CodeAnalysis;

namespace Daybreak.API.Services;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder WithDaybreakServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddSingleton<IBuildTemplateManager, BuildTemplateManager>();
        builder.Services.AddSingleton<MemoryScanningService>();
        builder.Services.AddSingleton<ChatService>();
        builder.Services.AddSingleton<MainPlayerService>();
        builder.Services.AddSingleton<CharacterSelectService>();
        builder.Services.AddSingleton<PartyService>();
        builder.Services.AddSingleton<UIService>();
        builder.Services.AddSingleton<LoginService>();
        builder.Services.AddSingleton<InventoryService>();
        builder.Services.AddSingleton<GameThreadService>();
        builder.Services.AddSingleton<SkillbarContextService>();
        builder.Services.AddSingleton<GameContextService>();
        builder.Services.AddSingleton<InstanceContextService>();
        builder.Services.AddSingleton<AgentContextService>();
        builder.Services.AddSingleton<PlatformContextService>();
        builder.Services.AddSingleton<PartyContextService>();
        builder.Services.AddSingleton<PreferencesService>();
        builder.Services.AddSingleton<UIContextService>();
        return builder;
    }
}
