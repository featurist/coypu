using Coypu.Finders;
using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    public class When_clicking : DriverSpecs
    {
        [Test]
        public void Clicks_the_underlying_element()
        {
            var element = new ButtonFinder(Driver,"clickMeTest", Root).Find();
            new ButtonFinder(Driver, "clickMeTest", Root).Find().Value.should_be("Click me");
            Driver.Click(element);
            new ButtonFinder(Driver, "clickMeTest", Root).Find().Value.should_be("Click me - clicked");
        }
    }
}