using System.Collections;
using System.Runtime.InteropServices;

namespace Daybreak.API.Interop.GuildWars;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
[GWCAEquivalent("Array")]
public readonly unsafe struct GuildWarsArray<T> : IEnumerable<T>
    where T : unmanaged
{
    public readonly T* Buffer;
    public readonly uint Capacity;
    public readonly uint Size;
    public readonly uint Param;

    /// <summary>
    /// Upper bound on a believable element count. Guild Wars' largest arrays hold at most a
    /// few thousand entries, so anything beyond this is uninitialised or misresolved memory
    /// rather than a real array.
    /// </summary>
    private const uint MaxPlausibleCapacity = 0x10000;

    /// <summary>
    /// Mirrors <c>GW::BaseArray::valid()</c> - the buffer must be null or aligned and the size
    /// must fit within the capacity - plus a bound on the capacity itself. Guild Wars leaves
    /// these fields transiently inconsistent while it (re)allocates an array, and a
    /// misresolved GWCA scan can point this struct at arbitrary bytes that still satisfy
    /// <c>size &lt;= capacity</c>.
    /// </summary>
    public bool IsValid =>
        (this.Buffer is null || ((nuint)this.Buffer & 0x3) == 0) &&
        this.Size <= this.Capacity &&
        this.Capacity <= MaxPlausibleCapacity;

    /// <summary>
    /// Number of elements that can safely be read. Zero whenever the array is not backed by
    /// a buffer, matching <c>GW::BaseArray::get()</c>, which returns null instead of
    /// dereferencing. Reading past this is an access violation, and because NativeAOT cannot
    /// throw <c>AccessViolationException</c> it fail-fasts and takes Guild Wars down with it.
    /// </summary>
    public uint Count => this.Buffer is not null && this.IsValid ? this.Size : 0;

    public T this[int index]
    {
        get
        {
            ArgumentOutOfRangeException.ThrowIfNegative(index);
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual((uint)index, this.Count);
            return this.Buffer[index];
        }
    }

    public Enumerator GetEnumerator() => new(this.Buffer, this.Count);

    IEnumerator<T> IEnumerable<T>.GetEnumerator() => this.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

    public unsafe struct Enumerator : IEnumerator, IEnumerator<T>
    {
        private readonly T* buffer;
        private readonly uint size;
        private int index;

        internal Enumerator(T* buffer, uint size)
        {
            this.buffer = buffer;
            this.size = size;
            this.index = -1;
        }

        public bool MoveNext()
        {
            int next = this.index + 1;
            if (next >= this.size)
            {
                return false;
            }

            this.index = next;
            return true;
        }

        public T Current => this.buffer[this.index];
        object IEnumerator.Current => this.Current;

        public void Reset() => this.index = -1;
        public readonly void Dispose() { }
    }
}
