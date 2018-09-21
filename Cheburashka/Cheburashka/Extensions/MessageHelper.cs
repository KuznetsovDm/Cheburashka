using Cheburashka.BE;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace Cheburashka.Extensions
{
    public static class MessageHelper
    {
        public static async Task<byte[]> GetBytes(this BaseMessage message)
        {
            return await Task.Run(() =>
            {
                using (var stream = new MemoryStream())
                {
                    var formatter = new BinaryFormatter();

                    formatter.Serialize(stream, message);
                    return stream.GetBuffer();
                }
            });
        }

        public static async Task<BaseMessage> GetMessage(this Stream stream)
        {
            return await Task.Run(() =>
            {
                var formatter = new BinaryFormatter();
                return (BaseMessage)formatter.Deserialize(stream);
            });
        }

        public static BaseMessage GetMessage(byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            {
                var formatter = new BinaryFormatter();
                return (BaseMessage)formatter.Deserialize(stream);
            }
        }
    }
}
