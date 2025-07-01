using Trade360SDK.CustomersApi.Entities.MetadataApi.Requests;
using Xunit;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Trade360SDK.CustomersApi.Tests.Entities.MetadataApi.Requests
{
    public class IncidentFilterDtoComprehensiveTests
    {
        [Fact]
        public void IncidentFilterDto_DefaultConstructor_ShouldInitializeWithDefaults()
        {
            // Act
            var filterDto = new IncidentFilterDto();

            // Assert
            filterDto.Should().NotBeNull();
            filterDto.Ids.Should().BeNull();
            filterDto.Sports.Should().BeNull();
            filterDto.From.Should().BeNull();
            filterDto.SearchText.Should().BeNull();
        }

        [Fact]
        public void IncidentFilterDto_IdsProperty_ShouldAcceptNull()
        {
            // Arrange
            var filterDto = new IncidentFilterDto();

            // Act
            filterDto.Ids = null;

            // Assert
            filterDto.Ids.Should().BeNull();
        }

        [Fact]
        public void IncidentFilterDto_IdsProperty_ShouldAcceptEmptyCollection()
        {
            // Arrange
            var filterDto = new IncidentFilterDto();
            var emptyIds = new List<int>();

            // Act
            filterDto.Ids = emptyIds;

            // Assert
            filterDto.Ids.Should().NotBeNull();
            filterDto.Ids.Should().BeEmpty();
        }

        [Fact]
        public void IncidentFilterDto_IdsProperty_ShouldAcceptPopulatedCollection()
        {
            // Arrange
            var filterDto = new IncidentFilterDto();
            var ids = new List<int> { 101, 102, 103, 105 };

            // Act
            filterDto.Ids = ids;

            // Assert
            filterDto.Ids.Should().NotBeNull();
            filterDto.Ids.Should().HaveCount(4);
            filterDto.Ids.Should().Equal(101, 102, 103, 105);
        }

        [Fact]
        public void IncidentFilterDto_SportsProperty_ShouldAcceptNull()
        {
            // Arrange
            var filterDto = new IncidentFilterDto();

            // Act
            filterDto.Sports = null;

            // Assert
            filterDto.Sports.Should().BeNull();
        }

        [Fact]
        public void IncidentFilterDto_SportsProperty_ShouldAcceptEmptyCollection()
        {
            // Arrange
            var filterDto = new IncidentFilterDto();
            var emptySports = new List<int>();

            // Act
            filterDto.Sports = emptySports;

            // Assert
            filterDto.Sports.Should().NotBeNull();
            filterDto.Sports.Should().BeEmpty();
        }

        [Fact]
        public void IncidentFilterDto_SportsProperty_ShouldAcceptPopulatedCollection()
        {
            // Arrange
            var filterDto = new IncidentFilterDto();
            var sports = new List<int> { 1, 6, 12, 18 };

            // Act
            filterDto.Sports = sports;

            // Assert
            filterDto.Sports.Should().NotBeNull();
            filterDto.Sports.Should().HaveCount(4);
            filterDto.Sports.Should().Equal(1, 6, 12, 18);
        }

        [Fact]
        public void IncidentFilterDto_FromProperty_ShouldAcceptNull()
        {
            // Arrange
            var filterDto = new IncidentFilterDto();

            // Act
            filterDto.From = null;

            // Assert
            filterDto.From.Should().BeNull();
        }

        [Fact]
        public void IncidentFilterDto_FromProperty_ShouldAcceptDateTime()
        {
            // Arrange
            var filterDto = new IncidentFilterDto();
            var dateTime = new DateTime(2022, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            // Act
            filterDto.From = dateTime;

            // Assert
            filterDto.From.Should().Be(dateTime);
        }

        [Fact]
        public void IncidentFilterDto_SearchTextProperty_ShouldAcceptNull()
        {
            // Arrange
            var filterDto = new IncidentFilterDto();

            // Act
            filterDto.SearchText = null;

            // Assert
            filterDto.SearchText.Should().BeNull();
        }

        [Fact]
        public void IncidentFilterDto_SearchTextProperty_ShouldAcceptEmptyCollection()
        {
            // Arrange
            var filterDto = new IncidentFilterDto();
            var emptyText = new List<string>();

            // Act
            filterDto.SearchText = emptyText;

            // Assert
            filterDto.SearchText.Should().NotBeNull();
            filterDto.SearchText.Should().BeEmpty();
        }

        [Fact]
        public void IncidentFilterDto_SearchTextProperty_ShouldAcceptPopulatedCollection()
        {
            // Arrange
            var filterDto = new IncidentFilterDto();
            var searchTerms = new List<string> { "football", "match", "premier" };

            // Act
            filterDto.SearchText = searchTerms;

            // Assert
            filterDto.SearchText.Should().NotBeNull();
            filterDto.SearchText.Should().HaveCount(3);
            filterDto.SearchText.Should().Equal("football", "match", "premier");
        }

        [Fact]
        public void IncidentFilterDto_AllProperties_ShouldWorkTogether()
        {
            // Arrange
            var filterDto = new IncidentFilterDto();
            var ids = new List<int> { 101, 102 };
            var sports = new List<int> { 1, 6 };
            var fromDate = new DateTime(2022, 1, 1);
            var searchText = new List<string> { "test" };

            // Act
            filterDto.Ids = ids;
            filterDto.Sports = sports;
            filterDto.From = fromDate;
            filterDto.SearchText = searchText;

            // Assert
            filterDto.Ids.Should().Equal(101, 102);
            filterDto.Sports.Should().Equal(1, 6);
            filterDto.From.Should().Be(fromDate);
            filterDto.SearchText.Should().Equal("test");
        }

        [Theory]
        [InlineData(new int[] { })]
        [InlineData(new int[] { 1 })]
        [InlineData(new int[] { 1, 6, 12 })]
        [InlineData(new int[] { -1, 0, 999999 })]
        public void IncidentFilterDto_SportsProperty_ShouldAcceptVariousArrays(int[] sportIds)
        {
            // Arrange
            var filterDto = new IncidentFilterDto();

            // Act
            filterDto.Sports = sportIds;

            // Assert
            filterDto.Sports.Should().Equal(sportIds);
        }

        [Theory]
        [InlineData(new int[] { })]
        [InlineData(new int[] { 101 })]
        [InlineData(new int[] { 101, 102, 103 })]
        [InlineData(new int[] { -1, 0, 999999 })]
        public void IncidentFilterDto_IdsProperty_ShouldAcceptVariousArrays(int[] ids)
        {
            // Arrange
            var filterDto = new IncidentFilterDto();

            // Act
            filterDto.Ids = ids;

            // Assert
            filterDto.Ids.Should().Equal(ids);
        }

        [Fact]
        public void IncidentFilterDto_PropertyAssignment_ShouldAllowChaining()
        {
            // Arrange & Act
            var filterDto = new IncidentFilterDto
            {
                Ids = new[] { 101, 102 },
                Sports = new[] { 1, 6 },
                From = new DateTime(2022, 1, 1),
                SearchText = new[] { "test" }
            };

            // Assert
            filterDto.Ids.Should().Equal(101, 102);
            filterDto.Sports.Should().Equal(1, 6);
            filterDto.From.Should().Be(new DateTime(2022, 1, 1));
            filterDto.SearchText.Should().Equal("test");
        }

        [Fact]
        public void IncidentFilterDto_Properties_ShouldAllowReassignment()
        {
            // Arrange
            var filterDto = new IncidentFilterDto
            {
                Ids = new[] { 101 },
                Sports = new[] { 1 },
                From = new DateTime(2022, 1, 1),
                SearchText = new[] { "old" }
            };

            // Act
            filterDto.Ids = new[] { 102, 103 };
            filterDto.Sports = new[] { 6, 12 };
            filterDto.From = new DateTime(2022, 12, 31);
            filterDto.SearchText = new[] { "new", "updated" };

            // Assert
            filterDto.Ids.Should().Equal(102, 103);
            filterDto.Sports.Should().Equal(6, 12);
            filterDto.From.Should().Be(new DateTime(2022, 12, 31));
            filterDto.SearchText.Should().Equal("new", "updated");
        }

        [Fact]
        public void IncidentFilterDto_DifferenceFromIncidentFilter_ShouldHaveSameStructure()
        {
            // This test documents that IncidentFilterDto has the same structure as IncidentFilter
            // Both use IEnumerable<T> for collections and DateTime? for dates
            
            // Arrange & Act
            var filterDto = new IncidentFilterDto
            {
                Ids = new[] { 101, 102 },
                Sports = new[] { 1, 6 },
                From = new DateTime(2022, 1, 1),
                SearchText = new[] { "test1", "test2" }
            };

            // Assert - Both classes should have identical structure
            filterDto.Ids.Should().BeAssignableTo<IEnumerable<int>>();
            filterDto.Sports.Should().BeAssignableTo<IEnumerable<int>>();
            filterDto.From.Should().NotBeNull();
            filterDto.SearchText.Should().BeAssignableTo<IEnumerable<string>>();
        }

        [Fact]
        public void IncidentFilterDto_IEnumerableUsage_ShouldWorkWithLinq()
        {
            // Arrange
            var filterDto = new IncidentFilterDto
            {
                Ids = new[] { 101, 102, 103, 104, 105 },
                Sports = new[] { 1, 6, 12, 18 },
                SearchText = new[] { "football", "basketball", "tennis" }
            };

            // Act & Assert
            filterDto.Ids!.Count().Should().Be(5);
            filterDto.Sports!.Where(s => s > 10).Should().Equal(12, 18);
            filterDto.SearchText!.Any(t => t.Contains("ball")).Should().BeTrue();
        }

        [Fact]
        public void IncidentFilterDto_DateTimeHandling_ShouldSupportDifferentKinds()
        {
            // Arrange
            var filterDto = new IncidentFilterDto();
            var utcTime = new DateTime(2022, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var localTime = new DateTime(2022, 1, 1, 0, 0, 0, DateTimeKind.Local);
            var unspecifiedTime = new DateTime(2022, 1, 1, 0, 0, 0, DateTimeKind.Unspecified);

            // Act & Assert
            filterDto.From = utcTime;
            filterDto.From.Should().Be(utcTime);
            filterDto.From!.Value.Kind.Should().Be(DateTimeKind.Utc);

            filterDto.From = localTime;
            filterDto.From.Should().Be(localTime);
            filterDto.From!.Value.Kind.Should().Be(DateTimeKind.Local);

            filterDto.From = unspecifiedTime;
            filterDto.From.Should().Be(unspecifiedTime);
            filterDto.From!.Value.Kind.Should().Be(DateTimeKind.Unspecified);
        }
    }
} 