using Shouldly;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_iframes : DriverSpecs
    {
        [Test]
        public void Finds_by_header_text()
        {
            Frame("I am iframe one").Id.ShouldBe("iframe1");
            Frame("I am iframe two").Id.ShouldBe("iframe2");
        }

        [Test]
        public void Finds_by_id()
        {
            Frame("iframe1").Id.ShouldBe("iframe1");
            Frame("iframe2").Id.ShouldBe("iframe2");
        }

        [Test]
        public void Finds_by_title()
        {
            Frame("iframe one title").Id.ShouldBe("iframe1");
            Frame("iframe two title").Id.ShouldBe("iframe2");
        }
    }
}
