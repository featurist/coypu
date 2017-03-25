using Coypu.Finders;
using Shouldly;
using Xunit;

namespace Coypu.Drivers.Tests
{
    internal class When_using_iframes_as_scope : DriverSpecs
    {
        [Fact]
        public void Does_not_find_something_in_an_iframe()
        {
            Assert.Throws<MissingHtmlException>(() => Id("iframe1ButtonId", Root, DefaultOptions));
        }

        [Fact(Skip = "Tried everything to work out why Selenium 3 is failing this test, something to do with how it switches between iframes. Have to do without this, it's pretty obscure anyway.")]
        public void Finds_elements_among_multiple_scopes()
        {
            Finds_elements_among_multiple_scopes(new FrameFinder(Driver, "I am iframe one", Root, DefaultOptions),
                                                 new FrameFinder(Driver, "I am iframe two", Root, DefaultOptions));
        }

        [Fact]
        public void Finds_elements_among_multiple_scopes_when_finding_by_css()
        {
            Finds_elements_among_multiple_scopes(new CssFinder(Driver, "iframe#iframe1", Root, DefaultOptions),
                                                 new CssFinder(Driver, "iframe#iframe2", Root, DefaultOptions));
        }

        [Fact]
        public void Finds_elements_among_multiple_scopes_when_finding_by_xpath()
        {
            Finds_elements_among_multiple_scopes(new XPathFinder(Driver, "//iframe[@id='iframe1']", Root, DefaultOptions),
                                                 new XPathFinder(Driver, "//iframe[@id='iframe2']", Root, DefaultOptions));
        }

        [Fact]
        public void Finds_elements_among_multiple_scopes_when_finding_by_id()
        {
            Finds_elements_among_multiple_scopes(new IdFinder(Driver, "iframe1", Root, DefaultOptions),
                                                 new IdFinder(Driver, "iframe2", Root, DefaultOptions));
        }

        private static void Finds_elements_among_multiple_scopes(ElementFinder elementFinder1, ElementFinder elementFinder2)
        {
            var iframeOne = new BrowserWindow(DefaultSessionConfiguration, elementFinder1, Driver, null, null, null, DisambiguationStrategy);
            var iframeTwo = new BrowserWindow(DefaultSessionConfiguration, elementFinder2, Driver, null, null, null, DisambiguationStrategy);

            Button("scoped button", iframeOne, DefaultOptions).Id.ShouldBe("iframe1ButtonId");
            Button("scoped button", iframeTwo, DefaultOptions).Id.ShouldBe("iframe2ButtonId");
        }

        [Fact]
        public void Finds_clear_scope_back_to_the_whole_window()
        {
            var iframeOne = new BrowserWindow(DefaultSessionConfiguration, new FrameFinder(Driver, "I am iframe one", Root, DefaultOptions), Driver,null,null,null,DisambiguationStrategy);
            Button("scoped button", iframeOne, DefaultOptions).Id.ShouldBe("iframe1ButtonId");

            Button("scoped button", Root, Options.PreferExact).Id.ShouldBe("scope1ButtonId");
        }

        [Fact]
        public void Can_fill_in_a_text_input_within_an_iframe()
        {
            var iframeOne = new BrowserWindow(DefaultSessionConfiguration, new FrameFinder(Driver, "I am iframe one", Root, DefaultOptions), Driver, null, null, null, DisambiguationStrategy);
            Driver.Set(FindField("text input in iframe", iframeOne), "filled in");
            
            Assert.Equal("filled in", FindField("text input in iframe", iframeOne).Value);
        }

        private static Element FindField(string locator, DriverScope scope)
        {
            return Field(locator, scope, DefaultOptions);
        }

        [Fact]
        public void Can_scope_around_an_iframe()
        {
            var iframeOne = new BrowserWindow(DefaultSessionConfiguration, new FrameFinder(Driver, "I am iframe one", Root,DefaultOptions), Driver, null, null, null, DisambiguationStrategy);
            Button("scoped button", iframeOne, DefaultOptions).Id.ShouldBe("iframe1ButtonId");

            var body = new BrowserWindow(DefaultSessionConfiguration, new CssFinder(Driver, "body", Root, DefaultOptions), Driver, null, null, null, DisambiguationStrategy);
            Button("scoped button", body, Options.PreferExact).Id.ShouldBe("scope1ButtonId");
        }

        [Fact]
        public void Can_scope_inside_an_iframe()
        {
            var iframeOne = new BrowserWindow(DefaultSessionConfiguration, new FrameFinder(Driver, "I am iframe one", Root, DefaultOptions), Driver, null, null, null, null);
            var iframeForm = new BrowserWindow(DefaultSessionConfiguration, new CssFinder(Driver, "form", iframeOne, DefaultOptions), Driver, null, null, null, null);
        }
    }
}