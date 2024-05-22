using System;

namespace Trade360SDK.Feed.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    internal class Trade360EntityAttribute : Attribute
    {
        private int _type;

        public Trade360EntityAttribute(int type)
        {
            _type = type;
        }
    }
}
