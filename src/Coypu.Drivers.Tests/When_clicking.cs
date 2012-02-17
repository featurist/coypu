using NSpec;

namespace Coypu.Drivers.Tests
{
    public class When_clicking : DriverSpecs
    {
        internal override void Specs()
        {
            it["clicks the underlying element"] = () =>
            {
                var element = driver.FindButton("clickMeTest", Root);
                driver.FindButton("clickMeTest", Root).Value.should_be("Click me");
                driver.Click(element);
                driver.FindButton("clickMeTest", Root).Value.should_be("Click me - clicked");
            };
        }
    }
}