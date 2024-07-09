using System;
using System.Collections.Generic;
using System.Linq;

namespace Trade360SDK.Api.Abstraction
{
    public class Trade360Exception : Exception
    {
        public IEnumerable<string?>? Errors { get; }

        public Trade360Exception(IEnumerable<string?>? errors)
        {
            Errors = errors;
        }

        public override string Message => Errors != null && Errors.Any()
            ? string.Join("; ", Errors)
            : "An error occurred in Trade360 API.";
    }
}