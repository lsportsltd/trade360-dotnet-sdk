using System.Collections.Generic;
using Trade360SDK.Api.Common.Models.Requests.Base;

namespace Trade360SDK.Common.Metadata.Requests
{
    public class GetTranslationsRequest : BaseRequest
    {
        public IEnumerable<int> SportIds { get; set; }
        public IEnumerable<int> LocationIds { get; set; }
        public IEnumerable<int> LeagueIds { get; set; }
        public IEnumerable<int> MarketIds { get; set; }
        public IEnumerable<int> ParticipantIds { get; set; }
        public IEnumerable<int> Languages { get; set; }
    }
}
