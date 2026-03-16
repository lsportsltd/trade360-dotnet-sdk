using FluentAssertions;
using Trade360SDK.BetBuilderApi.Entities.BetBuilderApi.Responses;
using Xunit;

namespace Trade360SDK.BetBuilderApi.Tests.Entities.BetBuilderApi.Responses
{
    public class BetBuilderMarginedTests
    {
        [Fact]
        public void BetBuilderMargined_DefaultConstructor_ShouldCreateInstanceWithDefaultValues()
        {
            var margined = new BetBuilderMargined();

            margined.Should().NotBeNull();
            margined.Status.Should().BeNull();
            margined.Pcb.Should().BeNull();
            margined.RevenueTax.Should().BeNull();
            margined.Value.Should().Be(0);
        }

        [Fact]
        public void BetBuilderMargined_SetProperties_ShouldReturnCorrectValues()
        {
            var pcb = new[] { 1.0, 2.0 };

            var margined = new BetBuilderMargined
            {
                Status = "OK",
                Pcb = pcb,
                RevenueTax = 0.15,
                Value = 2.5
            };

            margined.Status.Should().Be("OK");
            margined.Pcb.Should().BeSameAs(pcb);
            margined.RevenueTax.Should().Be(0.15);
            margined.Value.Should().Be(2.5);
        }
    }
}
