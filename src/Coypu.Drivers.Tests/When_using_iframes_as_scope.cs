using Coypu.Finders;
using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_using_iframes_as_scope : DriverSpecs
    {
        [Test]
        public void Does_not_find_something_in_an_iframe()
        {
            Assert.Throws<MissingHtmlException>(() => Driver.FindId("iframe1ButtonId", Root));
        }

        [Test]
        public void Finds_elements_among_multiple_scopes()
        {
            Finds_elements_among_multiple_scopes(new FrameFinder(Driver, "I am iframe one", Root),
                                                 new FrameFinder(Driver, "I am iframe two", Root));
        }

        [Test]
        public void Finds_elements_among_multiple_scopes_when_finding_by_css()
        {
            Finds_elements_among_multiple_scopes(new CssFinder(Driver, "iframe#iframe1", Root),
                                                 new CssFinder(Driver, "iframe#iframe2", Root));
        }

        [Test]
        public void Finds_elements_among_multiple_scopes_when_finding_by_xpath()
        {
            Finds_elements_among_multiple_scopes(new XPathFinder(Driver, "//iframe[@id='iframe1']", Root),
                                                 new XPathFinder(Driver, "//iframe[@id='iframe2']", Root));
        }

        [Test]
        public void Finds_elements_among_multiple_scopes_when_finding_by_id()
        {
            Finds_elements_among_multiple_scopes(new IdFinder(Driver, "iframe1", Root),
                                                 new IdFinder(Driver, "iframe2", Root));
        }

        private static void Finds_elements_among_multiple_scopes(ElementFinder elementFinder1, ElementFinder elementFinder2)
        {
            var iframeOne = new DriverScope(new SessionConfiguration(), elementFinder1, Driver,
                                            null, null, null);
            var iframeTwo = new DriverScope(new SessionConfiguration(), elementFinder2, Driver,
                                            null, null, null);

            new ButtonFinder(Driver, "scoped button", iframeOne).Find().Id.should_be("iframe1ButtonId");
            new ButtonFinder(Driver, "scoped button", iframeTwo).Find().Id.should_be("iframe2ButtonId");
        }

        [Test]
        public void Finds_clear_scope_back_to_the_whole_window()
        {
            var iframeOne = new DriverScope(new SessionConfiguration(), new FrameFinder(Driver, "I am iframe one", Root), Driver,null,null,null);
            new ButtonFinder(Driver, "scoped button", iframeOne).Find().Id.should_be("iframe1ButtonId");

            new ButtonFinder(Driver, "scoped button", Root).Find().Id.should_be("scope1ButtonId");
        }

        [Test]
        public void Can_fill_in_a_text_input_within_an_iframe()
        {
            var iframeOne = new DriverScope(new SessionConfiguration(), new FrameFinder(Driver, "I am iframe one", Root), Driver, null, null, null);
            Driver.Set(Driver.FindField("text input in iframe", iframeOne), "filled in");

            Assert.That(Driver.FindField("text input in iframe", iframeOne).Value, Is.EqualTo("filled in"));
        }

        [Test]
        public void Can_scope_around_an_iframe()
        {
            var body = new DriverScope(new SessionConfiguration(), new CssFinder(Driver, "body", Root), Driver, null, null, null);
            var iframeOne = new DriverScope(new SessionConfiguration(), new FrameFinder(Driver, "I am iframe one", body), Driver, null, null, null);

            new ButtonFinder(Driver, "scoped button", iframeOne).Find().Id.should_be("iframe1ButtonId");

            new ButtonFinder(Driver, "scoped button", body).Find().Id.should_be("scope1ButtonId");
        }

        [Test]
        public void Can_scope_inside_an_iframe()
        {
            var iframeOne = new DriverScope(new SessionConfiguration(), new FrameFinder(Driver, "I am iframe one", Root), Driver, null, null, null);
            var iframeForm = new DriverScope(new SessionConfiguration(), new CssFinder(Driver, "form", iframeOne), Driver, null, null, null);
            
            Driver.FindField("text input in iframe", iframeForm);
        }
    }
}