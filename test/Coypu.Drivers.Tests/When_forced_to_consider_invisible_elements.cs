using Xunit;

namespace Coypu.Drivers.Tests
{
    public class When_forced_to_find_invisible_elements : DriverSpecs
    {
        [Fact]
        public void Does_find_hidden_inputs()
        {
            Assert.Equal("first hidden input", Field("firstHiddenInputId", options : Options.Invisible).Value);

            Assert.Throws<MissingHtmlException>(() => Field("firstHiddenInputId"));
        }

        [Fact]
        public void Does_find_invisible_elements()
        {
            Assert.Equal("firstInvisibleInputName", Button("firstInvisibleInputId", options: Options.Invisible).Name);

            Assert.Throws<MissingHtmlException>(() => Button("firstInvisibleInputId"));
        }

        //[Fact, Explicit("Only works in WatiN")]
        //public void It_can_find_invisible_elements_by_text()
        //{
        //    Assert.That(Css("#firstInvisibleSpanId",new Regex("I am an invisible span"), options: Options.Invisible).Name,
        //        Is.EqualTo("firstInvisibleSpanName"));
        //}

        //[Fact, Explicit("Only works this way in WebDriver")]
        //public void Explains_it_cannot_find_invisible_elements_by_text()
        //{
        //    Assert.Throws<NotSupportedException>(() =>
        //        Css("#firstInvisibleSpanId", new Regex("I am an invisible span"), options: Options.Invisible));
        //}
    }
}