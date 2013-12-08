using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_fields_id : DriverSpecs
    {
        [Test]
        public void Finds_field()
        {
            Field("containerLabeledTextInputFieldId").Value.should_be("text input field two val");
        }

        [Test]
        public void Finds_email_field()
        {
            Field("containerLabeledEmailInputFieldId").Value.should_be("email input field two val");
        }

        [Test]
        public void Finds_tel_field()
        {
            Field("containerLabeledTelInputFieldId").Value.should_be("0123456789");
        }

        [Test]
        public void Finds_url_field()
        {
            Field("containerLabeledUrlInputFieldId").Value.should_be("http://www.example.com");
        }

        [Test]
        public void Finds_textarea()
        {
            Field("containerLabeledTextareaFieldId").Value.should_be("textarea field two val");
        }

        [Test]
        public void Finds_select()
        {
            Field("containerLabeledSelectFieldId").Name.should_be("containerLabeledSelectFieldName");
        }

        [Test]
        public void Finds_checkbox()
        {
            Field("containerLabeledCheckboxFieldId").Value.should_be("checkbox field two val");
        }

        [Test]
        public void Finds_radio()
        {
            Field("containerLabeledRadioFieldId").Value.should_be("radio field two val");
        }

        [Test]
        public void Finds_password()
        {
            Field("containerLabeledPasswordFieldId").Name.should_be("containerLabeledPasswordFieldName");
        }

        [Test]
        public void Finds_file()
        {
            Field("containerLabeledFileFieldId").Name.should_be("containerLabeledFileFieldName");
        }
    }
}