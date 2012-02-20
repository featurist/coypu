using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_iframes : DriverSpecs
    {
        [Test]
        public void Finds_by_header_text()
        {
            Driver.FindIFrame("I am iframe one", Root).Id.should_be("iframe1");
            Driver.FindIFrame("I am iframe two", Root).Id.should_be("iframe2");
        }

        [Test]
        public void Finds_by_id()
        {
            Driver.FindIFrame("iframe1", Root).Id.should_be("iframe1");
            Driver.FindIFrame("iframe2", Root).Id.should_be("iframe2");
        }

        [Test]
        public void Finds_by_title()
        {
            Driver.FindIFrame("iframe one title", Root).Id.should_be("iframe1");
            Driver.FindIFrame("iframe two title", Root).Id.should_be("iframe2");
        }
    }
}