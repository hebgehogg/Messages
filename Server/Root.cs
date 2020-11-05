using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Messages;
using Messages.Client;
using Messages.Server;
using Server.Exceptions;
using Utils;

namespace Server
{
    public sealed class Root
    {
        private static readonly IDatabaseProvider SqLiteDataBaseProvider;

        static Root()
        {
            SqLiteDataBaseProvider = new SqLiteDataBaseProvider(new SessionKeyManager());
        }

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
                    List<byte> bytes = new List<byte>();
                    byte[] buffer = new byte[256];
 
                    do
                    {
                        var countBytes = connection.Receive(buffer);
                        var freeBytes = buffer.Take(countBytes);
                        bytes.AddRange(freeBytes);
                    }
                    while (connection.Available>0);

                    var deCompressData = ByteCompressor.DeCompress(bytes.ToArray());
                    var deSerializedData = Utils.Serializer.DeSerialize(deCompressData);
                    Message returnMessage = default;
                    try
                    {
                        switch (deSerializedData)
                        {
                            case RegistrationMessage registrationMessage:
                                Console.WriteLine(registrationMessage.ToString() + DateTime.Now);
                                try
                                {
                                    SqLiteDataBaseProvider.Registration(registrationMessage.Login);
                                    returnMessage = new SuccessfulMessage();
                                }
                                catch (ExistingUserException)
                                {
                                    returnMessage = new ErrorMessage() {Error = "User already exist"};
                                }
                                break;
                            case GetListOfConfigByPeriodMessage getListOfConfigByPeriodMessage:
                                Console.WriteLine(getListOfConfigByPeriodMessage.ToString() + DateTime.Now);
                                var configs = SqLiteDataBaseProvider.GetListOfConfig(getListOfConfigByPeriodMessage.SessionKey,
                                    getListOfConfigByPeriodMessage.From, getListOfConfigByPeriodMessage.To);
                                returnMessage = new ConfigListMessage(){Configs=configs};
                                break;
                            case LogOutMessage logOutMessage:
                                Console.WriteLine(logOutMessage.ToString() + DateTime.Now);
                                var logOut = SqLiteDataBaseProvider.LogOut(logOutMessage.SessionKey);
                                if(logOut)
                                    returnMessage = new SuccessfulMessage();
                                else
                                    returnMessage = new ErrorMessage();
                                break;
                            case SaveConfigMessage saveConfigMessage:
                                Console.WriteLine(saveConfigMessage.ToString() + DateTime.Now);
                                var saveConfig = SqLiteDataBaseProvider.SaveConfig(saveConfigMessage.SessionKey,
                                    saveConfigMessage.Config);
                                if(saveConfig)
                                    returnMessage = new SuccessfulMessage();
                                else
                                    returnMessage = new ErrorMessage();
                                break;
                            case LogInMessage logInMessage:
                                Console.WriteLine(logInMessage.ToString() + DateTime.Now);
                                try
                                {
                                    var sessionKey =SqLiteDataBaseProvider.LogIn(logInMessage.Login);
                                    returnMessage = new SessionKeyMessage() {SessionKey = sessionKey};
                                }
                                catch (NonExistExceptions)
                                {
                                    returnMessage = new ErrorMessage() {Error = "User don`t exist"};
                                }
                                break;
                            default:
                                throw new ArgumentOutOfRangeException(nameof(deSerializedData));
                        }
                    }
                    catch (Exception e)
                    {
                        returnMessage = new ErrorMessage(){Error = e.ToString()};
                    }
                    if(returnMessage==default) throw new Exception();

                    var serializedMessage = Serializer.Serialize(returnMessage);
                    var compressMessage = ByteCompressor.Compress(serializedMessage);

                    connection.Send(compressMessage);

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