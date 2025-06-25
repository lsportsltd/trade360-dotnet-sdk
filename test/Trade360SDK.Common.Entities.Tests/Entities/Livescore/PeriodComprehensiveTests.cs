using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;
using Trade360SDK.Common.Entities.Livescore;

namespace Trade360SDK.Common.Entities.Tests.Entities.Livescore
{
    public class PeriodComprehensiveTests
    {
        [Fact]
        public void Period_DefaultConstructor_ShouldCreateInstanceWithDefaultValues()
        {
            // Act
            var period = new Period();

            // Assert
            period.Should().NotBeNull();
            period.Type.Should().Be(0);
            period.IsFinished.Should().BeFalse();
            period.IsConfirmed.Should().BeFalse();
            period.Results.Should().BeNull();
            period.Incidents.Should().BeNull();
            period.SubPeriods.Should().BeNull();
            period.SequenceNumber.Should().Be(0);
        }

        [Fact]
        public void Period_SetType_ShouldSetValue()
        {
            // Arrange
            var period = new Period();
            var type = 1;

            // Act
            period.Type = type;

            // Assert
            period.Type.Should().Be(type);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        [InlineData(100)]
        public void Period_SetVariousTypes_ShouldSetValue(int type)
        {
            // Arrange
            var period = new Period();

            // Act
            period.Type = type;

            // Assert
            period.Type.Should().Be(type);
        }

        [Fact]
        public void Period_SetIsFinished_ShouldSetValue()
        {
            // Arrange
            var period = new Period();

            // Act
            period.IsFinished = true;

            // Assert
            period.IsFinished.Should().BeTrue();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Period_SetVariousIsFinished_ShouldSetValue(bool isFinished)
        {
            // Arrange
            var period = new Period();

            // Act
            period.IsFinished = isFinished;

            // Assert
            period.IsFinished.Should().Be(isFinished);
        }

        [Fact]
        public void Period_SetIsConfirmed_ShouldSetValue()
        {
            // Arrange
            var period = new Period();

            // Act
            period.IsConfirmed = true;

            // Assert
            period.IsConfirmed.Should().BeTrue();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Period_SetVariousIsConfirmed_ShouldSetValue(bool isConfirmed)
        {
            // Arrange
            var period = new Period();

            // Act
            period.IsConfirmed = isConfirmed;

            // Assert
            period.IsConfirmed.Should().Be(isConfirmed);
        }

        [Fact]
        public void Period_SetResults_ShouldSetValue()
        {
            // Arrange
            var period = new Period();
            var results = new List<Result>
            {
                new Result { Position = "1", Value = "2" },
                new Result { Position = "2", Value = "1" }
            };

            // Act
            period.Results = results;

            // Assert
            period.Results.Should().BeEquivalentTo(results);
            period.Results.Should().HaveCount(2);
        }

        [Fact]
        public void Period_SetIncidents_ShouldSetValue()
        {
            // Arrange
            var period = new Period();
            var incidents = new List<Incident>
            {
                new Incident { Period = 1, IncidentType = 1, Seconds = 2700, ParticipantPosition = "1" },
                new Incident { Period = 1, IncidentType = 2, Seconds = 4020, ParticipantPosition = "2" }
            };

            // Act
            period.Incidents = incidents;

            // Assert
            period.Incidents.Should().BeEquivalentTo(incidents);
            period.Incidents.Should().HaveCount(2);
        }

        [Fact]
        public void Period_SetSubPeriods_ShouldSetValue()
        {
            // Arrange
            var period = new Period();
            var subPeriods = new List<Period>
            {
                new Period { Type = 1, SequenceNumber = 1 },
                new Period { Type = 1, SequenceNumber = 2 }
            };

            // Act
            period.SubPeriods = subPeriods;

            // Assert
            period.SubPeriods.Should().BeEquivalentTo(subPeriods);
            period.SubPeriods.Should().HaveCount(2);
        }

        [Fact]
        public void Period_SetSequenceNumber_ShouldSetValue()
        {
            // Arrange
            var period = new Period();
            var sequenceNumber = 5;

            // Act
            period.SequenceNumber = sequenceNumber;

            // Assert
            period.SequenceNumber.Should().Be(sequenceNumber);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(-1)]
        [InlineData(int.MaxValue)]
        [InlineData(int.MinValue)]
        [InlineData(999)]
        public void Period_SetVariousSequenceNumbers_ShouldSetValue(int sequenceNumber)
        {
            // Arrange
            var period = new Period();

            // Act
            period.SequenceNumber = sequenceNumber;

            // Assert
            period.SequenceNumber.Should().Be(sequenceNumber);
        }

        [Fact]
        public void Period_SetAllProperties_ShouldSetAllValues()
        {
            // Arrange
            var period = new Period();
            var type = 2;
            var isFinished = true;
            var isConfirmed = false;
            var results = new List<Result> { new Result { Position = "1", Value = "3" } };
            var incidents = new List<Incident> { new Incident { Period = 2, IncidentType = 1, Seconds = 5400 } };
            var subPeriods = new List<Period> { new Period { Type = 3, SequenceNumber = 1 } };
            var sequenceNumber = 10;

            // Act
            period.Type = type;
            period.IsFinished = isFinished;
            period.IsConfirmed = isConfirmed;
            period.Results = results;
            period.Incidents = incidents;
            period.SubPeriods = subPeriods;
            period.SequenceNumber = sequenceNumber;

            // Assert
            period.Type.Should().Be(type);
            period.IsFinished.Should().Be(isFinished);
            period.IsConfirmed.Should().Be(isConfirmed);
            period.Results.Should().BeEquivalentTo(results);
            period.Incidents.Should().BeEquivalentTo(incidents);
            period.SubPeriods.Should().BeEquivalentTo(subPeriods);
            period.SequenceNumber.Should().Be(sequenceNumber);
        }

        [Fact]
        public void Period_SetNullValues_ShouldSetNulls()
        {
            // Arrange
            var period = new Period();

            // Act
            period.Results = null;
            period.Incidents = null;
            period.SubPeriods = null;

            // Assert
            period.Results.Should().BeNull();
            period.Incidents.Should().BeNull();
            period.SubPeriods.Should().BeNull();
        }

        [Fact]
        public void Period_SetEmptyCollections_ShouldSetEmptyCollections()
        {
            // Arrange
            var period = new Period();
            var emptyResults = new List<Result>();
            var emptyIncidents = new List<Incident>();
            var emptySubPeriods = new List<Period>();

            // Act
            period.Results = emptyResults;
            period.Incidents = emptyIncidents;
            period.SubPeriods = emptySubPeriods;

            // Assert
            period.Results.Should().BeEmpty();
            period.Incidents.Should().BeEmpty();
            period.SubPeriods.Should().BeEmpty();
        }



        [Fact]
        public void Period_WithComplexData_ShouldStoreCorrectly()
        {
            // Arrange
            var period = new Period();
            var complexResults = new List<Result>
            {
                new Result { Position = "1", Value = "3" },
                new Result { Position = "2", Value = "1" },
                new Result { Position = "0", Value = "4" } // Total
            };
            var complexIncidents = new List<Incident>
            {
                new Incident { Period = 1, IncidentType = 1, Seconds = 1380, PlayerId = "10", PlayerName = "Player A" },
                new Incident { Period = 1, IncidentType = 1, Seconds = 2700, PlayerId = "11", PlayerName = "Player B" },
                new Incident { Period = 1, IncidentType = 1, Seconds = 4020, PlayerId = "20", PlayerName = "Player C" },
                new Incident { Period = 1, IncidentType = 1, Seconds = 5340, PlayerId = "12", PlayerName = "Player D" }
            };

            // Act
            period.Type = 1; // First Half
            period.IsFinished = true;
            period.IsConfirmed = true;
            period.Results = complexResults;
            period.Incidents = complexIncidents;
            period.SequenceNumber = 1;

            // Assert
            period.Type.Should().Be(1);
            period.IsFinished.Should().BeTrue();
            period.IsConfirmed.Should().BeTrue();
            period.Results.Should().HaveCount(3);
            period.Incidents.Should().HaveCount(4);
            period.Results.Where(r => r.Position == "1").First().Value.Should().Be("3");
            period.Incidents.Where(i => i.PlayerId == "10").First().PlayerName.Should().Be("Player A");
            period.SequenceNumber.Should().Be(1);
        }

        [Fact]
        public void Period_WithNestedSubPeriods_ShouldStoreCorrectly()
        {
            // Arrange
            var period = new Period();
            var subPeriod1 = new Period
            {
                Type = 11, // Extra Time First Half
                IsFinished = true,
                IsConfirmed = true,
                SequenceNumber = 1,
                Results = new List<Result> { new Result { Position = "1", Value = "1" } }
            };
            var subPeriod2 = new Period
            {
                Type = 12, // Extra Time Second Half
                IsFinished = true,
                IsConfirmed = true,
                SequenceNumber = 2,
                Results = new List<Result> { new Result { Position = "2", Value = "1" } }
            };

            // Act
            period.Type = 10; // Extra Time
            period.IsFinished = true;
            period.IsConfirmed = true;
            period.SubPeriods = new List<Period> { subPeriod1, subPeriod2 };
            period.SequenceNumber = 3;

            // Assert
            period.Type.Should().Be(10);
            period.SubPeriods.Should().HaveCount(2);
            period.SubPeriods.First().Type.Should().Be(11);
            period.SubPeriods.Last().Type.Should().Be(12);
            period.SubPeriods.First().Results.Should().HaveCount(1);
            period.SubPeriods.Last().Results.Should().HaveCount(1);
        }



        [Fact]
        public void Period_WithLargeCollections_ShouldHandleCorrectly()
        {
            // Arrange
            var period = new Period();
            var largeResults = Enumerable.Range(1, 100).Select(i => new Result { Position = i.ToString(), Value = i.ToString() }).ToList();
            var largeIncidents = Enumerable.Range(1, 50).Select(i => new Incident { Period = i % 2 + 1, IncidentType = i % 5 + 1, Seconds = i * 60 }).ToList();

            // Act
            period.Results = largeResults;
            period.Incidents = largeIncidents;

            // Assert
            period.Results.Should().HaveCount(100);
            period.Incidents.Should().HaveCount(50);
            period.Results.First().Value.Should().Be("1");
            period.Results.Last().Value.Should().Be("100");
        }
    }
} 