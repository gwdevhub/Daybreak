using System.Runtime.InteropServices;

namespace Daybreak.API.Interop;

public sealed class UnmanagedStruct<T> : IDisposable
    where T : struct
{
    public nint Address { get; private set; }

    public UnmanagedStruct(T value)
    {
        this.Address = Marshal.AllocHGlobal(Marshal.SizeOf<T>());
        Marshal.StructureToPtr(value, this.Address, false);
    }

    public void Dispose()
    {
        Marshal.DestroyStructure<T>(this.Address);
        Marshal.FreeHGlobal(this.Address);
        this.Address = 0x0;
    }
}
