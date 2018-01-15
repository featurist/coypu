using Coypu.Finders;
using Shouldly;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_fields_by_name : DriverSpecs {

        [Test]
        public void Finds_text_input()
        {
            Field("containerLabeledTextInputFieldName").Value.ShouldBe("text input field two val");
        }

        [Test]
        public void Finds_textarea()
        {
            Field("containerLabeledTextareaFieldName").Value.ShouldBe("textarea field two val");
        }

        [Test]
        public void Finds_select()
        {
            Field("containerLabeledSelectFieldName").Id.ShouldBe("containerLabeledSelectFieldId");
        }

        [Test]
        public void Finds_checkbox()
        {
            Field("containerLabeledCheckboxFieldName").Value.ShouldBe("checkbox field two val");
        }

        [Test]
        public void Does_NOT_find_radio_button()
        {
            Assert.Throws<MissingHtmlException>(() => Button("containerLabeledRadioFieldName"));
        }

        [Test]
        public void Finds_password_input()
        {
            Field("containerLabeledPasswordFieldName").Id.ShouldBe("containerLabeledPasswordFieldId");
        }

        [Test]
        public void Finds_file_input()
        {
            Field("containerLabeledFileFieldName").Id.ShouldBe("containerLabeledFileFieldId");
        }

    }
}