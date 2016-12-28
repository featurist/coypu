using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_iframes : DriverSpecs
    {
        [Test]
        public void Finds_by_header_text()
        {
            Frame("I am iframe one").Id.should_be("iframe1");
            Frame("I am iframe two").Id.should_be("iframe2");
        }

        [Test]
        public void Finds_by_id()
        {
            Frame("iframe1").Id.should_be("iframe1");
            Frame("iframe2").Id.should_be("iframe2");
        }

        [Test]
        public void Finds_by_title()
        {
            Frame("iframe one title").Id.should_be("iframe1");
            Frame("iframe two title").Id.should_be("iframe2");
        }
    }
}