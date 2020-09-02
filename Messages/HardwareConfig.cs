using ProtoBuf;

namespace Messages
{
    [ProtoContract]
    public sealed class HardwareConfig
    {
        [ProtoMember(1)]
        public int CpuCoreCount { get; set; }
        [ProtoMember(2)]
        public double ClockFrequency { get; set; }
        [ProtoMember(3)]
        public double RandomAccessMemory { get; set; }
        [ProtoMember(4)]
        public double DiskSpace { get; set; }
    }
}