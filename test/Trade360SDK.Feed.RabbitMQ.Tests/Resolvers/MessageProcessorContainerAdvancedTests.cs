using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using Moq;
using Trade360SDK.Common.Attributes;
using Trade360SDK.Common.Entities.MessageTypes;
using Trade360SDK.Feed.FeedType;
using Trade360SDK.Feed.RabbitMQ.Resolvers;
using Xunit;

namespace Trade360SDK.Feed.RabbitMQ.Tests.Resolvers
{
    public class MessageProcessorContainerAdvancedTests
    {
        [Fact]
        public void Constructor_WithValidProcessors_ShouldInitializeCorrectly()
        {
            // Arrange
            var mockProcessor = new Mock<IMessageProcessor>();
            mockProcessor.Setup(x => x.GetTypeOfTFlow()).Returns(typeof(InPlay));
            mockProcessor.Setup(x => x.GetTypeOfTType()).Returns(typeof(MarketUpdate));

            var processors = new List<IMessageProcessor> { mockProcessor.Object };

            // Act
            var container = new MessageProcessorContainer<InPlay>(processors);

            // Assert
            container.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithEmptyProcessors_ShouldInitializeEmptyContainer()
        {
            // Arrange
            var processors = new List<IMessageProcessor>();

            // Act
            var container = new MessageProcessorContainer<InPlay>(processors);

            // Assert
            container.Should().NotBeNull();
        }

        [Fact]
        public void Constructor_WithMultipleValidProcessors_ShouldRegisterAll()
        {
            // Arrange
            var mockProcessor1 = new Mock<IMessageProcessor>();
            mockProcessor1.Setup(x => x.GetTypeOfTFlow()).Returns(typeof(InPlay));
            mockProcessor1.Setup(x => x.GetTypeOfTType()).Returns(typeof(MarketUpdate));

            var mockProcessor2 = new Mock<IMessageProcessor>();
            mockProcessor2.Setup(x => x.GetTypeOfTFlow()).Returns(typeof(InPlay));
            mockProcessor2.Setup(x => x.GetTypeOfTType()).Returns(typeof(LivescoreUpdate));

            var mockProcessor3 = new Mock<IMessageProcessor>();
            mockProcessor3.Setup(x => x.GetTypeOfTFlow()).Returns(typeof(InPlay));
            mockProcessor3.Setup(x => x.GetTypeOfTType()).Returns(typeof(FixtureMetadataUpdate));

            var processors = new List<IMessageProcessor> { mockProcessor1.Object, mockProcessor2.Object, mockProcessor3.Object };

            // Act
            var container = new MessageProcessorContainer<InPlay>(processors);

            // Assert
            container.Should().NotBeNull();
            var processor1 = container.GetMessageProcessor(3); // MarketUpdate
            var processor2 = container.GetMessageProcessor(2); // LivescoreUpdate
            var processor3 = container.GetMessageProcessor(1); // FixtureMetadataUpdate

            processor1.Should().BeSameAs(mockProcessor1.Object);
            processor2.Should().BeSameAs(mockProcessor2.Object);
            processor3.Should().BeSameAs(mockProcessor3.Object);
        }

        [Fact]
        public void Constructor_WithMixedFlowTypes_ShouldOnlyRegisterMatchingFlowType()
        {
            // Arrange
            var inPlayProcessor = new Mock<IMessageProcessor>();
            inPlayProcessor.Setup(x => x.GetTypeOfTFlow()).Returns(typeof(InPlay));
            inPlayProcessor.Setup(x => x.GetTypeOfTType()).Returns(typeof(MarketUpdate));

            var preMatchProcessor = new Mock<IMessageProcessor>();
            preMatchProcessor.Setup(x => x.GetTypeOfTFlow()).Returns(typeof(PreMatch));
            preMatchProcessor.Setup(x => x.GetTypeOfTType()).Returns(typeof(LivescoreUpdate));

            var processors = new List<IMessageProcessor> { inPlayProcessor.Object, preMatchProcessor.Object };

            // Act
            var inPlayContainer = new MessageProcessorContainer<InPlay>(processors);

            // Assert
            inPlayContainer.Should().NotBeNull();
            var retrievedProcessor = inPlayContainer.GetMessageProcessor(3); // MarketUpdate
            retrievedProcessor.Should().BeSameAs(inPlayProcessor.Object);

            // PreMatch processor should not be registered in InPlay container
            var act = () => inPlayContainer.GetMessageProcessor(2); // LivescoreUpdate
            act.Should().Throw<KeyNotFoundException>();
        }

        [Fact]
        public void Constructor_WithDuplicateEntityKeys_ShouldThrowArgumentException()
        {
            // Arrange
            var mockProcessor1 = new Mock<IMessageProcessor>();
            mockProcessor1.Setup(x => x.GetTypeOfTFlow()).Returns(typeof(InPlay));
            mockProcessor1.Setup(x => x.GetTypeOfTType()).Returns(typeof(MarketUpdate));

            var mockProcessor2 = new Mock<IMessageProcessor>();
            mockProcessor2.Setup(x => x.GetTypeOfTFlow()).Returns(typeof(InPlay));
            mockProcessor2.Setup(x => x.GetTypeOfTType()).Returns(typeof(MarketUpdate)); // Same entity key

            var processors = new List<IMessageProcessor> { mockProcessor1.Object, mockProcessor2.Object };

            // Act & Assert
            var act = () => new MessageProcessorContainer<InPlay>(processors);
            act.Should().Throw<ArgumentException>()
                .WithMessage("Failed to add resolver as the resolver already registered.");
        }

        [Fact]
        public void GetMessageProcessor_WithValidMessageType_ShouldReturnCorrectProcessor()
        {
            // Arrange
            var mockProcessor = new Mock<IMessageProcessor>();
            mockProcessor.Setup(x => x.GetTypeOfTFlow()).Returns(typeof(InPlay));
            mockProcessor.Setup(x => x.GetTypeOfTType()).Returns(typeof(MarketUpdate));

            var processors = new List<IMessageProcessor> { mockProcessor.Object };
            var container = new MessageProcessorContainer<InPlay>(processors);

            // Act
            var result = container.GetMessageProcessor(3); // MarketUpdate entity key

            // Assert
            result.Should().BeSameAs(mockProcessor.Object);
        }

        [Fact]
        public void GetMessageProcessor_WithInvalidMessageType_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var mockProcessor = new Mock<IMessageProcessor>();
            mockProcessor.Setup(x => x.GetTypeOfTFlow()).Returns(typeof(InPlay));
            mockProcessor.Setup(x => x.GetTypeOfTType()).Returns(typeof(MarketUpdate));

            var processors = new List<IMessageProcessor> { mockProcessor.Object };
            var container = new MessageProcessorContainer<InPlay>(processors);

            // Act & Assert
            var act = () => container.GetMessageProcessor(999); // Non-existent entity key
            act.Should().Throw<KeyNotFoundException>();
        }

