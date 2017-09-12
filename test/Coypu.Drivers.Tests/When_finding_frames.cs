using Shouldly;
using Xunit;

namespace Coypu.Drivers.Tests
{
    public class When_finding_frames : DriverSpecs
    {
        protected override string TestPage
        {
            get { return @"html\frameset.htm"; }
        }

        [Fact]
        public void Finds_by_header_text()
        {
            Frame("I am frame one").Name.ShouldBe("frame1");
            Frame("I am frame two").Name.ShouldBe("frame2");
        }

        [Fact]
        public void Finds_by_name()
        {
            Frame("frame1").Name.ShouldBe("frame1");
            Frame("frame2").Name.ShouldBe("frame2");
        }

        [Fact]
        public void Finds_by_id()
        {
            Frame("frame1id").Name.ShouldBe("frame1");
            Frame("frame2id").Name.ShouldBe("frame2");
        }
    }
}