using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Moq;
using Trade360SDK.Common.Attributes;
using Trade360SDK.Common.Entities.Enums;
using Trade360SDK.Feed.RabbitMQ.Resolvers;
using Xunit;

namespace Trade360SDK.Feed.RabbitMQ.Tests.Resolvers
{
    /// <summary>
    /// Comprehensive unit tests for MessageProcessorContainer covering dependency resolution,
    /// error scenarios, and edge cases to maximize code coverage.
    /// </summary>
    public class MessageProcessorContainerTests
    {
        #region Test Classes and Mocks

        // Test flow types
        public class TestFlowA { }
        public class TestFlowB { }

        // Test entity types with attributes
        [Trade360Entity(1)]
        public class TestEntity1 { }

        [Trade360Entity(2)]
        public class TestEntity2 { }

        [Trade360Entity(3)]
        public class TestEntity3 { }

        [Trade360Entity(999)]
        public class TestEntityForDuplicateTest { }

        #endregion

        #region Constructor Tests

        [Fact]
        public void Constructor_WithValidProcessors_ShouldInitializeCorrectly()
        {
            // Arrange
            var mockProcessor1 = CreateMockProcessor<TestFlowA, TestEntity1>();
            var mockProcessor2 = CreateMockProcessor<TestFlowA, TestEntity2>();
            var processors = new[] { mockProcessor1.Object, mockProcessor2.Object };

            // Act
            var container = new MessageProcessorContainer<TestFlowA>(processors);

            // Assert
            container.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithEmptyProcessors_ShouldInitializeWithEmptyContainer()
        {
            // Arrange
            var processors = Array.Empty<IMessageProcessor>();

            // Act
            var container = new MessageProcessorContainer<TestFlowA>(processors);

            // Assert
            container.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithNullProcessors_ShouldThrowNullReferenceException()
        {
            // Arrange
            IEnumerable<IMessageProcessor> processors = null!;

            // Act & Assert
            var act = () => new MessageProcessorContainer<TestFlowA>(processors);
            act.Should().Throw<NullReferenceException>();
        }

        [Fact]
        public void Constructor_WithMixedFlowTypes_ShouldOnlyIncludeMatchingFlowType()
        {
            // Arrange
            var processorA1 = CreateMockProcessor<TestFlowA, TestEntity1>();
            var processorA2 = CreateMockProcessor<TestFlowA, TestEntity2>();
            var processorB1 = CreateMockProcessor<TestFlowB, TestEntity1>(); // Different flow type
            var processorB2 = CreateMockProcessor<TestFlowB, TestEntity3>(); // Different flow type

            var processors = new[]
            {
                processorA1.Object,
                processorA2.Object,
                processorB1.Object,
                processorB2.Object
            };

            // Act
            var container = new MessageProcessorContainer<TestFlowA>(processors);

            // Assert
            // Should be able to get processors for TestFlowA entities
            var processor1 = container.GetMessageProcessor(1);
            var processor2 = container.GetMessageProcessor(2);
            
            processor1.Should().BeSameAs(processorA1.Object);
            processor2.Should().BeSameAs(processorA2.Object);

            // Should not be able to get processors for TestFlowB entities (3)
            var act = () => container.GetMessageProcessor(3);
            act.Should().Throw<KeyNotFoundException>();
        }

        [Fact]
        public void Constructor_WithDuplicateEntityKeys_ShouldThrowArgumentException()
        {
            // Arrange
            var processor1 = CreateMockProcessor<TestFlowA, TestEntityForDuplicateTest>();
            var processor2 = CreateMockProcessor<TestFlowA, TestEntityForDuplicateTest>(); // Same entity key

            var processors = new[] { processor1.Object, processor2.Object };

            // Act & Assert
            var act = () => new MessageProcessorContainer<TestFlowA>(processors);
            act.Should().Throw<ArgumentException>()
                .WithMessage("Failed to add resolver as the resolver already registered.");
        }

        #endregion

        #region GetMessageProcessor Tests

        [Fact]
        public void GetMessageProcessor_WithValidEntityKey_ShouldReturnCorrectProcessor()
        {
            // Arrange
            var mockProcessor1 = CreateMockProcessor<TestFlowA, TestEntity1>();
            var mockProcessor2 = CreateMockProcessor<TestFlowA, TestEntity2>();
            var processors = new[] { mockProcessor1.Object, mockProcessor2.Object };
            var container = new MessageProcessorContainer<TestFlowA>(processors);

            // Act
            var result1 = container.GetMessageProcessor(1);
            var result2 = container.GetMessageProcessor(2);

            // Assert
            result1.Should().BeSameAs(mockProcessor1.Object);
            result2.Should().BeSameAs(mockProcessor2.Object);
        }

        [Fact]
        public void GetMessageProcessor_WithInvalidEntityKey_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var mockProcessor = CreateMockProcessor<TestFlowA, TestEntity1>();
            var processors = new[] { mockProcessor.Object };
            var container = new MessageProcessorContainer<TestFlowA>(processors);

            // Act & Assert
            var act = () => container.GetMessageProcessor(999); // Non-existent key
            act.Should().Throw<KeyNotFoundException>();
        }

        [Fact]
        public void GetMessageProcessor_WithZeroEntityKey_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var mockProcessor = CreateMockProcessor<TestFlowA, TestEntity1>();
            var processors = new[] { mockProcessor.Object };
            var container = new MessageProcessorContainer<TestFlowA>(processors);

            // Act & Assert
            var act = () => container.GetMessageProcessor(0);
            act.Should().Throw<KeyNotFoundException>();
        }

