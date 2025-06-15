using System.Collections.Generic;
using Trade360SDK.Common.Entities.Livescore;
using Xunit;

namespace Trade360SDK.Common.Tests
{
    public class PeriodTests
    {
        [Fact]
        public void Properties_ShouldGetAndSetValues()
        {
            var results = new List<Result> { new Result() };
            var incidents = new List<Incident> { new Incident() };
            var subPeriods = new List<Period> { new Period() };
            var period = new Period
            {
                Type = 1,
                IsFinished = true,
                IsConfirmed = true,
                Results = results,
                Incidents = incidents,
                SubPeriods = subPeriods,
                SequenceNumber = 5
            };
            Assert.Equal(1, period.Type);
            Assert.True(period.IsFinished);
            Assert.True(period.IsConfirmed);
            Assert.Equal(results, period.Results);
            Assert.Equal(incidents, period.Incidents);
            Assert.Equal(subPeriods, period.SubPeriods);
            Assert.Equal(5, period.SequenceNumber);
        }

        [Fact]
        public void Properties_ShouldAllowNullsAndDefaults()
        {
            var period = new Period();
            Assert.Equal(0, period.Type);
            Assert.False(period.IsFinished);
            Assert.False(period.IsConfirmed);
            Assert.Null(period.Results);
            Assert.Null(period.Incidents);
            Assert.Null(period.SubPeriods);
            Assert.Equal(0, period.SequenceNumber);
        }
    }
} 