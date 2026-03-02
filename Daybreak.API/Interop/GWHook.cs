using System.Runtime.InteropServices;
using MinHook;

namespace Daybreak.API.Interop;

/// <summary>
/// A thin convenience wrapper around MinHook that
/// resolves the target address lazily through <see cref="GWAddressCache"/>
/// unwraps an existing "CALL/JMP rel32" stub so we patch the real code
/// exposes <see cref="Original"/> so your detour can chain
/// </summary>
public sealed class GWHook<T>(
    GWAddressCache funcAddress,
    T detour,
    bool bypassPreviousHooks = false)
    where T : Delegate
{
    private const uint PAGE_EXECUTE_READWRITE = 0x40;

    private readonly bool bypassPreviousHooks = bypassPreviousHooks;
    private readonly GWAddressCache addressCache = funcAddress ?? throw new ArgumentNullException(nameof(funcAddress));
    private readonly T detour = detour ?? throw new ArgumentNullException(nameof(detour));
    private readonly SemaphoreSlim semaphore = new(1, 1);

    private T? cont;          // trampoline returned by MinHook, or previous hook's detour
    private HookEngine? engine;
    private bool usedJmpPatch;  // true if we patched an existing JMP instead of using MinHook
    private int originalRel32;  // saved rel32 for restoration when usedJmpPatch

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

            // If not bypassing previous hooks and target starts with JMP, use our JMP patching
            // This handles the case where GWCA already hooked the function
            if (!this.bypassPreviousHooks && StartsWithJmp(target))
            {
                if (this.TryPatchExistingJmp(target))
                {
                    return true;
                }
            }

            // Try MinHook for normal (unhooked) functions
            if (this.TryMinHook(target))
            {
                return true;
            }

            // Fallback: try JMP patching even if we didn't detect JMP initially
            // (in case StartsWithJmp was wrong or MinHook failed for other reasons)
            if (this.TryPatchExistingJmp(target))
            {
                return true;
            }

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
            if (!this.Hooked)
            {
                return;
            }

            if (this.usedJmpPatch)
            {
                // Restore the original JMP target
                RestoreJmpRel32(this.TargetAddress, this.originalRel32);
            }
            else if (this.engine is not null)
            {
                this.engine.DisableHooks();
                this.engine.Dispose();
            }

            this.cont = null;
            this.Hooked = false;
        }
        finally
        {
            this.semaphore.Release();
        }
    }

    private bool TryMinHook(nuint target)
    {
        try
        {
            this.engine = new HookEngine();
            this.cont = this.engine.CreateHook((nint)target, this.detour);
            this.engine.EnableHooks();
            this.TargetAddress = target;
            this.ContinueAddress = (nuint)this.cont.Method.MethodHandle.GetFunctionPointer();
            this.DetourAddress = (nuint)this.detour.Method.MethodHandle.GetFunctionPointer();
            this.Hooked = true;
            this.usedJmpPatch = false;
            return true;
        }
        catch
        {
            this.engine?.Dispose();
            this.engine = null;
            return false;
        }
    }

    /// <summary>
    /// If the target starts with JMP rel32 (0xE9), another hook is already installed.
    /// We patch that JMP to point to our detour, and call the previous hook's detour as Continue.
    /// </summary>
    private unsafe bool TryPatchExistingJmp(nuint target)
    {
        var p = (byte*)target;
        if (*p != 0xE9)
        {
            return false;
        }

        // Read the existing rel32 offset (points to previous hook's detour)
        var existingRel32 = *(int*)(p + 1);
        var previousDetourAddr = (nuint)(p + 5 + existingRel32);

        // Create a delegate to call the previous hook
        this.cont = Marshal.GetDelegateForFunctionPointer<T>((nint)previousDetourAddr);

        // Calculate new rel32 to point to our detour
        var ourDetourPtr = Marshal.GetFunctionPointerForDelegate(this.detour);
        var newRel32 = (int)((nint)ourDetourPtr - (nint)(p + 5));

        // Patch the JMP
        if (!PatchJmpRel32(target, newRel32))
        {
            this.cont = null;
            return false;
        }

        this.originalRel32 = existingRel32;
        this.TargetAddress = target;
        this.ContinueAddress = previousDetourAddr;
        this.DetourAddress = (nuint)ourDetourPtr;
        this.Hooked = true;
        this.usedJmpPatch = true;
        return true;
    }

    private static unsafe bool PatchJmpRel32(nuint jmpAddr, int newRel32)
    {
        var rel32Ptr = (byte*)(jmpAddr + 1);
        if (!NativeMethods.VirtualProtect(rel32Ptr, 4, PAGE_EXECUTE_READWRITE, out var oldProtect))
        {
            return false;
        }

        try
        {
            *(int*)rel32Ptr = newRel32;
            return true;
        }
        finally
        {
            NativeMethods.VirtualProtect(rel32Ptr, 4, oldProtect, out _);
        }
    }

    /// <summary>
    /// Checks if the target address starts with a JMP rel32 instruction (0xE9).
    /// </summary>
    private static unsafe bool StartsWithJmp(nuint addr)
    {
        var p = (byte*)addr;
        return *p == 0xE9;
    }

    private static unsafe void RestoreJmpRel32(nuint jmpAddr, int originalRel32)
    {
        var rel32Ptr = (byte*)(jmpAddr + 1);
        if (NativeMethods.VirtualProtect(rel32Ptr, 4, PAGE_EXECUTE_READWRITE, out var oldProtect))
        {
            *(int*)rel32Ptr = originalRel32;
            NativeMethods.VirtualProtect(rel32Ptr, 4, oldProtect, out _);
        }
    }

    /// <summary>
    /// Follows a <c>CALL rel32</c> (<c>E8</c>) **or** <c>JMP rel32</c> (<c>E9</c>)
    /// trampoline that may have been planted by a previous hook.
    /// </summary>
    private static unsafe nuint UnwrapNearBranch(nuint addr)
    {
        var p = (byte*)addr;
        if (*p != 0xE8 && *p != 0xE9)
            return addr;

        var rel = *(int*)(p + 1);
        return (nuint)(p + 5 + rel);
    }
}