namespace Daybreak.API.Interop.GuildWars;

public readonly unsafe struct WrappedPointer<T>(T* pointer)
    where T : unmanaged
{
    public readonly T* Pointer = pointer;

    public bool IsNull => this.Pointer is null;

    /// <summary>
    /// True when the pointer is non-null and plausibly dereferenceable.
    /// </summary>
    /// <remarks>
    /// Guild Wars structures are 4-byte aligned on x86, so a misaligned address is never a
    /// real structure. When a GWCA pattern scan fails to resolve, it can hand back an
    /// address inside Gw.exe's code section (for example 0x0048CA72); reading through that
    /// yields nonsense field values and eventually an access violation. NativeAOT cannot
    /// throw <c>AccessViolationException</c>, so it fail-fasts and terminates Guild Wars.
    /// </remarks>
    public bool IsValid => this.Pointer is not null && ((nuint)this.Pointer & 0x3) == 0;

    public static implicit operator WrappedPointer<T>(T* pointer) => new(pointer);

    public static implicit operator T*(WrappedPointer<T> wrappedPointer) => wrappedPointer.Pointer;

    public static bool operator ==(T* left, WrappedPointer<T> right) => left == right.Pointer;

    public static bool operator !=(T* left, WrappedPointer<T> right) => left != right.Pointer;

    public static bool operator ==(WrappedPointer<T> left, T* right) => left.Pointer == right;

    public static bool operator !=(WrappedPointer<T> left, T* right) => left.Pointer != right;

    public static bool operator ==(WrappedPointer<T> left, WrappedPointer<T> right) => left.Pointer == right.Pointer;

    public static bool operator !=(WrappedPointer<T> left, WrappedPointer<T> right) => left.Pointer != right.Pointer;

    public override int GetHashCode() => this.Pointer is null ? 0 : this.Pointer->GetHashCode();

    public override bool Equals(object? obj)
    {
        if (obj is WrappedPointer<T> wrappedPointer)
        {
            return this.Pointer == wrappedPointer.Pointer;
        }

        return false;
    }
}
