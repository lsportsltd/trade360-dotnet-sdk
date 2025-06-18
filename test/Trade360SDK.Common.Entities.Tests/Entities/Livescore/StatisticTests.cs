using System.Collections.Generic;
using Trade360SDK.Common.Entities.Livescore;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class StatisticTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var results = new List<Result> { new Result() };
            var incidents = new List<Incident> { new Incident() };
            var stat = new Statistic
            {
                Type = 2,
                Results = results,
                Incidents = incidents
            };
            Assert.Equal(2, stat.Type);
            Assert.Equal(results, stat.Results);
            Assert.Equal(incidents, stat.Incidents);
        }

        [Fact]
        public void Properties_ShouldAllowNullsAndDefaults()
        {
            var stat = new Statistic();
            Assert.Equal(0, stat.Type);
            Assert.Null(stat.Results);
            Assert.Null(stat.Incidents);
        }
    }
} 