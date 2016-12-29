using NSpec;
using Xunit;

namespace Coypu.Drivers.Tests
{
    public class When_finding_ids : DriverSpecs
    {
        [Fact]
        public void Finds_element_by_id()
        {
            Id("firstLinkId").Id.should_be("firstLinkId");
            Id("secondLinkId").Id.should_be("secondLinkId");
        }

        [Fact]
        public void Does_not_find_display_none()
        {
            Assert.Throws<MissingHtmlException>(() => Id("invisibleLinkByDisplayId"));
        }

        [Fact]
        public void Does_not_find_visibility_hidden_links()
        {
            Assert.Throws<MissingHtmlException>(() => Id("invisibleLinkByVisibilityId"));
        }

        [Fact]
        public void Ignores_exact_option()
        {
            Assert.Throws<MissingHtmlException>(() => Id("firstLink", options: Options.Substring));
            Assert.Throws<MissingHtmlException>(() => Id("firstLink", options: Options.Exact));
        }
    }
}