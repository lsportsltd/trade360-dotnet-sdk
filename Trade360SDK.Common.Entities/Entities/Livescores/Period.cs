using System.Collections.Generic;

namespace Trade360SDK.Common.Entities.Livescores
{
    public class Period
    {
        public int Type { get; set; }

        public bool IsFinished { get; set; }

        public bool IsConfirmed { get; set; }

        public IEnumerable<Result>? Results { get; set; }

        public IEnumerable<Incident>? Incidents { get; set; }

        public IEnumerable<Period>? SubPeriods { get; set; }

        public int SequenceNumber { get; set; }
    }
}
