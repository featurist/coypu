using NSpec;

namespace Coypu.Drivers.Tests
{
    public class When_hovering : DriverSpecs
    {
        internal override void Specs()
        {
            it["mouses over the underlying element"] = () =>
            {
                var element = driver.FindId("hoverOnMeTest");
                driver.FindId("hoverOnMeTest").Text.should_be("Hover on me");
                driver.Hover(element);
                driver.FindId("hoverOnMeTest").Text.should_be("Hover on me - hovered");
            };
        }
    }
}