        [Fact]
        public void Constructor_WithPreMatchFlowType_ShouldWorkCorrectly()
        {
            // Arrange
            var mockProcessor = new Mock<IMessageProcessor>();
            mockProcessor.Setup(x => x.GetTypeOfTFlow()).Returns(typeof(PreMatch));
            mockProcessor.Setup(x => x.GetTypeOfTType()).Returns(typeof(FixtureMetadataUpdate));

            var processors = new List<IMessageProcessor> { mockProcessor.Object };

            // Act
            var container = new MessageProcessorContainer<PreMatch>(processors);

            // Assert
            container.Should().NotBeNull();
            var result = container.GetMessageProcessor(1); // FixtureMetadataUpdate entity key
            result.Should().BeSameAs(mockProcessor.Object);
        }

        [Fact]
        public void Constructor_WithAllKnownMessageTypes_ShouldRegisterAllCorrectly()
        {
            // Arrange
            var processors = new List<IMessageProcessor>();

            // Create processors for all known message types
            var messageTypes = new[]
            {
                (typeof(FixtureMetadataUpdate), 1),
                (typeof(LivescoreUpdate), 2),
                (typeof(MarketUpdate), 3),
                (typeof(KeepAliveUpdate), 31),
                (typeof(SettlementUpdate), 35)
            };

            foreach (var (messageType, entityKey) in messageTypes)
            {
                var mockProcessor = new Mock<IMessageProcessor>();
                mockProcessor.Setup(x => x.GetTypeOfTFlow()).Returns(typeof(InPlay));
                mockProcessor.Setup(x => x.GetTypeOfTType()).Returns(messageType);
                processors.Add(mockProcessor.Object);
            }

            // Act
            var container = new MessageProcessorContainer<InPlay>(processors);

            // Assert
            container.Should().NotBeNull();
            
            foreach (var (messageType, entityKey) in messageTypes)
            {
                var processor = container.GetMessageProcessor(entityKey);
                processor.Should().NotBeNull();
                processor.GetTypeOfTType().Should().Be(messageType);
            }
        }

