using ProtoBuf;

namespace Messages.Client
{
    [ProtoContract]
    public sealed class SaveConfigMessage: ClientMessage
    {
        [ProtoMember(1)]
        public HardwareConfig Config { get; set; }
    }
}