using System.Linq;
using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    public class When_hovering : DriverSpecs
    {
        [Test]
        public void Mouses_over_the_underlying_element()

        {
            var element = Driver.FindId("hoverOnMeTest", Root, DefaultOptions).Single();
            element.Text.should_be("Hover on me");
            Driver.Hover(element);

            Driver.FindId("hoverOnMeTest", Root, DefaultOptions).Single().Text.should_be("Hover on me - hovered");
        }
    }
}