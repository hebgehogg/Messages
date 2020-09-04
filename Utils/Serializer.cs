using System;
using System.IO;
using Messages;

namespace Utils
{
    public static class Serializer
    {
        public static byte[] Serialize(Message message)
        {
            using (var ms = new MemoryStream()) {
                ProtoBuf.Serializer.Serialize(ms, message);
                return ms.ToArray();
            }
        }

        public static Message DeSerialize(byte[] bytes)
        {
            Message message;
            using (var ms = new MemoryStream(bytes)) {
                message = ProtoBuf.Serializer.Deserialize<Message>(ms);
                return message;
            }
        }
    }
}