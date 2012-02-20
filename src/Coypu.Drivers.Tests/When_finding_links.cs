using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    public class When_finding_links : DriverSpecs
    {
        [Test]
        public void Finds_link_by_text()
        {
            Driver.FindLink("first link", Root).Id.should_be("firstLinkId");
            Driver.FindLink("second link", Root).Id.should_be("secondLinkId");
        }

        [Test]
        public void Does_not_find_display_none()
        {
            Assert.Throws<MissingHtmlException>(() => Driver.FindLink("I am an invisible link by display", Root));
        }


        [Test]
        public void Does_not_find_visibility_hidden_links()
        {
            Assert.Throws<MissingHtmlException>(() => Driver.FindLink("I am an invisible link by visibility", Root));
        }


        [Test]
        public void Finds_a_link_with_both_types_of_quote_in_its_text()
        {
            var link = Driver.FindLink("I'm a link with \"both\" types of quote in my text", Root);
            Assert.That(link.Id, Is.EqualTo("linkWithBothQuotesId"));
        }
    }
}