using System;
using Messages.Client;
using Messages.Server;
using ProtoBuf;

namespace Messages
{
    [ProtoContract]
    [ProtoInclude(1, typeof(ClientMessage))]
    [ProtoInclude(2, typeof(ServerMessage))]
    public abstract class Message
    {
        
    }
}