using System.Linq;
using Coypu.Finders;
using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_fields_by_for_attribute : DriverSpecs
    {
        [Test]
        public void Finds_text_input()
        {
            Field("text input field linked by for", options: Options.ExactTrue).Id.should_be("forLabeledTextInputFieldId");
        }

        [Test]
        public void Finds_password_field()
        {
            Field("password field linked by for").Id.should_be("forLabeledPasswordFieldId");
        }

        [Test]
        public void Finds_select_field()
        {
            Field("select field linked by for").Id.should_be("forLabeledSelectFieldId");
        }

        [Test]
        public void Finds_checkbox()
        {
            Field("checkbox field linked by for").Id.should_be("forLabeledCheckboxFieldId");
        }

        [Test]
        public void Finds_radio_button()
        {
            Field("radio field linked by for").Id.should_be("forLabeledRadioFieldId");
        }

        [Test]
        public void Finds_textarea()
        {
            Field("textarea field linked by for").Id.should_be("forLabeledTextareaFieldId");
        }

        [Test]
        public void Finds_file_input()
        {
            Field("file field linked by for").Id.should_be("forLabeledFileFieldId");
        }

        [Test]
        public void Finds_by_partial_text()
        {
            var fields = new FieldFinder(Driver, "Some for labeled radio option", Root, DefaultOptions).Find(Options.ExactFalse);
            Assert.That(fields.Select(e => e.Id).OrderBy(id => id), Is.EquivalentTo(new[]
                {
                    "forLabeledRadioFieldExactMatchId",
                    "forLabeledRadioFieldPartialMatchId"
                }));
        }

        [Test]
        public void Finds_by_exact_text()
        {
            var fields = new FieldFinder(Driver, "Some for labeled radio option", Root, DefaultOptions).Find(Options.ExactTrue);
            Assert.That(fields.Select(e => e.Id).OrderBy(id => id), Is.EquivalentTo(new[]
                {
                    "forLabeledRadioFieldExactMatchId"
                }));
        }
    }
}