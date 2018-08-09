using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace SHLibrary.Utils
{
    public class BinarySerializer
    {
        public static byte[] Serialize(object obj)
        {
            byte[] result;

            if (obj != null)
            {
                using (var stream = new MemoryStream())
                {
                    new BinaryFormatter().Serialize(stream, obj);
                    result = stream.ToArray();
                }
            }
            else
            {
                result = new byte[0];
            }

            return result;
        }

        public static object Deserialize(byte[] bytes)
        {
            object result;

            using (var stream = new MemoryStream { Position = 0 })
            {
                stream.Write(bytes, 0, bytes.Length);
                try
                {
                    stream.Position = 0;
                    object obj = new BinaryFormatter().Deserialize(stream);
                    result = obj;
                }
                catch
                {
                    result = null;
                }
            }

            return result;
        }

        public static TResult Deserialize<TResult>(byte[] bytes)
        {
            TResult result;

            using (var stream = new MemoryStream { Position = 0 })
            {
                stream.Write(bytes, 0, bytes.Length);
                try
                {
                    stream.Position = 0;
                    var obj = (TResult)new BinaryFormatter().Deserialize(stream);
                    result = obj;
                }
                catch
                {
                    result = default(TResult);
                }
            }

            return result;
        }

        public static string ToBase64String(object obj, out int bytesAdded)
        {
            string result;

            if (obj != null)
            {
                byte[] bytes;
                using (var stream = new MemoryStream())
                {
                    var formatter = new BinaryFormatter();
                    formatter.Serialize(stream, obj);
                    stream.Position = 0;
                    bytes = new byte[stream.Length];
                    stream.Read(bytes, 0, bytes.Length);
                }

                int risidue = bytes.Length % 4;
                bytesAdded = 4 - risidue;

                var resultBytes = new byte[bytes.Length + bytesAdded];
                Array.Copy(bytes, resultBytes, bytes.Length);
                result = Convert.ToBase64String(resultBytes);
            }
            else
            {
                bytesAdded = 0;
                result = string.Empty;
            }

            return result;
        }

        public static TResult FromBase64String<TResult>(string obj, int bytesAdded) where TResult : class
        {
            TResult result;

            byte[] bytes = Convert.FromBase64String(obj);
            int resultBytesLength = bytes.Length - bytesAdded;
            var resultBytes = new byte[resultBytesLength];
            Array.Copy(bytes, resultBytes, resultBytesLength);

            using (var stream = new MemoryStream { Position = 0 })
            {
                stream.Write(resultBytes, 0, resultBytesLength);
                var formatter = new BinaryFormatter();
                try
                {
                    stream.Position = 0;
                    result = (TResult)formatter.Deserialize(stream);
                }
                catch
                {
                    result = null;
                }
            }

            return result;
        }

        public static object FromBase64String(string obj, int bytesAdded)
        {
            return FromBase64String<object>(obj, bytesAdded);
        }
    }
}
