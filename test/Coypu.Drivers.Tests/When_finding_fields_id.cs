using Shouldly;
using Xunit;

namespace Coypu.Drivers.Tests
{
    public class When_finding_fields_id : DriverSpecs
    {
        [Fact]
        public void Finds_field()
        {
            Field("containerLabeledTextInputFieldId").Value.ShouldBe("text input field two val");
        }

        [Fact]
        public void Finds_email_field()
        {
            Field("containerLabeledEmailInputFieldId").Value.ShouldBe("email input field two val");
        }

        [Fact]
        public void Finds_tel_field()
        {
            Field("containerLabeledTelInputFieldId").Value.ShouldBe("0123456789");
        }

        [Fact]
        public void Finds_number_field()
        {
            Field("containerLabeledNumberInputFieldId").Value.ShouldBe("42");
        }

        [Fact]
        public void Finds_datetime_field()
        {
            Field("containerLabeledDatetimeInputFieldId").Value.ShouldBe("2012-01-02T03:04:05Z");
        }

        [Fact]
        public void Finds_datetime_local_field()
        {
            Field("containerLabeledDatetimeLocalInputFieldId").Value.ShouldBe("2012-01-02T03:04:05");
        }

        [Fact]
        public void Finds_date_field()
        {
            Field("containerLabeledDateInputFieldId").Value.ShouldBe("2012-01-02");
        }

        [Fact]
        public void Finds_url_field()
        {
            Field("containerLabeledUrlInputFieldId").Value.ShouldBe("http://www.example.com");
        }
        [Fact]
        public void Finds_color_field()
        {
            Field("containerLabeledColorInputFieldId").Value.ShouldBe("#ff0000");
        }

        [Fact]
        public void Finds_textarea()
        {
            Field("containerLabeledTextareaFieldId").Value.ShouldBe("textarea field two val");
        }

        [Fact]
        public void Finds_select()
        {
            Field("containerLabeledSelectFieldId").Name.ShouldBe("containerLabeledSelectFieldName");
        }

        [Fact]
        public void Finds_checkbox()
        {
            Field("containerLabeledCheckboxFieldId").Value.ShouldBe("checkbox field two val");
        }

        [Fact]
        public void Finds_radio()
        {
            Field("containerLabeledRadioFieldId").Value.ShouldBe("radio field two val");
        }

        [Fact]
        public void Finds_password()
        {
            Field("containerLabeledPasswordFieldId").Name.ShouldBe("containerLabeledPasswordFieldName");
        }

        [Fact]
        public void Finds_file()
        {
            Field("containerLabeledFileFieldId").Name.ShouldBe("containerLabeledFileFieldName");
        }
    }
}