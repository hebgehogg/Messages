using System;
using ProtoBuf;

namespace Messages.Client
{
    [ProtoContract]
    public class GetListOfConfigByPeriodMessage: ClientMessage
    {
        [ProtoMember(1)]
        public DateTime From { get; set; }
        [ProtoMember(2)]
        public DateTime To { get; set; }
    }
}