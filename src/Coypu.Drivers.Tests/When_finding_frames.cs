using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_frames : DriverSpecs
    {
        [Test]
        public void Finds_by_header_text()
        {
            Driver.FindFrame("I am frame one", Root).Id.should_be("frame1");
            Driver.FindFrame("I am frame two", Root).Id.should_be("frame2");
        }

        [Test]
        public void Finds_by_id()
        {
            Driver.FindFrame("frame1", Root).Id.should_be("frame1");
            Driver.FindFrame("frame2", Root).Id.should_be("frame2");
        }

        [Test]
        public void Finds_by_title()
        {
            Driver.FindFrame("frame one title", Root).Id.should_be("frame1");
            Driver.FindFrame("frame two title", Root).Id.should_be("frame2");
        }
    }
}