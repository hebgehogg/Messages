using System;
using ProtoBuf;

namespace Messages.Client
{
    [ProtoContract]
    public sealed class GetListOfConfigByPeriodMessage: ClientMessage
    {
        [ProtoMember(1)]
        public DateTimeOffset From { get; set; }
        [ProtoMember(2)]
        public DateTimeOffset To { get; set; }
    }
}