        [Fact]
        public void GetMessageProcessor_WithNegativeEntityKey_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var mockProcessor = CreateMockProcessor<TestFlowA, TestEntity1>();
            var processors = new[] { mockProcessor.Object };
            var container = new MessageProcessorContainer<TestFlowA>(processors);

            // Act & Assert
            var act = () => container.GetMessageProcessor(-1);
            act.Should().Throw<KeyNotFoundException>();
        }

        [Fact]
        public void GetMessageProcessor_CalledMultipleTimesWithSameKey_ShouldReturnSameInstance()
        {
            // Arrange
            var mockProcessor = CreateMockProcessor<TestFlowA, TestEntity1>();
            var processors = new[] { mockProcessor.Object };
            var container = new MessageProcessorContainer<TestFlowA>(processors);

            // Act
            var result1 = container.GetMessageProcessor(1);
            var result2 = container.GetMessageProcessor(1);
            var result3 = container.GetMessageProcessor(1);

            // Assert
            result1.Should().BeSameAs(mockProcessor.Object);
            result2.Should().BeSameAs(mockProcessor.Object);
            result3.Should().BeSameAs(mockProcessor.Object);
            result1.Should().BeSameAs(result2);
            result2.Should().BeSameAs(result3);
        }

        #endregion

        #region Integration Tests

        [Fact]
        public void MessageProcessorContainer_WithMultipleProcessors_ShouldHandleCorrectly()
        {
            // Arrange
            var processor1 = CreateMockProcessor<TestFlowA, TestEntity1>();
            var processor2 = CreateMockProcessor<TestFlowA, TestEntity2>();
            var processor3 = CreateMockProcessor<TestFlowA, TestEntity3>();
            
            var processors = new[] { processor1.Object, processor2.Object, processor3.Object };

            // Act
            var container = new MessageProcessorContainer<TestFlowA>(processors);

            // Assert
            var result1 = container.GetMessageProcessor(1);
            var result2 = container.GetMessageProcessor(2);
            var result3 = container.GetMessageProcessor(3);
            
            result1.Should().BeSameAs(processor1.Object);
            result2.Should().BeSameAs(processor2.Object);
            result3.Should().BeSameAs(processor3.Object);
        }

        [Fact]
        public void MessageProcessorContainer_WithComplexFlowTypeHierarchy_ShouldWorkCorrectly()
        {
            // Arrange
            var processor1 = CreateMockProcessor<FlowType, TestEntity1>(); // Using actual FlowType enum
            var processor2 = CreateMockProcessor<FlowType, TestEntity2>();
            var processors = new[] { processor1.Object, processor2.Object };

            // Act
            var container = new MessageProcessorContainer<FlowType>(processors);

            // Assert
            var result1 = container.GetMessageProcessor(1);
            var result2 = container.GetMessageProcessor(2);
            
            result1.Should().BeSameAs(processor1.Object);
            result2.Should().BeSameAs(processor2.Object);
        }

        #endregion

        #region Thread Safety Tests

        [Fact]
        public void GetMessageProcessor_AccessedMultipleTimes_ShouldBeConsistent()
        {
            // Arrange
            var mockProcessor = CreateMockProcessor<TestFlowA, TestEntity1>();
            var processors = new[] { mockProcessor.Object };
            var container = new MessageProcessorContainer<TestFlowA>(processors);

            // Act & Assert
            for (int i = 0; i < 10; i++)
            {
                var result = container.GetMessageProcessor(1);
                result.Should().BeSameAs(mockProcessor.Object);
            }
        }

        #endregion

        #region Edge Cases

        [Fact]
        public void Constructor_WithNullProcessor_ShouldHandleGracefully()
        {
            // Arrange
            var mockProcessor = CreateMockProcessor<TestFlowA, TestEntity1>();
            var processors = new IMessageProcessor[] { mockProcessor.Object, null! };

            // Act & Assert
            var act = () => new MessageProcessorContainer<TestFlowA>(processors);
            act.Should().Throw<NullReferenceException>();
        }

        [Fact]
        public void Constructor_WithProcessorReturningNullType_ShouldThrowException()
        {
            // Arrange
            var mockProcessor = new Mock<IMessageProcessor>();
            mockProcessor.Setup(x => x.GetTypeOfTFlow()).Returns(typeof(TestFlowA));
            mockProcessor.Setup(x => x.GetTypeOfTType()).Returns((Type)null!);
            
            var processors = new[] { mockProcessor.Object };

            // Act & Assert
            var act = () => new MessageProcessorContainer<TestFlowA>(processors);
            act.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Constructor_WithProcessorHavingNoAttribute_ShouldThrowException()
        {
            // Arrange
            var mockProcessor = new Mock<IMessageProcessor>();
            mockProcessor.Setup(x => x.GetTypeOfTFlow()).Returns(typeof(TestFlowA));
            mockProcessor.Setup(x => x.GetTypeOfTType()).Returns(typeof(string)); // No Trade360EntityAttribute
            
            var processors = new[] { mockProcessor.Object };

            // Act & Assert
            var act = () => new MessageProcessorContainer<TestFlowA>(processors);
            act.Should().Throw<NullReferenceException>();
        }

        #endregion

        #region Helper Methods

        private static Mock<IMessageProcessor> CreateMockProcessor<TFlow, TEntity>()
            where TEntity : class
        {
            var mock = new Mock<IMessageProcessor>();
            mock.Setup(x => x.GetTypeOfTFlow()).Returns(typeof(TFlow));
            mock.Setup(x => x.GetTypeOfTType()).Returns(typeof(TEntity));
            return mock;
        }



        #endregion
    }
}
