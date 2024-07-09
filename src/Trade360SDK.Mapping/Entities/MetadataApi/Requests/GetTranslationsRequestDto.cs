using System.Collections.Generic;
using System.Linq;

namespace Trade360SDK.CustomersApi.Entities.MetadataApi.Requests
{
    public class GetTranslationsRequestDto
    {
        public IEnumerable<int> SportIds { get; set; } = Enumerable.Empty<int>();
        public IEnumerable<int> LocationIds { get; set; } = Enumerable.Empty<int>();
        public IEnumerable<int> LeagueIds { get; set; } = Enumerable.Empty<int>();
        public IEnumerable<int> MarketIds { get; set; } = Enumerable.Empty<int>();
        public IEnumerable<int> ParticipantIds { get; set; } = Enumerable.Empty<int>();
        public IEnumerable<int> Languages { get; set; }
    }
}
