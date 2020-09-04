using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Messages.Client;
using Utils;

namespace Server
{
    public sealed class Root
    {
        static void Main(string[] args)
        {
            IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 33333);

            Socket listenSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                listenSocket.Bind(ipPoint);
 
                listenSocket.Listen(10);

                while (true)
                {
                    Socket connection = listenSocket.Accept();
                    int bytes = 0;
                    byte[] data = new byte[256];
 
                    do
                    {
                        bytes = connection.Receive(data);
                    }
                    while (connection.Available>0);

                    var deCompressData = ByteCompressor.DeCompress(data);
                    var deSerializedData = Utils.Serializer.DeSerialize(data);
                    switch (deSerializedData)
                    {
                        case AuthorizeMessage authorizeMessage:
                            break;
                        case GetListOfConfigByPeriodMessage getListOfConfigByPeriodMessage:
                            break;
                        case LogOutMessage logOutMessage:
                            break;
                        case SaveConfigMessage saveConfigMessage:
                            break;
                        case SIgnInMessage sIgnInMessage:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(deSerializedData));
                    }


                    connection.Shutdown(SocketShutdown.Both);
                    connection.Close();
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}