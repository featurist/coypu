using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_fields_id : DriverSpecs
    {

        [Test]
        public void Finds_field()
        {
            Driver.FindField("containerLabeledTextInputFieldId", Root).Value.should_be("text input field two val");
        }


        [Test]
        public void Finds_email_field()
        {
            Driver.FindField("containerLabeledEmailInputFieldId", Root).Value.should_be("email input field two val");
        }

        [Test]
        public void Finds_tel_field()
        {
            Driver.FindField("containerLabeledTelInputFieldId", Root).Value.should_be("0123456789");
        }

        [Test]
        public void Finds_url_field()
        {
            Driver.FindField("containerLabeledUrlInputFieldId", Root).Value.should_be("http://www.example.com");
        }

        [Test]
        public void Finds_textarea()
        {
            Driver.FindField("containerLabeledTextareaFieldId", Root).Value.should_be("textarea field two val");
        }

        [Test]
        public void Finds_select()
        {
            Driver.FindField("containerLabeledSelectFieldId", Root).Name.should_be("containerLabeledSelectFieldName");
        }

        [Test]
        public void Finds_checkbox()
        {
            Driver.FindField("containerLabeledCheckboxFieldId", Root).Value.should_be("checkbox field two val");
        }

        [Test]
        public void Finds_radio()
        {
            Driver.FindField("containerLabeledRadioFieldId", Root).Value.should_be("radio field two val");
        }

        [Test]
        public void Finds_password()
        {
            Driver.FindField("containerLabeledPasswordFieldId", Root).Name.should_be("containerLabeledPasswordFieldName");
        }

        [Test]
        public void Finds_file()
        {
            Driver.FindField("containerLabeledFileFieldId", Root).Name.should_be("containerLabeledFileFieldName");
        }
    }
}