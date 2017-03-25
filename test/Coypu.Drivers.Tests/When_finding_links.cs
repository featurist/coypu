using Shouldly;
using Xunit;

namespace Coypu.Drivers.Tests
{
    public class When_finding_links : DriverSpecs
    {
        [Fact]
        public void Finds_link_by_text()
        {
            Link("first link").Id.ShouldBe("firstLinkId");
            Link("second link").Id.ShouldBe("secondLinkId");
        }

        [Fact]
        public void Does_not_find_display_none()
        {
            Assert.Throws<MissingHtmlException>(() => Link("I am an invisible link by display"));
        }

        [Fact]
        public void Does_not_find_visibility_hidden_links()
        {
            Assert.Throws<MissingHtmlException>(() => Link("I am an invisible link by visibility"));
        }

        [Fact]
        public void Finds_a_link_with_both_types_of_quote_in_its_text()
        {
            Assert.Equal("linkWithBothQuotesId", Link("I'm a link with \"both\" types of quote in my text").Id);
        }
    }
}