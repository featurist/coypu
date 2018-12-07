using System;
using System.Text.RegularExpressions;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    public class When_forced_to_find_invisible_elements : DriverSpecs
    {
        [Test]
        public void Does_find_hidden_inputs()
        {
            Assert.That(Field("firstHiddenInputId", options: Options.Invisible)
                            .Value,
                        Is.EqualTo("first hidden input"));
            Assert.Throws<MissingHtmlException>(() => Field("firstHiddenInputId"));
        }

        [Test]
        public void Does_find_invisible_elements()
        {
            Assert.That(Button("firstInvisibleInputId", options: Options.Invisible)
                            .Name,
                        Is.EqualTo("firstInvisibleInputName"));
            Assert.Throws<MissingHtmlException>(() => Button("firstInvisibleInputId"));
        }

        [Test]
        public void Explains_it_cannot_find_invisible_elements_by_text()
        {
            Assert.Throws<NotSupportedException>(() => Css("#firstInvisibleSpanId", new Regex("I am an invisible span"), options: Options.Invisible));
        }
    }
}