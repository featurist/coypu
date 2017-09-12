using Coypu.Finders;
using Shouldly;
using Xunit;

namespace Coypu.Drivers.Tests
{
    public class When_selecting_options : DriverSpecs
    {
        private static DriverScope GetSelectScope(string locator)
        {
            var select = new BrowserWindow(DefaultSessionConfiguration,
                                         new SelectFinder(Driver, locator, Root, DefaultOptions), Driver,
                                         null, null, null, DisambiguationStrategy);
            return @select;
        }

        [Fact]
        public void Sets_text_of_selected_option()
        {
            Field("containerLabeledSelectFieldId").SelectedOption.ShouldBe("select two option one");

            Driver.Click(FindSingle(new OptionFinder(Driver, "select two option two", GetSelectScope("containerLabeledSelectFieldId"), DefaultOptions)));

            Field("containerLabeledSelectFieldId").SelectedOption.ShouldBe("select two option two");
        }

        [Fact]
        public void Selected_option_respects_TextPrecision()
        {
            Assert.Equal("select two option two", 
                FindSingle(new OptionFinder(Driver, "select two option t", GetSelectScope("containerLabeledSelectFieldId"), Options.Substring)).Text);

            Assert.Equal("select two option two", 
                FindSingle(new OptionFinder(Driver, "select two option two", GetSelectScope("containerLabeledSelectFieldId"), Options.Exact)).Text);

            Assert.Throws<MissingHtmlException>(
                () => FindSingle(new OptionFinder(Driver, "select two option t", GetSelectScope("containerLabeledSelectFieldId"), Options.Exact)));
        }

        [Fact]
        public void Selected_option_finds_exact_by_container_label()
        {
            Assert.Equal("one", FindSingle(new OptionFinder(Driver, "one", GetSelectScope("Ambiguous select options"), Options.Exact)).Text);
        }

        [Fact]
        public void Selected_option_finds_substring_by_container_label()
        {
            Assert.Equal("one", FindSingle(new OptionFinder(Driver, "one", GetSelectScope("Ambiguous select options"), Options.Substring)).Text);
        }
    }
    
}