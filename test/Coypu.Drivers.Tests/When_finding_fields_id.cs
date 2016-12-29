using NSpec;
using Xunit;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_fields_id : DriverSpecs
    {
        [Fact]
        public void Finds_field()
        {
            Field("containerLabeledTextInputFieldId").Value.should_be("text input field two val");
        }

        [Fact]
        public void Finds_email_field()
        {
            Field("containerLabeledEmailInputFieldId").Value.should_be("email input field two val");
        }

        [Fact]
        public void Finds_tel_field()
        {
            Field("containerLabeledTelInputFieldId").Value.should_be("0123456789");
        }

        [Fact]
        public void Finds_number_field()
        {
            Field("containerLabeledNumberInputFieldId").Value.should_be("42");
        }

        [Fact]
        public void Finds_datetime_field()
        {
            Field("containerLabeledDatetimeInputFieldId").Value.should_be("2012-01-02T03:04:05Z");
        }

        [Fact]
        public void Finds_datetime_local_field()
        {
            Field("containerLabeledDatetimeLocalInputFieldId").Value.should_be("2012-01-02T03:04:05");
        }

        [Fact]
        public void Finds_date_field()
        {
            Field("containerLabeledDateInputFieldId").Value.should_be("2012-01-02");
        }

        [Fact]
        public void Finds_url_field()
        {
            Field("containerLabeledUrlInputFieldId").Value.should_be("http://www.example.com");
        }
        [Fact]
        public void Finds_color_field()
        {
            Field("containerLabeledColorInputFieldId").Value.should_be("#ff0000");
        }

        [Fact]
        public void Finds_textarea()
        {
            Field("containerLabeledTextareaFieldId").Value.should_be("textarea field two val");
        }

        [Fact]
        public void Finds_select()
        {
            Field("containerLabeledSelectFieldId").Name.should_be("containerLabeledSelectFieldName");
        }

        [Fact]
        public void Finds_checkbox()
        {
            Field("containerLabeledCheckboxFieldId").Value.should_be("checkbox field two val");
        }

        [Fact]
        public void Finds_radio()
        {
            Field("containerLabeledRadioFieldId").Value.should_be("radio field two val");
        }

        [Fact]
        public void Finds_password()
        {
            Field("containerLabeledPasswordFieldId").Name.should_be("containerLabeledPasswordFieldName");
        }

        [Fact]
        public void Finds_file()
        {
            Field("containerLabeledFileFieldId").Name.should_be("containerLabeledFileFieldName");
        }
    }
}