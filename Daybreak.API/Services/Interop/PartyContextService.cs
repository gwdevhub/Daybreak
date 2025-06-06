using Daybreak.API.Models;
using System.ComponentModel;
using System.Core.Extensions;
using System.Extensions;
using System.Extensions.Core;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Daybreak.API.Services.Interop;

public sealed class PartyContextService(
    MemoryScanningService memoryScanningService,
    ILogger<PartyContextService> logger)
        : IAddressHealthService, IHostedService
{
    private const string SearchButtonFile = "\\Code\\Gw\\Ui\\Game\\Party\\PtSearch.cpp";
    private const string SearchButtonAssertion = "m_activeList == LIST_HEROES";
    private const string WindowButtonFile = "\\Code\\Gw\\Ui\\Game\\Party\\PtButtons.cpp";
    private const string WindowButtonAssertion = "m_selection.agentId";

    // Fastcall funcs do not work in .NET for x86 right now. https://github.com/dotnet/runtime/issues/113851
    private unsafe delegate* unmanaged[Stdcall]<void*, uint, uint*, void> searchButtonCallback;
    private unsafe delegate* unmanaged[Stdcall]<void*, uint, uint*, void> windowButtonCallback;

    private readonly MemoryScanningService memoryScanningService = memoryScanningService.ThrowIfNull();
    private readonly ILogger<PartyContextService> logger = logger.ThrowIfNull();

    private CancellationTokenSource? cts;

    public Task StartAsync(CancellationToken cancellationToken)
    {
        this.cts?.Dispose();
        this.cts = new CancellationTokenSource();
        return Task.Factory.StartNew(() => this.InitializeCallbacks(this.cts.Token), this.cts.Token, TaskCreationOptions.LongRunning, TaskScheduler.Current);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        this.cts?.Dispose();
        return Task.CompletedTask;
    }

    public List<AddressState> GetAddressStates()
    {
        unsafe
        {
            return
            [
                new AddressState
                {
                    Address = (nuint)this.searchButtonCallback,
                    Name = nameof(this.searchButtonCallback)
                },
                new AddressState
                {
                    Address = (nuint)this.windowButtonCallback,
                    Name = nameof(this.windowButtonCallback)
                }
            ];
        }
    }

    public unsafe bool CallSearchButtonCallback(void* ctx, uint edx, uint* wparam)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (this.searchButtonCallback is null)
        {
            scopedLogger.LogError("SearchButtonCallbackFunc address not found");
            return false;
        }

        Invoke(this.searchButtonCallback, ctx, edx, wparam);
        return true;
    }

    public unsafe bool CallWindowButtonCallback(void* ctx, uint edx, uint* wparam)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        if (this.windowButtonCallback is null)
        {
            scopedLogger.LogError("WindowButtonCallbackFunc address not found");
            return false;
        }

        Invoke(this.windowButtonCallback, ctx, edx, wparam);
        return true;
    }

    public unsafe bool AddHero(uint heroId)
    {
        Span<uint> wparamSpan = stackalloc uint[4];
        wparamSpan.Clear();
        wparamSpan[2] = 0x6;
        wparamSpan[1] = 0x1;

        Span<uint> ctxSpan = stackalloc uint[13];
        ctxSpan.Clear();
        ctxSpan[0xb] = 1;
        ctxSpan[9] = heroId;
        fixed (uint* ctx = ctxSpan)
        fixed (uint* wparam = wparamSpan)
        {
            return this.CallSearchButtonCallback(ctx, 2, wparam);
        }
    }

    public unsafe bool KickHero(uint heroId)
    {
        Span<uint> wparamSpan = stackalloc uint[4];
        wparamSpan.Clear();
        wparamSpan[2] = 0x6;
        wparamSpan[1] = 0x6;

        Span<uint> ctxSpan = stackalloc uint[13];
        ctxSpan.Clear();
        ctxSpan[0xb] = 1;
        ctxSpan[9] = heroId;
        fixed (uint* ctx = ctxSpan)
        fixed (uint* wparam = wparamSpan)
        {
            return this.CallSearchButtonCallback(ctx, 0, wparam);
        }
    }

    public bool KickAllHeroes() => this.KickHero(0x26);

    public unsafe bool LeaveParty()
    {
        Span<uint> ctxSpan = stackalloc uint[14];
        ctxSpan.Clear(); // Ensure all values are zeroed
        ctxSpan[0xd] = 1;
        fixed (uint* ctx = ctxSpan)
        {
            return this.CallWindowButtonCallback(ctx, 0, (uint*)0);
        }
    }

    private async ValueTask InitializeCallbacks(CancellationToken cancellationToken)
    {
        var scopedLogger = this.logger.CreateScopedLogger();
        scopedLogger.LogInformation("Initializing callbacks");
        while(!cancellationToken.IsCancellationRequested)
        {
            unsafe
            {
                if (this.searchButtonCallback is null)
                {
                    var addr = this.memoryScanningService.ToFunctionStart(this.memoryScanningService.FindAssertion(SearchButtonFile, SearchButtonAssertion, 0, 0));
                    if (addr is not 0)
                    {
                        this.searchButtonCallback = BuildFastcallThunk(addr);
                    }
                }

                if (this.windowButtonCallback is null)
                {
                    var addr = this.memoryScanningService.ToFunctionStart(this.memoryScanningService.FindAssertion(WindowButtonFile, WindowButtonAssertion, 0, 0));
                    if (addr is not 0)
                    {
                        this.windowButtonCallback = BuildFastcallThunk(addr);
                    }
                }

                if (this.searchButtonCallback is not null && this.windowButtonCallback is not null)
                {
                    break;
                }
            }

            await Task.Delay(1000, cancellationToken);
        }

        scopedLogger.LogInformation("Callbacks initialized");
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static unsafe void Invoke(delegate* unmanaged[Stdcall]<void*, uint, uint*, void> f,
                               void* ctx, uint edx, uint* wparam)
    {
        if (f is null)
        {
            return;
        }

        f(ctx, edx, wparam);  // stdcall on our side -> fastcall inside thunk
    }

    private static unsafe delegate* unmanaged[Stdcall]
        <void*, uint, uint*, void> BuildFastcallThunk(nuint target)
    {
        if (target is 0)
        {
            return null;
        }

        Span<byte> code =
        [
            0x58,                      // pop eax               (ret)
            0x59,                      // pop ecx               (ctx)
            0x5A,                      // pop edx               (edxVal)
            0x5B,                      // pop ebx               (wparam)
            0x53,                      // push ebx              (wparam)
            0x50,                      // push eax              (ret)
            0xB8,0,0,0,0,              // mov  eax, TARGET
            0xFF,0xE0                  // jmp  eax
        ];                             // 13 bytes total

        const int TARGET_OFFSET = 7;
        Unsafe.WriteUnaligned(ref code[TARGET_OFFSET], (uint)target);

        nint mem = NativeMethods.VirtualAlloc(0, (nuint)code.Length, NativeMethods.MEM_COMMIT | NativeMethods.MEM_RESERVE, NativeMethods.PAGE_EXECUTE_READWRITE);
        if (mem == 0)
        {
            throw new Win32Exception(Marshal.GetLastWin32Error(), "VirtualAlloc");
        }

        fixed (byte* src = code)
        {
            Buffer.MemoryCopy(src, (void*)mem, code.Length, code.Length);
        }

        return (delegate* unmanaged[Stdcall]<void*, uint, uint*, void>)mem;
    }
}
