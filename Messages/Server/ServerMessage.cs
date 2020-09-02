using ProtoBuf;

namespace Messages.Server
{
    [ProtoContract]
    [ProtoInclude(1, typeof(SuccessfulMessage))]
    [ProtoInclude(2, typeof(SessionKeyMessage))]
    [ProtoInclude(3, typeof(ErrorMessage))]
    [ProtoInclude(4, typeof(ConfigListMessage))]
    public abstract class ServerMessage:Message
    {
    }
}