using Trade360SDK.Common.Entities.Shared;

namespace Trade360SDK.Common.Entities.Fixtures
{
    public class State
    {
        public int StateId { get; set; }
        public string Name { get; set; }
        public IdNamePair? Country { get; set; }
    }
}