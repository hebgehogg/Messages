using ProtoBuf;

namespace Messages.Client
{
    [ProtoContract]
    public sealed class LogOutMessage: ClientMessage
    {
        [ProtoMember(1)]
        public string Login { get; set; }
    }
}