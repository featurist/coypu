using Coypu.Finders;
using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_selecting_options : DriverSpecs
    {
        [Test]
        public void Sets_text_of_selected_option()
        {
            Field("containerLabeledSelectFieldId").SelectedOption.should_be("select two option one");

            Driver.Click(FindSingle(new OptionFinder(Driver, "containerLabeledSelectFieldId", "select two option two", Root, DefaultOptions)));

            Field("containerLabeledSelectFieldId").SelectedOption.should_be("select two option two");
        }

        [Test]
        public void Selected_option_respects_exact()
        {
            Assert.That(
                FindSingle(new OptionFinder(Driver, "containerLabeledSelectFieldId", "select two option t", Root, Options.Substring)).Text,
                Is.EqualTo("select two option two"));

            Assert.Throws<MissingHtmlException>(
                () => FindSingle(new OptionFinder(Driver, "containerLabeledSelectFieldId", "select two option t", Root, Options.Exact)));
        }
    }
    
}