﻿using System.Collections.Generic;
using ProtoBuf;

namespace Messages.Server
{
    [ProtoContract]
    public class ConfigListMessage: ServerMessage
    {
        [ProtoMember(1)]
        public HardwareConfig[] Configs { get; set; }
    }
}