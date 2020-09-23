using System;
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
        private static IDatabaseProvider _sqLiteDataBaseProvider;

        public Root()
        {
            _sqLiteDataBaseProvider = new SqLiteDataBaseProvider(new SessionKeyManager());
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
                    int bytes = 0;
                    byte[] data = new byte[256];
 
                    do
                    {
                        bytes = connection.Receive(data);
                    }
                    while (connection.Available>0);

                    var deCompressData = ByteCompressor.DeCompress(data);
                    var deSerializedData = Utils.Serializer.DeSerialize(deCompressData);
                    Message returnMessage = default;
                    try
                    {
                        switch (deSerializedData)
                        {
                            case RegistrationMessage registrationMessage:
                                try
                                {
                                    _sqLiteDataBaseProvider.Registration(registrationMessage.Login);
                                    returnMessage = new SuccessfulMessage();
                                }
                                catch (ExistingUserException)
                                {
                                    returnMessage = new ErrorMessage() {Error = "User already exist"};
                                }
                                break;
                            case GetListOfConfigByPeriodMessage getListOfConfigByPeriodMessage:
                                var configs = _sqLiteDataBaseProvider.GetListOfConfig(getListOfConfigByPeriodMessage.SessionKey,
                                    getListOfConfigByPeriodMessage.From, getListOfConfigByPeriodMessage.To);
                                returnMessage = new ConfigListMessage(){Configs=configs};
                                break;
                            case LogOutMessage logOutMessage:
                                var logOut = _sqLiteDataBaseProvider.LogOut(logOutMessage.SessionKey);
                                if(logOut)
                                    returnMessage = new SuccessfulMessage();
                                else
                                    returnMessage = new ErrorMessage();
                                break;
                            case SaveConfigMessage saveConfigMessage:
                                var saveConfig = _sqLiteDataBaseProvider.SaveConfig(saveConfigMessage.SessionKey,
                                    saveConfigMessage.Config);
                                if(saveConfig)
                                    returnMessage = new SuccessfulMessage();
                                else
                                    returnMessage = new ErrorMessage();
                                break;
                            case SIgnInMessage signInMessage:
                                
                                var sessionKey =_sqLiteDataBaseProvider.SignIn(signInMessage.Login);
                                try
                                {
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