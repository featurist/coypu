using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_setting_fields : DriverSpecs
    {
            
  [Test]
  public void Sets_value_of_text_input_field()

            {
                var textField = Driver.FindField("containerLabeledTextInputFieldName", Root);
                Driver.Set(textField, "New text input value");

                textField.Value.should_be("New text input value");

                var findAgain = Driver.FindField("containerLabeledTextInputFieldName", Root);
                findAgain.Value.should_be("New text input value");
            }

            
    [Test]
    public void Sets_value_of_textarea_field()
  
            {
                var textField = Driver.FindField("containerLabeledTextareaFieldName", Root);
                Driver.Set(textField, "New textarea value");

                textField.Value.should_be("New textarea value");

                var findAgain = Driver.FindField("containerLabeledTextareaFieldName", Root);
                findAgain.Value.should_be("New textarea value");
            }

            
    [Test]
    public void Selects_option_by_text_or_value()
  
            {
                var textField = Driver.FindField("containerLabeledSelectFieldId", Root);
                textField.Value.should_be("select2value1");

                Driver.Select(textField, "select two option two");

                var findAgain = Driver.FindField("containerLabeledSelectFieldId", Root);
                findAgain.Value.should_be("select2value2");

                Driver.Select(textField, "select2value1");

                var andAgain = Driver.FindField("containerLabeledSelectFieldId", Root);
                andAgain.Value.should_be("select2value1");
            }
                           
            
    [Test]
    public void Fires_change_event_when_selecting_an_option()
  
            {
                var textField = Driver.FindField("containerLabeledSelectFieldId", Root);
                textField.Name.should_be("containerLabeledSelectFieldName");

                Driver.Select(textField, "select two option two");

                Driver.FindField("containerLabeledSelectFieldId", Root).Name.should_be("containerLabeledSelectFieldName - changed");
            }


        }
    }
