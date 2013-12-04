using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_fields_by_container_label : DriverSpecs
    {
        [Test]
        public void Finds_text_input()
        {
            Driver.FindField("text input field in a label container", Root).Id.should_be("containerLabeledTextInputFieldId");
        }

        [Test]
        public void Finds_password()
        {
            Driver.FindField("password field in a label container", Root).Id.should_be("containerLabeledPasswordFieldId");
        }

        [Test]
        public void Finds_checkbox()
        {
            Driver.FindField("checkbox field in a label container", Root).Id.should_be("containerLabeledCheckboxFieldId");
        }

        [Test]
        public void Finds_radio()
        {
            Driver.FindField("radio field in a label container", Root).Id.should_be("containerLabeledRadioFieldId");
        }

        [Test]
        public void Finds_select()
        {
            Driver.FindField("select field in a label container", Root).Id.should_be("containerLabeledSelectFieldId");
        }

        [Test]
        public void Finds_textarea()
        {
            Driver.FindField("textarea field in a label container", Root).Id.should_be("containerLabeledTextareaFieldId");
        }

        [Test]
        public void Finds_file_field()
        {
            Driver.FindField("file field in a label container", Root).Id.should_be("containerLabeledFileFieldId");
        }
    }
}