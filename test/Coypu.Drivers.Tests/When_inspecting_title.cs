using NSpec;
using Xunit;

namespace Coypu.Drivers.Tests
{
    internal class When_inspecting_title : DriverSpecs
    {
        [Fact]
        public void Gets_the_current_page_title()
        {
            Driver.Title(Root).should_be("Coypu interaction tests page");
        }
    }
}
