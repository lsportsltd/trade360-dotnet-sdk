﻿using System.Collections.Generic;

namespace Trade360SDK.CustomersApi.Entities.MetadataApi.Responses
{
    public class TranslationResponse
    {
        public Dictionary<string, List<LocalizedValue>>? Sports { get; set; }
        public Dictionary<string, List<LocalizedValue>>? Leagues { get; set; }
        public Dictionary<string, List<LocalizedValue>>? Locations { get; set; }
        public Dictionary<string, List<LocalizedValue>>? Markets { get; set; }
        public Dictionary<string, List<LocalizedValue>>? Participants { get; set; }
    }
     
    public class LocalizedValue
    {
        public int LanguageId { get; set; }
        public string? Value { get; set; }
    }
}
