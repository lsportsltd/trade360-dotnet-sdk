using System;
using System.Linq;
using Trade360SDK.CustomersApi.Entities.MetadataApi.Requests;

namespace Trade360SDK.CustomersApi.Validators
{
    public static class GetTranslationsRequestValidator
    {
        public static void Validate(GetTranslationsRequest request)
        {
            // Ensure Languages is filled
            if (request.Languages == null || !request.Languages.Any())
            {
                throw new ArgumentException("Languages must be filled.");
            }

            // Ensure no invalid language values (e.g., 0 or negative)
            if (request.Languages.Any(lang => lang <= 0))
            {
                throw new ArgumentException("Languages cannot contain null, empty, or whitespace values");
            }

            // Ensure at least one of the other fields is filled
            if ((request.SportIds == null || !request.SportIds.Any()) &&
                (request.LocationIds == null || !request.LocationIds.Any()) &&
                (request.LeagueIds == null || !request.LeagueIds.Any()) &&
                (request.MarketIds == null || !request.MarketIds.Any()) &&
                (request.ParticipantIds == null || !request.ParticipantIds.Any()))
            {
                throw new ArgumentException("At least one of SportIds, LocationIds, LeagueIds, MarketIds, or ParticipantIds must be filled.");
            }
        }
    }
}
