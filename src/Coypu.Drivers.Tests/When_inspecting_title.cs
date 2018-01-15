using Shouldly;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_inspecting_title : DriverSpecs
    {
        [Test]
        public void Gets_the_current_page_title()
        {
            Driver.Title(Root).ShouldBe("Coypu interaction tests page");
        }
    }
}
