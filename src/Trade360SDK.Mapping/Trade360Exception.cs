using System;
using System.Collections.Generic;

namespace Trade360SDK.Common
{
    public class Trade360Exception : Exception
    {
        public IEnumerable<string?>? Errors { get; }

        public Trade360Exception(IEnumerable<string?>? errors)
        {
            Errors = errors;
        }
    }
}
