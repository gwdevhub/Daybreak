using Daybreak.API.Interop;
using Daybreak.API.Interop.GuildWars;
using Daybreak.API.Models;
using System.Core.Extensions;
using System.Extensions.Core;
using System.Runtime.InteropServices;
using System.Security;

namespace Daybreak.API.Services.Interop;

public sealed class SkillbarContextService
    : IAddressHealthService
{
    private const string LoadSkillTemplateFile = "TemplatesHelpers.cpp";
    private const string LoadSkillTemplateAssertion = "targetPrimaryProf == templateData.profPrimary";

    [SuppressUnmanagedCodeSecurity]
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private unsafe delegate void LoadSkillTemplate(uint agentId, SkillTemplate* template);

    private readonly MemoryScanningService memoryScanningService;
    private readonly GWDelegateCache<LoadSkillTemplate> loadSkillTemplateFunc;
    private readonly ILogger<SkillbarContextService> logger;

    public SkillbarContextService(
        MemoryScanningService memoryScanningService,
        ILogger<SkillbarContextService> logger)
    {
        this.logger = logger.ThrowIfNull();
        this.memoryScanningService = memoryScanningService.ThrowIfNull();
        unsafe
        {
            this.loadSkillTemplateFunc = new GWDelegateCache<LoadSkillTemplate>(
            new GWAddressCache(() => this.memoryScanningService.ToFunctionStart(
                this.memoryScanningService.FindAssertion(LoadSkillTemplateFile, LoadSkillTemplateAssertion, 0, 0), 0x200)));
        }
    }

    public List<AddressState> GetAddressStates()
    {
        return
            [
                new()
                {
                    Address = this.loadSkillTemplateFunc.Cache.GetAddress() ?? 0U,
                    Name = nameof(this.loadSkillTemplateFunc)
                }
            ];
    }

    public unsafe void LoadBuild(uint agentId, SkillTemplate* template)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (this.loadSkillTemplateFunc.GetDelegate() is not LoadSkillTemplate func)
        {
            scopedLogger.LogError("Load skill template delegate is not initialized");
            return;
        }

        func(agentId, template);
    }
}
