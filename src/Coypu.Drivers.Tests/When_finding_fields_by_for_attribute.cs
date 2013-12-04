using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_fields_by_for_attribute : DriverSpecs
    {
        [Test]
        public void Finds_text_input()
        {
            Driver.FindField("text input field linked by for", Root).Id.should_be("forLabeledTextInputFieldId");
        }

        [Test]
        public void Finds_password_field()
        {
            Driver.FindField("password field linked by for", Root).Id.should_be("forLabeledPasswordFieldId");
        }

        [Test]
        public void Finds_select_field()
        {
            Driver.FindField("select field linked by for", Root).Id.should_be("forLabeledSelectFieldId");
        }

        [Test]
        public void Finds_checkbox()
        {
            Driver.FindField("checkbox field linked by for", Root).Id.should_be("forLabeledCheckboxFieldId");
        }

        [Test]
        public void Finds_radio_button()
        {
            Driver.FindField("radio field linked by for", Root).Id.should_be("forLabeledRadioFieldId");
        }

        [Test]
        public void Finds_textarea()
        {
            Driver.FindField("textarea field linked by for", Root).Id.should_be("forLabeledTextareaFieldId");
        }

        [Test]
        public void Finds_file_input()
        {
            Driver.FindField("file field linked by for", Root).Id.should_be("forLabeledFileFieldId");
        }
    }
}