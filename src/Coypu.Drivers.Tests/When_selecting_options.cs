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
            GetFieldScope().Now().SelectedOption.should_be("select two option one");

            Driver.Click(FindSingle(new OptionFinder(Driver, "select two option two", GetFieldScope(), DefaultOptions)));

            GetFieldScope().Now().SelectedOption.should_be("select two option two");
        }

        [Test]
        public void Selected_option_respects_exact()
        {
            Assert.That(
                FindSingle(new OptionFinder(Driver, "select two option t", GetFieldScope(), PartialOptions)).Text,
                Is.EqualTo("select two option two"));

            Assert.Throws<MissingHtmlException>(
                () => FindSingle(new OptionFinder(Driver, "select two option t", GetFieldScope(), ExactOptions)));
        }

        private static DriverScope GetFieldScope()
        {
            var fieldScope = new DriverScope(DefaultSessionConfiguration,
                                             new FieldFinder(Driver, "containerLabeledSelectFieldId", Root, DefaultOptions),
                                             Driver, null, null,
                                             null, DisambiguationStrategy);
            return fieldScope;
        }
    }
    
}