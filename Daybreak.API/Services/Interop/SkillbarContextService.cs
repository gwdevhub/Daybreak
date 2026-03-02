using System.Extensions;
using System.Extensions.Core;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Daybreak.API.Interop;
using Daybreak.API.Interop.GuildWars;
using Daybreak.API.Models;
using Daybreak.Shared.Models.Builds;
using Daybreak.Shared.Services.BuildTemplates;

namespace Daybreak.API.Services.Interop;

public sealed class SkillbarContextService : IHostedService, IInteropHealthService
{
    private const string Base64Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/";

    private static readonly byte[] DecodeTemplateHeaderSignature =
    [
        0x55, 0x8B, 0xEC, 0x56,
        0x8B, 0x75, 0x0C, 0x8B,
        0xCE, 0x6A, 0x04, 0xE8,
        0x00, 0x00, 0x00, 0x00,
        0x8B, 0xC8, 0x83, 0xE1,
        0x0E, 0x80, 0xF9, 0x0E,
        0x75, 0x00, 0xA9, 0xF1,
        0xFF, 0xFF, 0xFF, 0x74
    ];
    private const string DecodeTemplateHeaderMask = "xxxxxxxxxxxx????xxxxxxxxx?xxxxxx";
    private const int DecodeTemplateHeaderOffset = 0x0;

    // GWCA hooks LoadSkillTemplate first (patching the prologue), so byte-pattern scan fails.
    // Use the same assertion-based scan that GWCA uses to find the function reliably.
    private const string LoadSkillTemplateAssertionFile = "TemplatesHelpers.cpp";
    private const string LoadSkillTemplateAssertionMessage = "targetPrimaryProf == templateData.profPrimary";

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private unsafe delegate byte DecodeTemplateHeaderDelegate(SkillTemplate* outTemplate, BitReader* bitReader);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private unsafe delegate void LoadSkillTemplateDelegate(int targetAgentId, SkillTemplate* templateData);

    private readonly ChatService chatService;
    private readonly MemoryScanningService memoryScanningService;
    private readonly IBuildTemplateManager templateManager;
    private readonly GWAddressCache decodeTemplateHeaderAddressCache;
    private readonly GWHook<DecodeTemplateHeaderDelegate> decodeTemplateHeaderHook;
    private readonly GWAddressCache loadSkillTemplateAddressCache;
    private readonly GWHook<LoadSkillTemplateDelegate> loadSkillTemplateHook;
    private readonly ILogger<SkillbarContextService> logger;
    private readonly List<CallbackRegistration<Func<string, WrappedPointer<SkillTemplate>, bool>>> decodeTemplateHeaderCallbacks = [];
    private readonly List<CallbackRegistration<Func<SkillTemplate, bool>>> loadSkillTemplateCallbacks = [];

    public unsafe SkillbarContextService(
        ChatService chatService,
        IBuildTemplateManager buildTemplateManager,
        MemoryScanningService memoryScanningService,
        ILogger<SkillbarContextService> logger)
    {
        this.chatService = chatService;
        this.templateManager = buildTemplateManager;
        this.memoryScanningService = memoryScanningService;
        this.decodeTemplateHeaderAddressCache = new GWAddressCache(() =>
            this.memoryScanningService.FindAddress(DecodeTemplateHeaderSignature, DecodeTemplateHeaderMask, DecodeTemplateHeaderOffset));
        this.decodeTemplateHeaderHook = new GWHook<DecodeTemplateHeaderDelegate>(
            this.decodeTemplateHeaderAddressCache,
            this.DecodeTemplateHeaderDetour);
        this.loadSkillTemplateAddressCache = new GWAddressCache(() =>
        {
            var assertion = this.memoryScanningService.FindAssertion(
                LoadSkillTemplateAssertionFile,
                LoadSkillTemplateAssertionMessage);
            if (assertion is 0) return 0;
            return this.memoryScanningService.ToFunctionStartFromPadding(assertion);
        });
        this.loadSkillTemplateHook = new GWHook<LoadSkillTemplateDelegate>(
            this.loadSkillTemplateAddressCache,
            this.LoadSkillTemplateDetour);
        this.logger = logger;
    }

    public unsafe void LoadBuild(uint agentId, SkillTemplate* template) => GWCA.GW.SkillbarMgr.LoadSkillTemplate(agentId, template);

    public Task StartAsync(CancellationToken cancellationToken)
    {
        Task.Run(async () =>
        {
            while (!this.decodeTemplateHeaderHook.EnsureInitialized())
            {
                await Task.Delay(500, cancellationToken);
            }

            this.logger.LogInformation(
                "DecodeTemplateHeader hook installed at 0x{target:X8}",
                this.decodeTemplateHeaderHook.TargetAddress);
            this.logger.LogDebug("DecodeTemplateHeader hook diagnostics:\n{diagnostics}", this.decodeTemplateHeaderHook.GetDiagnosticInfo());

            while (!this.loadSkillTemplateHook.EnsureInitialized())
            {
                await Task.Delay(500, cancellationToken);
            }

            this.logger.LogInformation(
                "LoadSkillTemplate hook installed at 0x{target:X8}",
                this.loadSkillTemplateHook.TargetAddress);
            this.logger.LogDebug("LoadSkillTemplate hook diagnostics:\n{diagnostics}", this.loadSkillTemplateHook.GetDiagnosticInfo());
        }, cancellationToken);
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        this.decodeTemplateHeaderHook.Dispose();
        this.loadSkillTemplateHook.Dispose();
        return Task.CompletedTask;
    }

