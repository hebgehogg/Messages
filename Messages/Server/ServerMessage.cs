using ProtoBuf;

namespace Messages.Server
{
    [ProtoContract]
    [ProtoInclude(8, typeof(SuccessfulMessage))]
    [ProtoInclude(9, typeof(SessionKeyMessage))]
    [ProtoInclude(10, typeof(ErrorMessage))]
    [ProtoInclude(11, typeof(ConfigListMessage))]
    public abstract class ServerMessage:Message
    {
    }
}