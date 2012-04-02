using Coypu.Finders;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    public class When_forced_to_find_invisible_elements : DriverSpecs
    {
        private static DriverScope RootConsideringInvisibleElements
        {
            get
            {
                var configuration = new SessionConfiguration();
                configuration.ConsiderInvisibleElements = true;

                var rootConsideringInvisibleElements = new DriverScope(configuration, new DocumentElementFinder(Driver), null, null, null, null);
                return rootConsideringInvisibleElements;
            }
        }

        [Test]
        public void Does_find_hidden_inputs()
        {
            Assert.That(Driver.FindField("firstHiddenInputId", RootConsideringInvisibleElements).Value, Is.EqualTo("first hidden input"));

            Assert.Throws<MissingHtmlException>(() => Driver.FindField("firstHiddenInputId", Root));
        }


        [Test]
        public void Does_find_invisible_elements()
        {
            Assert.That(Driver.FindButton("firstInvisibleInputId", RootConsideringInvisibleElements).Name, Is.EqualTo("firstInvisibleInputName"));

            Assert.Throws<MissingHtmlException>(() => Driver.FindButton("firstInvisibleInputId", Root));
        }
    }
}