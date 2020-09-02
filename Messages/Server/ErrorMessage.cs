using ProtoBuf;

namespace Messages.Server
{
    [ProtoContract]
    public sealed class ErrorMessage: ServerMessage
    {
        [ProtoMember(1)]
        public string Error { get; set; }
    }
}