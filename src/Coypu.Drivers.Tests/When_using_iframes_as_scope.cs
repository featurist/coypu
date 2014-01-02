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
            Assert.Throws<MissingHtmlException>(() => Id("iframe1ButtonId", Root, DefaultOptions));
        }

        [Test]
        public void Finds_elements_among_multiple_scopes()
        {
            Finds_elements_among_multiple_scopes(new FrameFinder(Driver, "I am iframe one", Root, DefaultOptions),
                                                 new FrameFinder(Driver, "I am iframe two", Root, DefaultOptions));
        }

        [Test]
        public void Finds_elements_among_multiple_scopes_when_finding_by_css()
        {
            Finds_elements_among_multiple_scopes(new CssFinder(Driver, "iframe#iframe1", Root, DefaultOptions),
                                                 new CssFinder(Driver, "iframe#iframe2", Root, DefaultOptions));
        }

        [Test]
        public void Finds_elements_among_multiple_scopes_when_finding_by_xpath()
        {
            Finds_elements_among_multiple_scopes(new XPathFinder(Driver, "//iframe[@id='iframe1']", Root, DefaultOptions),
                                                 new XPathFinder(Driver, "//iframe[@id='iframe2']", Root, DefaultOptions));
        }

        [Test]
        public void Finds_elements_among_multiple_scopes_when_finding_by_id()
        {
            Finds_elements_among_multiple_scopes(new IdFinder(Driver, "iframe1", Root, DefaultOptions),
                                                 new IdFinder(Driver, "iframe2", Root, DefaultOptions));
        }

        private static void Finds_elements_among_multiple_scopes(ElementFinder elementFinder1, ElementFinder elementFinder2)
        {
            var iframeOne = new DriverScope(DefaultSessionConfiguration, elementFinder1, Driver, null, null, null, DisambiguationStrategy);
            var iframeTwo = new DriverScope(DefaultSessionConfiguration, elementFinder2, Driver, null, null, null, DisambiguationStrategy);

            Button("scoped button", iframeOne, DefaultOptions).Id.should_be("iframe1ButtonId");
            Button("scoped button", iframeTwo, DefaultOptions).Id.should_be("iframe2ButtonId");
        }

        [Test]
        public void Finds_clear_scope_back_to_the_whole_window()
        {
            var iframeOne = new DriverScope(DefaultSessionConfiguration, new FrameFinder(Driver, "I am iframe one", Root, DefaultOptions), Driver,null,null,null,DisambiguationStrategy);
            Button("scoped button", iframeOne, DefaultOptions).Id.should_be("iframe1ButtonId");

            Button("scoped button", Root, Options.PreferExact).Id.should_be("scope1ButtonId");
        }

        [Test]
        public void Can_fill_in_a_text_input_within_an_iframe()
        {
            var iframeOne = new DriverScope(DefaultSessionConfiguration, new FrameFinder(Driver, "I am iframe one", Root, DefaultOptions), Driver, null, null, null, DisambiguationStrategy);
            Driver.Set(FindField("text input in iframe", iframeOne), "filled in");
            
            Assert.That(FindField("text input in iframe", iframeOne).Value, Is.EqualTo("filled in"));
        }

        private static ElementFound FindField(string locator, DriverScope scope)
        {
            return Field(locator, scope, DefaultOptions);
        }

        [Test]
        public void Can_scope_around_an_iframe()
        {
            var body = new DriverScope(DefaultSessionConfiguration, new CssFinder(Driver, "body", Root, DefaultOptions), Driver, null, null, null, DisambiguationStrategy);

            var iframeOne = new DriverScope(DefaultSessionConfiguration, new FrameFinder(Driver, "I am iframe one", body,DefaultOptions), Driver, null, null, null, DisambiguationStrategy);

            Button("scoped button", iframeOne, DefaultOptions).Id.should_be("iframe1ButtonId");

            Button("scoped button", body, Options.PreferExact).Id.should_be("scope1ButtonId");
        }

        [Test]
        public void Can_scope_inside_an_iframe()
        {
            var iframeOne = new DriverScope(DefaultSessionConfiguration, new FrameFinder(Driver, "I am iframe one", Root, DefaultOptions), Driver, null, null, null, null);
            var iframeForm = new DriverScope(DefaultSessionConfiguration, new CssFinder(Driver, "form", iframeOne, DefaultOptions), Driver, null, null, null, null);
        }
    }
}