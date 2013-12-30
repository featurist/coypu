using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    public class When_finding_ids : DriverSpecs
    {
        [Test]
        public void Finds_element_by_id()
        {
            Id("firstLinkId").Id.should_be("firstLinkId");
            Id("secondLinkId").Id.should_be("secondLinkId");
        }

        [Test]
        public void Does_not_find_display_none()
        {
            Assert.Throws<MissingHtmlException>(() => Id("invisibleLinkByDisplayId"));
        }

        [Test]
        public void Does_not_find_visibility_hidden_links()
        {
            Assert.Throws<MissingHtmlException>(() => Id("invisibleLinkByVisibilityId"));
        }

        [Test]
        public void Ignores_exact_option()
        {
            Assert.Throws<MissingHtmlException>(() => Id("firstLink", options: PartialOptions));
            Assert.Throws<MissingHtmlException>(() => Id("firstLink", options: ExactOptions));
        }
    }
}