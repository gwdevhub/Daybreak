using System.Collections;
using System.Runtime.InteropServices;

namespace Daybreak.API.Interop.GuildWars;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly unsafe struct GuildWarsArray<T> : IEnumerable<T>
    where T : unmanaged
{
    public readonly T* Buffer;
    public readonly uint Capacity;
    public readonly uint Size;
    public readonly uint Param;

    public T this[int index]
    {
        get
        {
            ArgumentOutOfRangeException.ThrowIfNegative(index);
            ArgumentOutOfRangeException.ThrowIfGreaterThanOrEqual((uint)index, this.Size);
            return this.Buffer[index];
        }
    }

    public Enumerator GetEnumerator() => new(this.Buffer, this.Size);

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
