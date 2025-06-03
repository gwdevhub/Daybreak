using System.Text;

namespace Daybreak.Shared.Services.BuildTemplates.Models;

internal sealed class EncodeCharStream
{
    private readonly StringBuilder innerStringBuilder = new();

    public void Write(int value, int count)
    {
        this.EncodeToBinary(value, count);
    }

    public string GetEncodedString()
    {
        return this.innerStringBuilder.ToString();
    }

    private void EncodeToBinary(int value, int count)
    {
        while(value > 0 && count > 0)
        {
            this.innerStringBuilder.Append(value % 2 == 1 ? '1' : '0');
            value /= 2;
            count--;
        }

        while (count > 0)
        {
            this.innerStringBuilder.Append('0');
            count--;
        }
    }
}
