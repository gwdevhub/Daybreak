using System.Text;

namespace Daybreak.Scanner.Utils;

public static class FixedByteArrayExtensions
{
    public static string DecodeASCII(this byte[] buffer)
    {
        int count = Array.IndexOf<byte>(buffer, 0, 0);
        if (count < 0)
        {
            count = buffer.Length;
        }

        return Encoding.ASCII.GetString(buffer, 0, count);
    }
}
