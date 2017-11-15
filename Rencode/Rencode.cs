using System;
using System.IO;

namespace SilverCard.Rencode
{
    public class Rencode
    {
        public static Object Decode(byte[] data, int index, int count)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));

            using (MemoryStream i = new MemoryStream(data, index, count))
            using (Decoder decoder = new Decoder(i))
            {
                return decoder.ReadObject();
            }
        }

        public static Object Decode(byte[] data)
        {
            return Decode(data, 0, data.Length);
        }

        public static byte[] Encode(Object obj)
        {
            using (MemoryStream i = new MemoryStream())
            using (Encoder encoder = new Encoder(i))
            {
                encoder.WriteObject(obj);
                return i.ToArray();
            }
        }

    }
}