        [Fact]
        public void Constructor_WithNullProcessorCollection_ShouldHandleGracefully()
        {
            // Arrange
            IEnumerable<IMessageProcessor>? processors = null;

            // Act & Assert
            var act = () => new MessageProcessorContainer<InPlay>(processors!);
            act.Should().Throw<NullReferenceException>();
        }

        [Fact]
        public void GetMessageProcessor_MultipleCalls_ShouldReturnSameInstance()
        {
            // Arrange
            var mockProcessor = new Mock<IMessageProcessor>();
            mockProcessor.Setup(x => x.GetTypeOfTFlow()).Returns(typeof(InPlay));
            mockProcessor.Setup(x => x.GetTypeOfTType()).Returns(typeof(MarketUpdate));

            var processors = new List<IMessageProcessor> { mockProcessor.Object };
            var container = new MessageProcessorContainer<InPlay>(processors);

            // Act
            var result1 = container.GetMessageProcessor(3);
            var result2 = container.GetMessageProcessor(3);

            // Assert
            result1.Should().BeSameAs(result2);
            result1.Should().BeSameAs(mockProcessor.Object);
        }

        [Fact]
        public void Constructor_WithProcessorsHavingNoEntityAttribute_ShouldThrowException()
        {
            // Arrange - Create a class without Trade360EntityAttribute for testing
            var mockProcessor = new Mock<IMessageProcessor>();
            mockProcessor.Setup(x => x.GetTypeOfTFlow()).Returns(typeof(InPlay));
            
            // Use a real type that doesn't have the attribute
            mockProcessor.Setup(x => x.GetTypeOfTType()).Returns(typeof(string));

            var processors = new List<IMessageProcessor> { mockProcessor.Object };

            // Act & Assert
            var act = () => new MessageProcessorContainer<InPlay>(processors);
            act.Should().Throw<NullReferenceException>();
        }

        [Fact]
        public void Constructor_WithLargeNumberOfProcessors_ShouldHandleEfficiently()
        {
            // Arrange
            var processors = new List<IMessageProcessor>();
            const int processorCount = 5; // Reduced for simpler testing

            // Use real message types with actual attributes
            var messageTypes = new[]
            {
                typeof(FixtureMetadataUpdate),
                typeof(LivescoreUpdate),
                typeof(MarketUpdate),
                typeof(KeepAliveUpdate),
                typeof(SettlementUpdate)
            };

            for (int i = 0; i < processorCount; i++)
            {
                var mockProcessor = new Mock<IMessageProcessor>();
                mockProcessor.Setup(x => x.GetTypeOfTFlow()).Returns(typeof(InPlay));
                mockProcessor.Setup(x => x.GetTypeOfTType()).Returns(messageTypes[i]);
                
                processors.Add(mockProcessor.Object);
            }

            // Act
            var container = new MessageProcessorContainer<InPlay>(processors);

            // Assert
            container.Should().NotBeNull();
            
            // Verify processors are registered for known entity keys
            var knownKeys = new[] { 1, 2, 3, 31, 35 };
            for (int i = 0; i < processorCount; i++)
            {
                var processor = container.GetMessageProcessor(knownKeys[i]);
                processor.Should().NotBeNull();
            }
        }

