﻿using System;
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
        
        private async Task<Message> GetDataAsync(string login, Message message, CancellationToken token = default)
        {
            var server = "127.0.0.1";
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
            
            var deSerializedData = GetDeSerializedData(counter);
            
            return deSerializedData;
        }
        
        public async Task<bool> RegisterAsync([NotNull] string login, CancellationToken token = default)
        {
            if (login == null) throw new ArgumentNullException(nameof(login));
            var message = new RegistrationMessage() {Login = login};
            var deSerializedData = await GetDataAsync(login,message, token);

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

        public async Task<bool> LogInAsync([NotNull] string login)
        {
            if (login == null) throw new ArgumentNullException(nameof(login));
            var message = new LogInMessage() {Login = login};
            var deSerializedData = await GetDataAsync(login,message);
            
            switch (deSerializedData)//error
            {
                case ErrorMessage errorMessage:
                    MessageBox.Show(errorMessage.Error);
                    return false;
                case SessionKeyMessage sessionKeyMessage:
                    MessageBox.Show(sessionKeyMessage.ToString());
                    return true;
                default:
                    throw new Exception();//this
            }
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