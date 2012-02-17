using NSpec;

namespace Coypu.Drivers.Tests
{
    internal class When_setting_fields : DriverSpecs
    {
        internal override void Specs()
        {
            it["sets value of text input field"] = () =>
            {
                var textField = driver.FindField("containerLabeledTextInputFieldName", Root);
                driver.Set(textField, "New text input value");

                textField.Value.should_be("New text input value");

                var findAgain = driver.FindField("containerLabeledTextInputFieldName", Root);
                findAgain.Value.should_be("New text input value");
            };

            it["sets value of textarea field"] = () =>
            {
                var textField = driver.FindField("containerLabeledTextareaFieldName", Root);
                driver.Set(textField, "New textarea value");

                textField.Value.should_be("New textarea value");

                var findAgain = driver.FindField("containerLabeledTextareaFieldName", Root);
                findAgain.Value.should_be("New textarea value");
            };

            it["selects option by text or value"] = () =>
            {
                var textField = driver.FindField("containerLabeledSelectFieldId", Root);
                textField.Value.should_be("select2value1");

                driver.Select(textField, "select two option two");

                var findAgain = driver.FindField("containerLabeledSelectFieldId", Root);
                findAgain.Value.should_be("select2value2");

                driver.Select(textField, "select2value1");

                var andAgain = driver.FindField("containerLabeledSelectFieldId", Root);
                andAgain.Value.should_be("select2value1");
            };
                           
            it["fires change event when selecting an option"] = () =>
            {
                var textField = driver.FindField("containerLabeledSelectFieldId", Root);
                textField.Name.should_be("containerLabeledSelectFieldName");

                driver.Select(textField, "select two option two");

                driver.FindField("containerLabeledSelectFieldId", Root).Name.should_be("containerLabeledSelectFieldName - changed");
            };


        }
    }
}