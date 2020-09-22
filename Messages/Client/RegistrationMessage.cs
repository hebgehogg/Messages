using ProtoBuf;

namespace Messages.Client
{
    [ProtoContract]
    [ProtoInclude(1, typeof(ClientMessage))]
    public sealed class RegistrationMessage: ClientMessage
    {
        [ProtoMember(1)]
        public string Login { get; set; }
    }
}