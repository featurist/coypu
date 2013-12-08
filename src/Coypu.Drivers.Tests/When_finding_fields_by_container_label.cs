using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_fields_by_container_label : DriverSpecs
    {
        [Test]
        public void Finds_text_input()
        {
            Field("text input field in a label container").Id.should_be("containerLabeledTextInputFieldId");
        }

        [Test]
        public void Finds_password()
        {
            Field("password field in a label container").Id.should_be("containerLabeledPasswordFieldId");
        }

        [Test]
        public void Finds_checkbox()
        {
            Field("checkbox field in a label container").Id.should_be("containerLabeledCheckboxFieldId");
        }

        [Test]
        public void Finds_radio()
        {
            Field("radio field in a label container").Id.should_be("containerLabeledRadioFieldId");
        }

        [Test]
        public void Finds_select()
        {
            Field("select field in a label container").Id.should_be("containerLabeledSelectFieldId");
        }

        [Test]
        public void Finds_textarea()
        {
            Field("textarea field in a label container").Id.should_be("containerLabeledTextareaFieldId");
        }

        [Test]
        public void Finds_file_field()
        {
            Field("file field in a label container").Id.should_be("containerLabeledFileFieldId");
        }
    }
}