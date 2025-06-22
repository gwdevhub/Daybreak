namespace Daybreak.API.Interop.GuildWars;

public readonly unsafe struct WrappedPointer<T>(T* pointer)
    where T : unmanaged
{
    public readonly T* Pointer = pointer;

    public bool IsNull => this.Pointer is null;

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
