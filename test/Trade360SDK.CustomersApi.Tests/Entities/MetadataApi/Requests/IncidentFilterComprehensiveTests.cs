using Trade360SDK.CustomersApi.Entities.MetadataApi.Requests;
using Xunit;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Trade360SDK.CustomersApi.Tests.Entities.MetadataApi.Requests
{
    public class IncidentFilterComprehensiveTests
    {
        [Fact]
        public void IncidentFilter_DefaultConstructor_ShouldInitializeWithDefaults()
        {
            // Act
            var filter = new IncidentFilter();

            // Assert
            filter.Should().NotBeNull();
            filter.Ids.Should().BeNull();
            filter.Sports.Should().BeNull();
            filter.From.Should().BeNull();
            filter.SearchText.Should().BeNull();
        }

        [Fact]
        public void IncidentFilter_IdsProperty_ShouldAcceptNull()
        {
            // Arrange
            var filter = new IncidentFilter();

            // Act
            filter.Ids = null;

            // Assert
            filter.Ids.Should().BeNull();
        }

        [Fact]
        public void IncidentFilter_IdsProperty_ShouldAcceptEmptyCollection()
        {
            // Arrange
            var filter = new IncidentFilter();
            var emptyIds = new List<int>();

            // Act
            filter.Ids = emptyIds;

            // Assert
            filter.Ids.Should().NotBeNull();
            filter.Ids.Should().BeEmpty();
        }

        [Fact]
        public void IncidentFilter_IdsProperty_ShouldAcceptPopulatedCollection()
        {
            // Arrange
            var filter = new IncidentFilter();
            var ids = new List<int> { 101, 102, 103, 105 };

            // Act
            filter.Ids = ids;

            // Assert
            filter.Ids.Should().NotBeNull();
            filter.Ids.Should().HaveCount(4);
            filter.Ids.Should().Equal(101, 102, 103, 105);
        }

        [Fact]
        public void IncidentFilter_SportsProperty_ShouldAcceptNull()
        {
            // Arrange
            var filter = new IncidentFilter();

            // Act
            filter.Sports = null;

            // Assert
            filter.Sports.Should().BeNull();
        }

        [Fact]
        public void IncidentFilter_SportsProperty_ShouldAcceptEmptyCollection()
        {
            // Arrange
            var filter = new IncidentFilter();
            var emptySports = new List<int>();

            // Act
            filter.Sports = emptySports;

            // Assert
            filter.Sports.Should().NotBeNull();
            filter.Sports.Should().BeEmpty();
        }

        [Fact]
        public void IncidentFilter_SportsProperty_ShouldAcceptPopulatedCollection()
        {
            // Arrange
            var filter = new IncidentFilter();
            var sports = new List<int> { 1, 6, 12, 18 };

            // Act
            filter.Sports = sports;

            // Assert
            filter.Sports.Should().NotBeNull();
            filter.Sports.Should().HaveCount(4);
            filter.Sports.Should().Equal(1, 6, 12, 18);
        }

        [Fact]
        public void IncidentFilter_FromProperty_ShouldAcceptNull()
        {
            // Arrange
            var filter = new IncidentFilter();

            // Act
            filter.From = null;

            // Assert
            filter.From.Should().BeNull();
        }

        [Fact]
        public void IncidentFilter_FromProperty_ShouldAcceptDateTime()
        {
            // Arrange
            var filter = new IncidentFilter();
            var dateTime = new DateTime(2022, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            // Act
            filter.From = dateTime;

            // Assert
            filter.From.Should().Be(dateTime);
        }

        [Fact]
        public void IncidentFilter_SearchTextProperty_ShouldAcceptNull()
        {
            // Arrange
            var filter = new IncidentFilter();

            // Act
            filter.SearchText = null;

            // Assert
            filter.SearchText.Should().BeNull();
        }

        [Fact]
        public void IncidentFilter_SearchTextProperty_ShouldAcceptEmptyCollection()
        {
            // Arrange
            var filter = new IncidentFilter();
            var emptyText = new List<string>();

            // Act
            filter.SearchText = emptyText;

            // Assert
            filter.SearchText.Should().NotBeNull();
            filter.SearchText.Should().BeEmpty();
        }

        [Fact]
        public void IncidentFilter_SearchTextProperty_ShouldAcceptPopulatedCollection()
        {
            // Arrange
            var filter = new IncidentFilter();
            var searchTerms = new List<string> { "football", "match", "premier" };

            // Act
            filter.SearchText = searchTerms;

            // Assert
            filter.SearchText.Should().NotBeNull();
            filter.SearchText.Should().HaveCount(3);
            filter.SearchText.Should().Equal("football", "match", "premier");
        }

        [Fact]
        public void IncidentFilter_AllProperties_ShouldWorkTogether()
        {
            // Arrange
            var filter = new IncidentFilter();
            var ids = new List<int> { 101, 102 };
            var sports = new List<int> { 1, 6 };
            var fromDate = new DateTime(2022, 1, 1);
            var searchText = new List<string> { "test" };

            // Act
            filter.Ids = ids;
            filter.Sports = sports;
            filter.From = fromDate;
            filter.SearchText = searchText;

            // Assert
            filter.Ids.Should().Equal(101, 102);
            filter.Sports.Should().Equal(1, 6);
            filter.From.Should().Be(fromDate);
            filter.SearchText.Should().Equal("test");
        }

        [Theory]
        [InlineData(new int[] { })]
        [InlineData(new int[] { 1 })]
        [InlineData(new int[] { 1, 6, 12 })]
        [InlineData(new int[] { -1, 0, 999999 })]
        public void IncidentFilter_SportsProperty_ShouldAcceptVariousArrays(int[] sportIds)
        {
            // Arrange
            var filter = new IncidentFilter();

            // Act
            filter.Sports = sportIds;

            // Assert
            filter.Sports.Should().Equal(sportIds);
        }

        [Theory]
        [InlineData(new int[] { })]
        [InlineData(new int[] { 101 })]
        [InlineData(new int[] { 101, 102, 103 })]
        [InlineData(new int[] { -1, 0, 999999 })]
        public void IncidentFilter_IdsProperty_ShouldAcceptVariousArrays(int[] ids)
        {
            // Arrange
            var filter = new IncidentFilter();

            // Act
            filter.Ids = ids;

            // Assert
            filter.Ids.Should().Equal(ids);
        }

        [Fact]
        public void IncidentFilter_PropertyAssignment_ShouldAllowChaining()
        {
            // Arrange & Act
            var filter = new IncidentFilter
            {
                Ids = new[] { 101, 102 },
                Sports = new[] { 1, 6 },
                From = new DateTime(2022, 1, 1),
                SearchText = new[] { "test" }
            };

            // Assert
            filter.Ids.Should().Equal(101, 102);
            filter.Sports.Should().Equal(1, 6);
            filter.From.Should().Be(new DateTime(2022, 1, 1));
            filter.SearchText.Should().Equal("test");
        }

        [Fact]
        public void IncidentFilter_Properties_ShouldAllowReassignment()
        {
            // Arrange
            var filter = new IncidentFilter
            {
                Ids = new[] { 101 },
                Sports = new[] { 1 },
                From = new DateTime(2022, 1, 1),
                SearchText = new[] { "old" }
            };

            // Act
            filter.Ids = new[] { 102, 103 };
            filter.Sports = new[] { 6, 12 };
            filter.From = new DateTime(2022, 12, 31);
            filter.SearchText = new[] { "new", "updated" };

            // Assert
            filter.Ids.Should().Equal(102, 103);
            filter.Sports.Should().Equal(6, 12);
            filter.From.Should().Be(new DateTime(2022, 12, 31));
            filter.SearchText.Should().Equal("new", "updated");
        }

        [Fact]
        public void IncidentFilter_SearchTextProperty_ShouldHandleWhitespace()
        {
            // Arrange
            var filter = new IncidentFilter();
            var searchText = new[] { "", " ", "  ", "\t", "\n" };

            // Act
            filter.SearchText = searchText;

            // Assert
            filter.SearchText.Should().Equal("", " ", "  ", "\t", "\n");
        }

        [Fact]
        public void IncidentFilter_IEnumerableUsage_ShouldWorkWithLinq()
        {
            // Arrange
            var filter = new IncidentFilter
            {
                Ids = new[] { 101, 102, 103, 104, 105 },
                Sports = new[] { 1, 6, 12, 18 },
                SearchText = new[] { "football", "basketball", "tennis" }
            };

            // Act & Assert
            filter.Ids!.Count().Should().Be(5);
            filter.Sports!.Where(s => s > 10).Should().Equal(12, 18);
            filter.SearchText!.Any(t => t.Contains("ball")).Should().BeTrue();
        }
    }
} 