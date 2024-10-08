﻿using Trade360SDK.Common.Attributes;

namespace Trade360SDK.Common.Entities.MessageTypes
{
    [Trade360Entity(31)]
    public class KeepAliveUpdate : MessageUpdate
    {
        public KeepAlive.KeepAlive? KeepAlive { get; set; }
    }
}
