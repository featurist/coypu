using NSpec;
using Xunit;

namespace Coypu.Drivers.Tests
{
    public class When_hovering : DriverSpecs
    {
        [Fact]
        public void Mouses_over_the_underlying_element()

        {
            var element = Id("hoverOnMeTest");
            element.Text.should_be("Hover on me");
            Driver.Hover(element);

            Id("hoverOnMeTest").Text.should_be("Hover on me - hovered");
        }
    }
}