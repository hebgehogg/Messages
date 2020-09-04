using System.IO;
using System.IO.Compression;

namespace Utils
{
    public static class ByteCompressor
    {
        public static byte[] Compress(byte[] data)
        {
            using (var ms = new MemoryStream(data))
            {
                GZipStream compressingStream = new GZipStream(ms,
                    CompressionMode.Compress);
                var bytes = new byte[2048];
                int bytesRead;
                while ((bytesRead = ms.Read(bytes, 0, bytes.Length)) != 0)
                {
                    compressingStream.Write(bytes, 0, bytesRead);
                }
                compressingStream.Close();
                return ms.ToArray();
            }
        }
        

        public static byte[] DeCompress(byte[] bytes)
        {
            using (var ms = new MemoryStream(bytes))
            {

                GZipStream decompressingStream = new GZipStream(ms,
                    CompressionMode.Decompress);
                int byteRead;
                while ((byteRead = decompressingStream.ReadByte()) != -1)
                {
                    decompressingStream.WriteByte((byte) byteRead);
                }

                decompressingStream.Close();
                return ms.ToArray();
            }
        }
    }
}