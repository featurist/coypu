using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_fields_by_name : DriverSpecs {

        [Test]
        public void Finds_text_input()
        {
            Driver.FindField("containerLabeledTextInputFieldName", Root).Value.should_be("text input field two val");
        }

        [Test]
        public void Finds_textarea()
        {
            Driver.FindField("containerLabeledTextareaFieldName", Root).Value.should_be("textarea field two val");
        }

        [Test]
        public void Finds_select()
        {
            Driver.FindField("containerLabeledSelectFieldName", Root).Id.should_be("containerLabeledSelectFieldId");
        }

        [Test]
        public void Finds_checkbox()
        {
            Driver.FindField("containerLabeledCheckboxFieldName", Root).Value.should_be("checkbox field two val");
        }

        [Test]
        public void Does_NOT_find_radio_button()
        {
            Assert.Throws<MissingHtmlException>(() => Driver.FindButton("containerLabeledRadioFieldName", Root));
        }

        [Test]
        public void Finds_password_input()
        {
            Driver.FindField("containerLabeledPasswordFieldName", Root).Id.should_be("containerLabeledPasswordFieldId");
        }

        [Test]
        public void Finds_file_input()
        {
            Driver.FindField("containerLabeledFileFieldName", Root).Id.should_be("containerLabeledFileFieldId");
        }

    }
}