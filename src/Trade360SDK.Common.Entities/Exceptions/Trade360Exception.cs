using System;
using System.Collections.Generic;
using System.Linq;

namespace Trade360SDK.SnapshotApi
{
    public class Trade360Exception : Exception
    {
        public IEnumerable<string?>? Errors { get; }

        public Trade360Exception(IEnumerable<string?>? errors)
        {
            Errors = errors;
        }

        public Trade360Exception(string message, IEnumerable<string?>? errors) : base(message)
        {
            Errors = errors;
        }

        public Trade360Exception(string message, string rawErrorResponse) : base($"{message}: {rawErrorResponse}")
        {
        }

        public Trade360Exception(string message, Exception innerException) : base(message, innerException)
        {
        }

        public override string Message => Errors != null && Errors.Any()
            ? string.Join("; ", Errors)
            : base.Message;
    }
}