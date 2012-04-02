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
            Assert.Throws<MissingHtmlException>(() => Driver.FindButton("iframe1ButtonId", Root));
        }

        [Test]
        public void Finds_elements_among_multiple_scopes()
        {
            var iframeOne = new DriverScope(new SessionConfiguration(), new IFrameFinder(Driver, "I am iframe one", Root), Driver,null,null,null);
            var iframeTwo = new DriverScope(new SessionConfiguration(), new IFrameFinder(Driver, "I am iframe two", Root), Driver,null,null,null);

            Driver.FindButton("scoped button", iframeOne).Id.should_be("iframe1ButtonId");
            Driver.FindButton("scoped button", iframeTwo).Id.should_be("iframe2ButtonId");
        }

        [Test]
        public void Finds_clears_scope_back_to_the_whole_window()
        {
            var iframeOne = new DriverScope(new SessionConfiguration(), new IFrameFinder(Driver, "I am iframe one", Root), Driver,null,null,null);
            Driver.FindButton("scoped button", iframeOne).Id.should_be("iframe1ButtonId");

            Driver.FindButton("scoped button", Root).Id.should_be("scope1ButtonId");
        }

        [Test]
        public void Can_fill_in_a_text_input_within_an_iframe()
        {
            var iframeOne = new DriverScope(new SessionConfiguration(), new IFrameFinder(Driver, "I am iframe one", Root), Driver, null, null, null);
            Driver.Set(Driver.FindField("text input in iframe", iframeOne), "filled in");

            Assert.That(Driver.FindField("text input in iframe", iframeOne).Value, Is.EqualTo("filled in"));
        }

        [Test]
        public void Can_scope_around_an_iframe()
        {
            var body = new DriverScope(new SessionConfiguration(), new CssFinder(Driver, "body", Root), Driver, null, null, null);
            var iframeOne = new DriverScope(new SessionConfiguration(), new IFrameFinder(Driver, "I am iframe one", body), Driver, null, null, null);

            Driver.FindButton("scoped button", iframeOne).Id.should_be("iframe1ButtonId");

            Driver.FindButton("scoped button", body).Id.should_be("scope1ButtonId");
        }

        [Test]
        public void Can_scope_inside_an_iframe()
        {
            var iframeOne = new DriverScope(new SessionConfiguration(), new IFrameFinder(Driver, "I am iframe one", Root), Driver, null, null, null);
            var iframeForm = new DriverScope(new SessionConfiguration(), new CssFinder(Driver, "form", iframeOne), Driver, null, null, null);

            Driver.FindField("text input in iframe", iframeForm);
        }
    }
}