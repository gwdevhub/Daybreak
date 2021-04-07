using Newtonsoft.Json;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Daybreak.Utils
{
    public static class SerializationExtensions
    {
        public static byte[] SerializeBytes(this object obj)
        {
            byte[] returnArray;
            using (var memoryStream = new MemoryStream())
            {
                var bf = new BinaryFormatter();
                bf.Serialize(memoryStream, obj);
                returnArray = memoryStream.ToArray();
            }
            return returnArray;
        }

        public static T DeserializeBytes<T>(this byte[] serializedObject)
        {
            T obj;
            using (var memoryStream = new MemoryStream(serializedObject))
            {
                var bf = new BinaryFormatter();
                obj = (T)bf.Deserialize(memoryStream);
            }
            return obj;
        }

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
