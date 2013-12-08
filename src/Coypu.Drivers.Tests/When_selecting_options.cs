using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_selecting_options : DriverSpecs
    {
        [Test]
        public void Sets_text_of_selected_option()
        {
            var textField = Field("containerLabeledSelectFieldId");
            textField.SelectedOption.should_be("select two option one");

            Driver.Select(textField, "select2value2");

            textField = Field("containerLabeledSelectFieldId");
            textField.SelectedOption.should_be("select two option two");
        }
    }
    
}