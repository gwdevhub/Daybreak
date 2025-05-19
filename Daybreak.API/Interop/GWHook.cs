using System.Runtime.InteropServices;
using MinHook;

namespace Daybreak.API.Interop;

/// <summary>
/// A thin convenience wrapper around MinHook that
/// resolves the target address lazily through <see cref="GWAddressCache"/>
/// unwraps an existing “CALL/JMP rel32” stub so we patch the real code
/// exposes <see cref="Original"/> so your detour can chain
/// </summary>
public sealed class GWHook<T>(
    GWAddressCache funcAddress,
    T detour,
    bool bypassPreviousHooks = false) : IHook<T>
    where T : Delegate
{
    private readonly bool bypassPreviousHooks = bypassPreviousHooks;
    private readonly GWAddressCache addressCache = funcAddress ?? throw new ArgumentNullException(nameof(funcAddress));
    private readonly T detour = detour ?? throw new ArgumentNullException(nameof(detour));
    private readonly SemaphoreSlim semaphore = new(1, 1);

    private T? cont;          // trampoline returned by MinHook
    private HookEngine? engine;

    /// <summary>Delegate that calls the next hook / real function.</summary>
    public T Continue => this.cont ??
        throw new InvalidOperationException("Hook not initialised – call EnsureInitialized() first.");

    public bool Hooked { get; private set; } = false;
    public nuint TargetAddress { get; private set; }
    public nuint ContinueAddress { get; private set; }
    public nuint DetourAddress { get; private set; }

    /// <summary>Installs the hook exactly once (thread-safe).</summary>
    public bool EnsureInitialized()
    {
        if (this.Hooked)
        {
            return true;
        }

        this.semaphore.Wait();
        try
        {
            if (this.Hooked)
            {
                return true;
            }

            var addr = this.addressCache.GetAddress();
            if (addr.HasValue is false ||
                addr.Value is 0)
            {
                return false;
            }

            var target = this.bypassPreviousHooks
                ? UnwrapNearBranch(addr.Value)
                : addr.Value;
            if (target is 0)
            {
                return false;
            }

            this.engine = new HookEngine();
            this.cont = this.engine.CreateHook((nint)target, this.detour);
            this.engine.EnableHooks();
            this.TargetAddress = target;
            this.ContinueAddress = (nuint)this.cont.Method.MethodHandle.GetFunctionPointer();
            this.DetourAddress = (nuint)this.detour.Method.MethodHandle.GetFunctionPointer();
            this.Hooked = true;
            return true;
        }
        catch (Exception)
        {
            return false;
        }
        finally
        {
            this.semaphore.Release();
        }
    }

    public void Dispose()
    {
        this.semaphore.Wait();
        try
        {
            if (!this.Hooked ||
                this.engine is null)
            {
                return;
            }

            this.engine.DisableHooks();
            this.engine.Dispose();

            this.cont = null;
            this.Hooked = false;
        }
        finally
        {
            this.semaphore.Release();
        }
    }

    /// <summary>
    /// Follows a <c>CALL rel32</c> (<c>E8</c>) **or** <c>JMP rel32</c> (<c>E9</c>)
    /// trampoline that may have been planted by a previous hook.
    /// </summary>
    private static unsafe nuint UnwrapNearBranch(nuint addr)
    {
        byte op = Marshal.ReadByte((IntPtr)addr);
        if (op != 0xE8 && op != 0xE9)           // not CALL/JMP rel32
            return addr;

        int rel = Marshal.ReadInt32((IntPtr)(addr + 1));
        long dst = (long)addr + 5 + rel;        // 5-byte instruction
        return (nuint)dst;
    }
}