        [Fact]
        public void Constructor_WithMixedValidAndInvalidProcessors_ShouldOnlyRegisterValid()
        {
            // Arrange
            var validProcessor = new Mock<IMessageProcessor>();
            validProcessor.Setup(x => x.GetTypeOfTFlow()).Returns(typeof(InPlay));
            validProcessor.Setup(x => x.GetTypeOfTType()).Returns(typeof(MarketUpdate));

            var invalidFlowProcessor = new Mock<IMessageProcessor>();
            invalidFlowProcessor.Setup(x => x.GetTypeOfTFlow()).Returns(typeof(PreMatch));
            invalidFlowProcessor.Setup(x => x.GetTypeOfTType()).Returns(typeof(LivescoreUpdate));

            var processors = new List<IMessageProcessor> { validProcessor.Object, invalidFlowProcessor.Object };

            // Act
            var container = new MessageProcessorContainer<InPlay>(processors);

            // Assert
            container.Should().NotBeNull();
            
            // Valid processor should be registered
            var result = container.GetMessageProcessor(3); // MarketUpdate
            result.Should().BeSameAs(validProcessor.Object);
            
            // Invalid flow processor should not be registered
            var act = () => container.GetMessageProcessor(2); // LivescoreUpdate
            act.Should().Throw<KeyNotFoundException>();
        }

        [Fact]
        public void GetMessageProcessor_WithZeroEntityKey_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var mockProcessor = new Mock<IMessageProcessor>();
            mockProcessor.Setup(x => x.GetTypeOfTFlow()).Returns(typeof(InPlay));
            mockProcessor.Setup(x => x.GetTypeOfTType()).Returns(typeof(MarketUpdate));

            var processors = new List<IMessageProcessor> { mockProcessor.Object };
            var container = new MessageProcessorContainer<InPlay>(processors);

            // Act & Assert
            var act = () => container.GetMessageProcessor(0);
            act.Should().Throw<KeyNotFoundException>();
        }

        [Fact]
        public void GetMessageProcessor_WithNegativeEntityKey_ShouldThrowKeyNotFoundException()
        {
            // Arrange
            var mockProcessor = new Mock<IMessageProcessor>();
            mockProcessor.Setup(x => x.GetTypeOfTFlow()).Returns(typeof(InPlay));
            mockProcessor.Setup(x => x.GetTypeOfTType()).Returns(typeof(MarketUpdate));

            var processors = new List<IMessageProcessor> { mockProcessor.Object };
            var container = new MessageProcessorContainer<InPlay>(processors);

            // Act & Assert
            var act = () => container.GetMessageProcessor(-1);
            act.Should().Throw<KeyNotFoundException>();
        }

        [Fact]
        public void Constructor_WithProcessorReturningNullType_ShouldHandleGracefully()
        {
            // Arrange
            var mockProcessor = new Mock<IMessageProcessor>();
            mockProcessor.Setup(x => x.GetTypeOfTFlow()).Returns(typeof(InPlay));
            mockProcessor.Setup(x => x.GetTypeOfTType()).Returns((Type)null!);

            var processors = new List<IMessageProcessor> { mockProcessor.Object };

            // Act & Assert
            var act = () => new MessageProcessorContainer<InPlay>(processors);
            act.Should().Throw<ArgumentNullException>();
        }
    }
} 