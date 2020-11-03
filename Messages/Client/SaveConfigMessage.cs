using ProtoBuf;

namespace Messages.Client
{
    [ProtoContract]
    public class SaveConfigMessage: ClientMessage
    {
        [ProtoMember(1)]
        public HardwareConfig Config { get; set; }
    }
}