using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Client.Annotations;
using Messages;
using Messages.Client;
using Messages.Server;
using Utils;

namespace Client.Services
{
    public class ServerApiService
    {
        private byte[] GetCompressMessage(Message message)
        {
            var serializedMessage = Serializer.Serialize(message);
            var compressMessage = ByteCompressor.Compress(serializedMessage);

            return compressMessage;
        }
        
        private Message GetDeSerializedData(byte[] message)
        {
            var deCompressData = ByteCompressor.DeCompress(message);
            var deSerializedData = Serializer.DeSerialize(deCompressData);

            return deSerializedData;
        }
        
        public async Task<bool> RegisterAsync([NotNull] string login, CancellationToken token = default)
        {
            if (login == null) throw new ArgumentNullException(nameof(login));
            var server = "127.0.0.1";

            var message = new RegistrationMessage() {Login = login};
            var socet =new TcpClient( $"{server}",33333 );
            var networkStream = socet.GetStream();
            await networkStream.WriteAsync(GetCompressMessage(message),token).ConfigureAwait(false);

            while (!networkStream.DataAvailable)
            {
                await Task.Delay(1);
            }

            var buffer = new byte[1024];
            var result = await networkStream.ReadAsync(buffer, 0, buffer.Length);
            var counter = buffer.Take(result).ToArray();
            
            var deCompressData = ByteCompressor.DeCompress(counter);
            var deSerializedData = Serializer.DeSerialize(deCompressData);
            
            switch (deSerializedData)
            {
                case ErrorMessage errorMessage:
                    MessageBox.Show(errorMessage.Error);
                    return false;
                case SuccessfulMessage successfulMessage:
                    MessageBox.Show(successfulMessage.ToString());
                    return true;
                default:
                    throw new Exception();
            }
        }

        private void LogIn([NotNull] string login)
        {
            if (login == null) throw new ArgumentNullException(nameof(login));
            
        }
        
        private void LogOut()
        {
            
        }
        
        public static Task<int> SendTaskAsync(Socket socket, byte[] buffer, int offset, int size, SocketFlags flags)
        {
            AsyncCallback nullOp = (i) => {};
            IAsyncResult result = socket.BeginSend(buffer, offset, size, flags, nullOp, socket);
            // Use overload that takes an IAsyncResult directly
            return Task.Factory.FromAsync(result, socket.EndSend);
        }
    }
}