    public IEnumerable<AddressHealth> GetAddressHealth()
    {
        yield return new AddressHealth(
            "DecodeTemplateHeader",
            $"0x{this.decodeTemplateHeaderHook.TargetAddress:X8}",
            this.decodeTemplateHeaderHook.Hooked);
        yield return new AddressHealth(
            "LoadSkillTemplate",
            $"0x{this.loadSkillTemplateHook.TargetAddress:X8}",
            this.loadSkillTemplateHook.Hooked);
    }

    public CallbackRegistration<Func<string, WrappedPointer<SkillTemplate>, bool>> RegisterDecodeTemplateHeaderHandler(Func<string, WrappedPointer<SkillTemplate>, bool> callback)
    {
        var uid = Guid.NewGuid();
        var registration =
            new CallbackRegistration<Func<string, WrappedPointer<SkillTemplate>, bool>>(uid, callback, () => this.decodeTemplateHeaderCallbacks.RemoveAll(r => r.Uid == uid));
        this.decodeTemplateHeaderCallbacks.Add(registration);
        return registration;
    }

    public CallbackRegistration<Func<SkillTemplate, bool>> RegisterLoadSkillTemplateHandler(Func<SkillTemplate, bool> callback)
    {
        var uid = Guid.NewGuid();
        var registration =
            new CallbackRegistration<Func<SkillTemplate, bool>>(uid, callback, () => this.loadSkillTemplateCallbacks.RemoveAll(r => r.Uid == uid));
        this.loadSkillTemplateCallbacks.Add(registration);
        return registration;
    }

    private unsafe byte DecodeTemplateHeaderDetour(SkillTemplate* outTemplate, BitReader* bitReader)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (this.decodeTemplateHeaderCallbacks.Count > 0)
        {
            // Read the full raw buffer before the game consumes it
            var rawBytes = ReadBitReaderBuffer(bitReader);
            var templateString = ReencodeToBase64(rawBytes);
            if (this.decodeTemplateHeaderCallbacks.Any(r => r.Callback(templateString, outTemplate)))
            {
                scopedLogger.LogDebug("DecodeTemplateHeader callback handled template: {template}", templateString);
                return 1;
            }
        }

        return this.decodeTemplateHeaderHook.Continue(outTemplate, bitReader);
    }

    private unsafe void LoadSkillTemplateDetour(int targetAgentId, SkillTemplate* templateData)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        scopedLogger.LogDebug(
            "LoadSkillTemplateDetour called for agent {agentId}, templateData at 0x{addr:X8}",
            targetAgentId,
            (nuint)templateData);
        
        // Log template data for debugging
        if (templateData != null)
        {
            scopedLogger.LogDebug(
                "SkillTemplate: Primary={primary}, Secondary={secondary}",
                templateData->Primary,
                templateData->Secondary);
        }
        else
        {
            scopedLogger.LogWarning("templateData is null!");
        }

        if (this.loadSkillTemplateCallbacks.Any(r => r.Callback(*templateData)))
        {
            scopedLogger.LogDebug("LoadSkillTemplate callback handled loading for agent {agentId}", targetAgentId);
            return;
        }

        scopedLogger.LogDebug(
            "Calling Continue at 0x{continueAddr:X8}",
            this.loadSkillTemplateHook.ContinueAddress);
        
        this.loadSkillTemplateHook.Continue(targetAgentId, templateData);
        
        scopedLogger.LogDebug("Continue returned successfully");
    }

    /// <summary>
    /// Copies the entire byte buffer backing a BitReader (from StartPtr to EndPtr) into a managed byte array.
    /// </summary>
    private static unsafe byte[] ReadBitReaderBuffer(BitReader* reader)
    {
        var start = reader->StartPtr;
        var end = reader->EndPtr;
        if (start == 0 || end == 0 || end <= start)
        {
            return [];
        }

        var length = (int)(end - start);
        var buffer = new byte[length];
        fixed (byte* dst = buffer)
        {
            Buffer.MemoryCopy((void*)start, dst, length, length);
        }

        return buffer;
    }

    /// <summary>
    /// Re-encodes the raw BitReader buffer bytes back into the original base64 template string.
    /// The buffer contains packed bits (8 per byte) written by Base64DecodeTemplate's BitWriter,
    /// which wrote 6 bits per base64 character. We extract 6 bits at a time, LSB-first.
    /// </summary>
    private static string ReencodeToBase64(byte[] rawBytes)
    {
        var totalBits = rawBytes.Length * 8;
        var charCount = totalBits / 6;
        var chars = new char[charCount];

        var bitPosition = 0;
        for (var i = 0; i < charCount; i++)
        {
            var value = 0;
            for (var b = 0; b < 6; b++)
            {
                var byteIndex = bitPosition / 8;
                var bitIndex = bitPosition % 8;
                if (byteIndex < rawBytes.Length && ((rawBytes[byteIndex] >> bitIndex) & 1) != 0)
                {
                    value |= 1 << b;
                }

                bitPosition++;
            }

            chars[i] = value < Base64Alphabet.Length ? Base64Alphabet[value] : '?';
        }

        return new string(chars);
    }
}
