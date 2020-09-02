using ProtoBuf;

namespace Messages.Client
{
    [ProtoContract]
    public sealed class SIgnInMessage: ClientMessage
    {
        [ProtoMember(1)]
        public string Login { get; set; }
    }
}