using System.Linq;
using Coypu.Finders;
using NSpec;
using Xunit;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_fields_by_for_attribute : DriverSpecs
    {
        [Fact]
        public void Finds_text_input()
        {
            Field("text input field linked by for", options: Options.Exact).Id.should_be("forLabeledTextInputFieldId");
        }

        [Fact]
        public void Finds_password_field()
        {
            Field("password field linked by for").Id.should_be("forLabeledPasswordFieldId");
        }

        [Fact]
        public void Finds_select_field()
        {
            Field("select field linked by for").Id.should_be("forLabeledSelectFieldId");
        }

        [Fact]
        public void Finds_checkbox()
        {
            Field("checkbox field linked by for").Id.should_be("forLabeledCheckboxFieldId");
        }

        [Fact]
        public void Finds_radio_button()
        {
            Field("radio field linked by for").Id.should_be("forLabeledRadioFieldId");
        }

        [Fact]
        public void Finds_textarea()
        {
            Field("textarea field linked by for").Id.should_be("forLabeledTextareaFieldId");
        }

        [Fact]
        public void Finds_file_input()
        {
            Field("file field linked by for").Id.should_be("forLabeledFileFieldId");
        }

        [Fact]
        public void Finds_by_substring_text()
        {
            var fields = new FieldFinder(Driver, "Some for labeled radio option", Root, DefaultOptions).Find(Options.Substring);
            Assert.That(fields.Select(e => e.Id).OrderBy(id => id), Is.EquivalentTo(new[]
                {
                    "forLabeledRadioFieldExactMatchId",
                    "forLabeledRadioFieldPartialMatchId"
                }));
        }

        [Fact]
        public void Finds_by_exact_text()
        {
            var fields = new FieldFinder(Driver, "Some for labeled radio option", Root, DefaultOptions).Find(Options.Exact);
            Assert.That(fields.Select(e => e.Id).OrderBy(id => id), Is.EquivalentTo(new[]
                {
                    "forLabeledRadioFieldExactMatchId"
                }));
        }
    }
}