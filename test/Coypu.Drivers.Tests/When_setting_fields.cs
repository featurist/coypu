using Coypu.Finders;
using Shouldly;
using Xunit;

namespace Coypu.Drivers.Tests
{
    internal class When_setting_fields : DriverSpecs
    {

        private static DriverScope GetSelectScope(string locator)
        {
            var select = new BrowserWindow(DefaultSessionConfiguration,
                                         new SelectFinder(Driver, locator, Root, DefaultOptions), Driver,
                                         null, null, null, DisambiguationStrategy);
            return @select;
        }

        [Fact]
        public void Sets_value_of_text_input_field_with_id()
        {
            var textField = Field("containerLabeledTextInputFieldName");  
            Driver.Set(textField, "should be much quicker since it's set by js");

            textField.Value.ShouldBe("should be much quicker since it's set by js");

            var findAgain = Field("containerLabeledTextInputFieldName");
            findAgain.Value.ShouldBe("should be much quicker since it's set by js");
        }

        [Fact]
        public void Sets_value_of_text_input_field_with_no_id()
        {
            var textField = Field("Field with no id");
            Driver.Set(textField, "set by sendkeys");

            textField.Value.ShouldBe("set by sendkeys");

            var findAgain = Field("Field with no id");
            findAgain.Value.ShouldBe("set by sendkeys");
        }

        [Fact]
        public void Sets_value_of_number_input_field()
        {
            var numberField = Field("containerLabeledNumberInputFieldId");
            Driver.Set(numberField, "5150");

            numberField.Value.ShouldBe("5150");

            var findAgain = Field("containerLabeledNumberInputFieldId");
            findAgain.Value.ShouldBe("5150");
        }

        [Fact]
        public void Sets_value_of_text_input_field_with_no_type()
        {
            var textField = Field("fieldWithNoType");
            Driver.Set(textField, "set by sendkeys");

            textField.Value.ShouldBe("set by sendkeys");

            var findAgain = Field("fieldWithNoType");
            findAgain.Value.ShouldBe("set by sendkeys");
        }


        [Fact]
        public void Sets_value_of_textarea_field()
        {
            var textField = Field("containerLabeledTextareaFieldName");
            Driver.Set(textField, "New textarea value");

            textField.Value.ShouldBe("New textarea value");

            var findAgain = Field("containerLabeledTextareaFieldName");
            findAgain.Value.ShouldBe("New textarea value");
        }


        [Fact]
        public void Selects_option_by_text_or_value()
        {
            var textField = Field("containerLabeledSelectFieldId");
            textField.Value.ShouldBe("select2value1");

            Driver.Click(FindSingle(new OptionFinder(Driver, "select two option two", GetSelectScope("containerLabeledSelectFieldId"), DefaultOptions)));

            var findAgain = Field("containerLabeledSelectFieldId");
            findAgain.Value.ShouldBe("select2value2");

            Driver.Click(FindSingle(new OptionFinder(Driver, "select two option one", GetSelectScope("containerLabeledSelectFieldId"), DefaultOptions)));

            var andAgain = Field("containerLabeledSelectFieldId");
            andAgain.Value.ShouldBe("select2value1");
        }

        [Fact]
        public void Fires_change_event_when_selecting_an_option()
        {
            var textField = Field("containerLabeledSelectFieldId");
            textField.Name.ShouldBe("containerLabeledSelectFieldName");

            Driver.Click(FindSingle(new OptionFinder(Driver, "select two option two", GetSelectScope("containerLabeledSelectFieldId"), DefaultOptions)));

            Field("containerLabeledSelectFieldId", Root).Name.ShouldBe("containerLabeledSelectFieldName - changed");
        }
    }
}