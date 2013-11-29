using System;
using System.Text.RegularExpressions;
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
            Assert.That(new ButtonFinder(Driver,"firstInvisibleInputId", RootConsideringInvisibleElements).Find().Name, Is.EqualTo("firstInvisibleInputName"));

            Assert.Throws<MissingHtmlException>(() => new ButtonFinder(Driver,"firstInvisibleInputId", Root).Find());
        }

        [Test, Explicit("Only works in WatiN")]
        public void It_can_find_invisible_elements_by_text()
        {
            Assert.That(Driver.FindCss("#firstInvisibleSpanId", RootConsideringInvisibleElements,new Regex("I am an invisible span")).Name,
                Is.EqualTo("firstInvisibleSpanName"));
        }

        [Test, Explicit("Only works this way in WebDriver")]
        public void Explains_it_cannot_find_invisible_elements_by_text()
        {
            Assert.Throws<NotSupportedException>(() =>
                Driver.FindCss("#firstInvisibleSpanId", RootConsideringInvisibleElements, new Regex("I am an invisible span")));
        }
    }
}