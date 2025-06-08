using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Daybreak.API.Interop;

public abstract class GWFastCall(GWAddressCache target)
{
    public readonly struct Void { }

    public GWAddressCache Cache { get; } = target;
    public abstract void Initialize();

    protected unsafe static nint BuildFastCallThunk(nuint target)
    {
        if (target is 0)
        {
            return 0;
        }

        var code = stackalloc byte[13]
        {
            0x58,                      // pop eax               (ret)
            0x59,                      // pop ecx               (ctx)
            0x5A,                      // pop edx               (edxVal)
            0x5B,                      // pop ebx               (wparam)
            0x53,                      // push ebx              (wparam)
            0x50,                      // push eax              (ret)
            0xB8,0,0,0,0,              // mov  eax, TARGET
            0xFF,0xE0                  // jmp  eax
        };                             // 13 bytes total

        const int TARGET_OFFSET = 7;
        Unsafe.WriteUnaligned(ref code[TARGET_OFFSET], (uint)target);

        var mem = NativeMethods.VirtualAlloc(0, 13, NativeMethods.MEM_COMMIT | NativeMethods.MEM_RESERVE, NativeMethods.PAGE_EXECUTE_READWRITE);
        if (mem is 0)
        {
            throw new Win32Exception(Marshal.GetLastWin32Error(), "VirtualAlloc");
        }

        Buffer.MemoryCopy(code, (void*)mem, 13, 13);
        return mem;
    }
}

public unsafe sealed class GWFastCall<T1, T2, T3>(GWAddressCache target) : GWFastCall(target)
{
    private delegate* unmanaged[Stdcall]<T1, T2, T3> func = null;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe T3? Invoke(T1 t1, T2 t2)
    {
        if (this.func is null)
        {
            this.Initialize();
        }

        if (this.func is not null)
        {
            if (typeof(T3) == typeof(Void))
            {
                ((delegate* unmanaged[Stdcall]<T1, T2, void>)this.func)(t1, t2);
                return default;
            }

            return this.func(t1, t2);
        }

        return default;
    }

    public override void Initialize()
    {
        if (this.Cache.GetAddress() is not nuint addr ||
            addr is 0)
        {
            return;
        }

        var call = BuildFastCallThunk(addr);
        this.func = (delegate* unmanaged[Stdcall]<T1, T2, T3>)call;
    }
}

public unsafe sealed class GWFastCall<T1, T2, T3, T4>(GWAddressCache target) : GWFastCall(target)
{
    private delegate* unmanaged[Stdcall]<T1, T2, T3, T4> func = null;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe T4? Invoke(T1 t1, T2 t2, T3 t3)
    {
        if (this.func is null)
        {
            this.Initialize();
        }

        if (this.func is not null)
        {
            if (typeof(T4) == typeof(Void))
            {
                ((delegate* unmanaged[Stdcall]<T1, T2, T3, void>)this.func)(t1, t2, t3);
                return default;
            }

            return this.func(t1, t2, t3);
        }

        return default;
    }

    public override void Initialize()
    {
        if (this.Cache.GetAddress() is not nuint addr ||
            addr is 0)
        {
            return;
        }

        var call = BuildFastCallThunk(addr);
        this.func = (delegate* unmanaged[Stdcall]<T1, T2, T3, T4>)call;
    }
}

public unsafe sealed class GWFastCall<T1, T2, T3, T4, T5>(GWAddressCache target) : GWFastCall(target)
{
    private delegate* unmanaged[Stdcall]<T1, T2, T3, T4, T5> func = null;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe T5? Invoke(T1 t1, T2 t2, T3 t3, T4 t4)
    {
        if (this.func is null)
        {
            this.Initialize();
        }

        if (this.func is not null)
        {
            if (typeof(T5) == typeof(Void))
            {
                ((delegate* unmanaged[Stdcall]<T1, T2, T3, T4, void>)this.func)(t1, t2, t3, t4);
                return default;
            }

            return this.func(t1, t2, t3, t4);
        }

        return default;
    }

    public override void Initialize()
    {
        if (this.Cache.GetAddress() is not nuint addr ||
            addr is 0)
        {
            return;
        }

        var call = BuildFastCallThunk(addr);
        this.func = (delegate* unmanaged[Stdcall]<T1, T2, T3, T4, T5>)call;
    }
}

public unsafe sealed class GWFastCall<T1, T2, T3, T4, T5, T6>(GWAddressCache target) : GWFastCall(target)
{
    private delegate* unmanaged[Stdcall]<T1, T2, T3, T4, T5, T6> func = null;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe T6? Invoke(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
    {
        if (this.func is null)
        {
            this.Initialize();
        }

        if (this.func is not null)
        {
            if (typeof(T6) == typeof(Void))
            {
                ((delegate* unmanaged[Stdcall]<T1, T2, T3, T4, T5, void>)this.func)(t1, t2, t3, t4, t5);
                return default;
            }

            return this.func(t1, t2, t3, t4, t5);
        }

        return default;
    }

    public override void Initialize()
    {
        if (this.Cache.GetAddress() is not nuint addr ||
            addr is 0)
        {
            return;
        }

        var call = BuildFastCallThunk(addr);
        this.func = (delegate* unmanaged[Stdcall]<T1, T2, T3, T4, T5, T6>)call;
    }
}

public unsafe sealed class GWFastCall<T1, T2, T3, T4, T5, T6, T7>(GWAddressCache target) : GWFastCall(target)
{
    private delegate* unmanaged[Stdcall]<T1, T2, T3, T4, T5, T6, T7> func = null;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public unsafe T7? Invoke(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
    {
        if (this.func is null)
        {
            this.Initialize();
        }

        if (this.func is not null)
        {
            if (typeof(T7) == typeof(Void))
            {
                ((delegate* unmanaged[Stdcall]<T1, T2, T3, T4, T5, T6, void>)this.func)(t1, t2, t3, t4, t5, t6);
                return default;
            }

            return this.func(t1, t2, t3, t4, t5, t6);
        }

        return default;
    }

    public override void Initialize()
    {
        if (this.Cache.GetAddress() is not nuint addr ||
            addr is 0)
        {
            return;
        }

        var call = BuildFastCallThunk(addr);
        this.func = (delegate* unmanaged[Stdcall]<T1, T2, T3, T4, T5, T6, T7>)call;
    }
}
