using ProtoBuf;

namespace Messages.Client
{
    [ProtoContract]
    public class LogInMessage: ClientMessage
    {
        [ProtoMember(1)]
        public string Login { get; set; }
    }
}