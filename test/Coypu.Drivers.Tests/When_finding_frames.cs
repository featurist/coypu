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
            Frame("I am frame one").Name.should_be("frame1");
            Frame("I am frame two").Name.should_be("frame2");
        }

        [Test]
        public void Finds_by_name()
        {
            Frame("frame1").Name.should_be("frame1");
            Frame("frame2").Name.should_be("frame2");
        }

        [Test]
        public void Finds_by_id()
        {
            Frame("frame1id").Name.should_be("frame1");
            Frame("frame2id").Name.should_be("frame2");
        }
    }
}