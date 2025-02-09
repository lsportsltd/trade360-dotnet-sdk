﻿using System;

namespace Trade360SDK.Common.Models
{
    public class MessageHeader
    {
        public string? CreationDate { get; set; }
        public int Type { get; set; }
        public int? MsgSeq { get; set; }
        public string? MsgGuid { get; set; }
        public long? ServerTimestamp { get; set; }
        public DateTime? MessageBrokerTimestamp { get; set; }
        public DateTime? MessageTimestamp { get; set; }
    }
}
