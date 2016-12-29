using System.Linq;
using Coypu.Finders;
using NSpec;
using Xunit;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_fields_by_container_label : DriverSpecs
    {
        [Fact]
        public void Finds_text_input()
        {
            Field("text input field in a label container", options: Options.Exact).Id.should_be("containerLabeledTextInputFieldId");
        }

        [Fact]
        public void Finds_password()
        {
            Field("password field in a label container").Id.should_be("containerLabeledPasswordFieldId");
        }

        [Fact]
        public void Finds_checkbox()
        {
            Field("checkbox field in a label container").Id.should_be("containerLabeledCheckboxFieldId");
        }

        [Fact]
        public void Finds_radio()
        {
            Field("radio field in a label container").Id.should_be("containerLabeledRadioFieldId");
        }

        [Fact]
        public void Finds_select()
        {
            Field("select field in a label container").Id.should_be("containerLabeledSelectFieldId");
        }

        [Fact]
        public void Finds_textarea()
        {
            Field("textarea field in a label container").Id.should_be("containerLabeledTextareaFieldId");
        }

        [Fact]
        public void Finds_file_field()
        {
            Field("file field in a label container", options: Options.Exact).Id.should_be("containerLabeledFileFieldId");
        }

        [Fact]
        public void Finds_by_substring()
        {
            var fields = new FieldFinder(Driver, "Some container labeled radio option", Root, DefaultOptions).Find(Options.Substring);
            Assert.That(fields.Select(e => e.Id).OrderBy(id => id), Is.EquivalentTo(new[]
                {
                    "containerLabeledRadioFieldExactMatchId",
                    "containerLabeledRadioFieldPartialMatchId"
                }));
        }

        [Fact]
        public void Finds_by_exact_text()
        {
            var fields = new FieldFinder(Driver, "Some container labeled radio option", Root, DefaultOptions).Find(Options.Exact);
            Assert.That(fields.Select(e => e.Id).OrderBy(id => id), Is.EquivalentTo(new[]
                {
                    "containerLabeledRadioFieldExactMatchId"
                }));
        }
    }
}