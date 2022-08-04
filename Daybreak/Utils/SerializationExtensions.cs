using System.Text;

namespace Daybreak.Utils
{
    public static class SerializationExtensions
    {
        public static byte[] AsBytes(this string s)
        {
            return Encoding.UTF8.GetBytes(s);
        }

        public static string AsString(this byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
