using System;

namespace Trade360SDK.Feed.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class Trade360EntityAttribute : Attribute
    {
        public int EntityKey { get; }

        public Trade360EntityAttribute(int entityKey)
        {
            EntityKey = entityKey;
        }
    }
}
