using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_frames : DriverSpecs
    {
        protected override string TestPage
        {
            get { return @"html\frameset.htm"; }
        }

        [Test]
        public void Finds_by_header_text()
        {
            Driver.FindFrame("I am frame one", Root).Name.should_be("frame1");
            Driver.FindFrame("I am frame two", Root).Name.should_be("frame2");
        }

        [Test]
        public void Finds_by_name()
        {
            Driver.FindFrame("frame1", Root).Name.should_be("frame1");
            Driver.FindFrame("frame2", Root).Name.should_be("frame2");
        }

        [Test]
        public void Finds_by_id()
        {
            Driver.FindFrame("frame1id", Root).Name.should_be("frame1");
            Driver.FindFrame("frame2id", Root).Name.should_be("frame2");
        }
    }
}