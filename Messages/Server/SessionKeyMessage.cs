using ProtoBuf;

namespace Messages.Server
{
    [ProtoContract]
    public class SessionKeyMessage: ServerMessage
    {
        [ProtoMember(1)]
        public string SessionKey { get; set; }
    }
}