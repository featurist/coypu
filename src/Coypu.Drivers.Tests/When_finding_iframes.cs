using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_iframes : DriverSpecs
    {
        [Test]
        public void Finds_by_header_text()
        {
            Driver.FindFrame("I am iframe one", Root).Id.should_be("iframe1");
            Driver.FindFrame("I am iframe two", Root).Id.should_be("iframe2");
        }

        [Test]
        public void Finds_by_id()
        {
            Driver.FindFrame("iframe1", Root).Id.should_be("iframe1");
            Driver.FindFrame("iframe2", Root).Id.should_be("iframe2");
        }

        [Test]
        public void Finds_by_title()
        {
            Driver.FindFrame("iframe one title", Root).Id.should_be("iframe1");
            Driver.FindFrame("iframe two title", Root).Id.should_be("iframe2");
        }
    }
}