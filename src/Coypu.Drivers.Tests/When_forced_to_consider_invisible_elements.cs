using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    public class When_forced_to_find_invisible_elements : DriverSpecs
    {
        [Test]
        public void Does_find_hidden_inputs()
        {
            Root.ConsideringInvisibleElements();
            try
            {
                Assert.That(Driver.FindField("firstHiddenInputId", Root).Value, Is.EqualTo("first hidden input"));
            }
            finally
            {
                Root.ConsideringOnlyVisibleElements();
            }
            Assert.Throws<MissingHtmlException>(() => Driver.FindField("firstHiddenInputId", Root));
        }


        [Test]
        public void Does_find_invisible_elements()
        {
            Root.ConsideringInvisibleElements();
            try
            {
                Assert.That(Driver.FindButton("firstInvisibleInputId", Root).Name, Is.EqualTo("firstInvisibleInputName"));
            }
            finally
            {
                Root.ConsideringOnlyVisibleElements();
            }
            Assert.Throws<MissingHtmlException>(() => Driver.FindButton("firstInvisibleInputId", Root));
        }
    }
}