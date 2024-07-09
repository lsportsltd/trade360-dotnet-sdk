﻿namespace Trade360SDK.Api.Abstraction.MetadataApi
{
    internal class Response<TBody>
        where TBody : class
    {
        public Header? Header { get; set; }
        public TBody? Body { get; set; }
    }
}
