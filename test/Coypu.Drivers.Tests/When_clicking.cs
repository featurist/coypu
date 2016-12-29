using System.Linq;
using Coypu.Finders;
using NSpec;
using Xunit;

namespace Coypu.Drivers.Tests
{
    public class When_clicking : DriverSpecs
    {
        [Fact]
        public void Clicks_the_underlying_element()
        {
            var element = Button("clickMeTest");
            element.Value.should_be("Click me");

            Driver.Click(element);

            Button("clickMeTest").Value.should_be("Click me - clicked");
        }
    }
}