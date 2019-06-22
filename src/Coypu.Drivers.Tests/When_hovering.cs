using Shouldly;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    public class When_hovering : DriverSpecs
    {
        //Broken in firefox & edge due to the driver not scrolling when moving to an element
        [Test]
        public void Mouses_over_the_underlying_element()
        {
            var element = Id("hoverOnMeTest");
            element.Text.ShouldBe("Hover on me");
            Driver.Hover(element);

            Id("hoverOnMeTest").Text.ShouldBe("Hover on me - hovered");
        }
    }
}