using NSpec;
using Xunit;

namespace Coypu.Drivers.Tests
{
    internal class When_sending_keys_to_elements : DriverSpecs
    {
        [Fact]
        public void Sets_value_of_text_input_field_with_id()
        {
            var textField = Field("containerLabeledTextInputFieldName");
            textField.Value.should_be("text input field two val");

            Driver.SendKeys(textField, " - send keys without any click, clear first, etc");

            textField.Value.should_be("text input field two val - send keys without any click, clear first, etc");
        }
    }
}