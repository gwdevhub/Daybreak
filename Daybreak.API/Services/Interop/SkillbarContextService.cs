using Daybreak.API.Interop;
using Daybreak.API.Interop.GuildWars;
using Daybreak.API.Models;
using System.Core.Extensions;
using System.Extensions.Core;
using System.Runtime.InteropServices;
using System.Security;

namespace Daybreak.API.Services.Interop;

public sealed class SkillbarContextService
    : IHostedService, IHookHealthService
{
    private const string LoadSkillTemplateFile = "TemplatesHelpers.cpp";
    private const string LoadSkillTemplateAssertion = "targetPrimaryProf == templateData.profPrimary";

    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private unsafe delegate void LoadSkillTemplate(uint agentId, SkillTemplate* template);

    private readonly MemoryScanningService memoryScanningService;
    private readonly GWHook<LoadSkillTemplate> loadSkillTemplateHook;
    private readonly ILogger<SkillbarContextService> logger;

    private CancellationTokenSource? cts = default;

    public SkillbarContextService(
        MemoryScanningService memoryScanningService,
        ILogger<SkillbarContextService> logger)
    {
        this.logger = logger.ThrowIfNull();
        this.memoryScanningService = memoryScanningService.ThrowIfNull();
        unsafe
        {
            this.loadSkillTemplateHook = new GWHook<LoadSkillTemplate>(
            new GWAddressCache(() => this.memoryScanningService.ToFunctionStart(
                this.memoryScanningService.FindAssertion(LoadSkillTemplateFile, LoadSkillTemplateAssertion, 0, 0), 0x200)),
            this.OnLoadSkillTemplate);
        }
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        this.cts?.Dispose();
        this.cts = new CancellationTokenSource();
        return Task.Factory.StartNew(() => this.InitializeHooks(this.cts.Token), this.cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        this.cts?.Cancel();
        this.cts?.Dispose();
        this.loadSkillTemplateHook.Dispose();
        return Task.CompletedTask;
    }

    public List<HookState> GetHookStates()
    {
        return
            [
                new HookState
                {
                    Hooked = this.loadSkillTemplateHook.Hooked,
                    Name = nameof(this.loadSkillTemplateHook),
                    TargetAddress = new PointerValue(this.loadSkillTemplateHook.TargetAddress),
                    ContinueAddress = new PointerValue(this.loadSkillTemplateHook.ContinueAddress),
                    DetourAddress = new PointerValue(this.loadSkillTemplateHook.DetourAddress)
                }
            ];
    }

    public unsafe void LoadBuild(uint agentId, SkillTemplate* template) => this.OnLoadSkillTemplate(agentId, template);

    private unsafe void OnLoadSkillTemplate(uint agentId, SkillTemplate* template)
    {
        this.loadSkillTemplateHook.Continue(agentId, template);
    }

    private async ValueTask InitializeHooks(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        scopedLogger.LogInformation("Initializing hooks");
        while (!this.loadSkillTemplateHook.EnsureInitialized())
        {
            scopedLogger.LogDebug("Waiting for hook to initialize: {hook}", nameof(this.loadSkillTemplateHook));
            await Task.Delay(1000, cancellationToken);
        }

        scopedLogger.LogInformation("Hooks initialized");
    }
}
