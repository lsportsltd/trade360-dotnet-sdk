using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Trade360SDK.Common.Attributes;

namespace Trade360SDK.Common.Helpers
{
    public static class Trade360AttributeHelper
    {
        public static List<Type> GetAttributes()
        {
            return Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.GetCustomAttributes(typeof(Trade360EntityAttribute), false).Any()).ToList();
        }
    }
}