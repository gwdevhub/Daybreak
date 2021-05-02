using Newtonsoft.Json;
using System.Text;

namespace Daybreak.Utils
{
    public static class SerializationExtensions
    {
        public static string Serialize<T>(this T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T Deserialize<T>(this string serializedObject)
        {
            return JsonConvert.DeserializeObject<T>(serializedObject);
        }

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
