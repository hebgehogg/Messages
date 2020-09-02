using ProtoBuf;

namespace Messages.Server
{
    [ProtoContract]
    public sealed class SessionKeyMessage: ServerMessage
    {
        [ProtoMember(1)]
        public string SessionKey { get; set; }
    }
}