using Coypu.Finders;
using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_setting_fields : DriverSpecs
    {
        [Test]
        public void Sets_value_of_text_input_field_with_id()
        {
            var textField = Field("containerLabeledTextInputFieldName");  
            Driver.Set(textField, "should be much quicker since it's set by js");

            textField.Value.should_be("should be much quicker since it's set by js");

            var findAgain = Field("containerLabeledTextInputFieldName");
            findAgain.Value.should_be("should be much quicker since it's set by js");
        }

        [Test]
        public void Sets_value_of_text_input_field_with_no_id()
        {
            var textField = Field("Field with no id");
            Driver.Set(textField, "set by sendkeys");

            textField.Value.should_be("set by sendkeys");

            var findAgain = Field("Field with no id");
            findAgain.Value.should_be("set by sendkeys");
        }

        [Test]
        public void Sets_value_of_text_input_field_with_no_type()
        {
            var textField = Field("fieldWithNoType");
            Driver.Set(textField, "set by sendkeys");

            textField.Value.should_be("set by sendkeys");

            var findAgain = Field("fieldWithNoType");
            findAgain.Value.should_be("set by sendkeys");
        }


        [Test]
        public void Sets_value_of_textarea_field()
        {
            var textField = Field("containerLabeledTextareaFieldName");
            Driver.Set(textField, "New textarea value");

            textField.Value.should_be("New textarea value");

            var findAgain = Field("containerLabeledTextareaFieldName");
            findAgain.Value.should_be("New textarea value");
        }


        [Test]
        public void Selects_option_by_text_or_value()
        {
            var textField = Field("containerLabeledSelectFieldId");
            textField.Value.should_be("select2value1");

            Driver.Click(FindSingle(new OptionFinder(Driver, "containerLabeledSelectFieldId", "select two option two",Root,DefaultOptions)));

            var findAgain = Field("containerLabeledSelectFieldId");
            findAgain.Value.should_be("select2value2");

            Driver.Click(FindSingle(new OptionFinder(Driver, "containerLabeledSelectFieldId", "select two option one", Root, DefaultOptions)));

            var andAgain = Field("containerLabeledSelectFieldId");
            andAgain.Value.should_be("select2value1");
        }

        [Test]
        public void Fires_change_event_when_selecting_an_option()
        {
            var textField = Field("containerLabeledSelectFieldId");
            textField.Name.should_be("containerLabeledSelectFieldName");

            Driver.Click(FindSingle(new OptionFinder(Driver, "containerLabeledSelectFieldId",  "select two option two", Root, DefaultOptions)));

            Field("containerLabeledSelectFieldId", Root).Name.should_be("containerLabeledSelectFieldName - changed");
        }
    }
}