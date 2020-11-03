using ProtoBuf;

namespace Messages.Server
{
    [ProtoContract]
    public class ErrorMessage: ServerMessage
    {
        [ProtoMember(1)]
        public string Error { get; set; }
    }
}