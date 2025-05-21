using System.Collections;
using System.Runtime.InteropServices;

namespace Daybreak.API.Interop.GuildWars;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct AttributeContext
{
    public readonly uint Id;
    public readonly uint LevelBase;
    public readonly uint Level;
    public readonly uint DecrementPoints;
    public readonly uint IncrementPoints;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct AttributeInfo
{
    public readonly uint ProfessionId;
    public readonly uint AttributeId;
    public readonly uint NameId;
    public readonly uint DescriptionId;
    public readonly uint IsPve;
}

[StructLayout(LayoutKind.Sequential, Pack = 1)]
public readonly struct PartyAttribute
{
    public readonly uint AgentId;
    public readonly PartyAttributeArray Attributes;
}

// Kind of ugly but there's no easier way to make up a static sized array of a complex type
[StructLayout(LayoutKind.Sequential, Pack = 1, Size = 0x43C)]
public readonly unsafe struct PartyAttributeArray : IEnumerable<AttributeContext>
{
    private readonly AttributeContext baseAddr;

    public readonly int Length => 54;

    public AttributeContext this[int index]
    {
        get
        {
            if (index < 0 || index >= this.Length)
            {
                throw new IndexOutOfRangeException($"Index {index} is out of range");
            }

            fixed (AttributeContext* basePtr = &this.baseAddr)
            {
                return basePtr[index];
            }
        }
    }

    public ReadOnlySpan<AttributeContext> AsSpan()
    {
        fixed (AttributeContext* basePtr = &this.baseAddr)
        {
            return new ReadOnlySpan<AttributeContext>(basePtr, this.Length);
        }
    }

    public IEnumerator<AttributeContext> GetEnumerator()
    {
        fixed (AttributeContext* basePtr = &this.baseAddr)
        {
            return new Enumerator(basePtr, this.Length);
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    public struct Enumerator(
        AttributeContext* basePtr,
        int length)
        : IEnumerator, IEnumerator<AttributeContext>
    {
        private readonly AttributeContext* basePtr = basePtr;
        private readonly int length = length;

        private int index = 0;

        public AttributeContext Current => this.basePtr[this.index];
        object IEnumerator.Current => this.Current;

        public bool MoveNext()
        {
            if (this.index < this.length)
            {
                this.index++;
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Reset()
        {
            this.index = 0;
        }

        public readonly void Dispose()
        {
        }
    }
}
