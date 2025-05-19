namespace Daybreak.API.Interop;

public interface IHook<T>
    : IDisposable where T : Delegate
{
    public T Continue { get; }

    public nuint ContinueAddress { get; }
}
