using ProtoBuf;

namespace Messages.Client
{
    [ProtoContract]
    [ProtoInclude(6, typeof(ClientMessage))]
    public class RegistrationMessage: ClientMessage
    {
        [ProtoMember(1)]
        public string Login { get; set; }
    }
}