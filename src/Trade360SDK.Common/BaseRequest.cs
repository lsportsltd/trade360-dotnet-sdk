﻿namespace Trade360SDK.Api.Abstraction
{
    public class BaseRequest
    {
        public int PackageId { get; set; }
        public string? UserName { get; set; }
        public string? Password { get; set; }
    }
}
