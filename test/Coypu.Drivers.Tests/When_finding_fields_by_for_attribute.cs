using Coypu.Finders;
using Shouldly;
using System.Linq;
using Xunit;

namespace Coypu.Drivers.Tests
{
    public class When_finding_fields_by_for_attribute : DriverSpecs
    {
        [Fact]
        public void Finds_text_input()
        {
            Field("text input field linked by for", options: Options.Exact).Id.ShouldBe("forLabeledTextInputFieldId");
        }

        [Fact]
        public void Finds_password_field()
        {
            Field("password field linked by for").Id.ShouldBe("forLabeledPasswordFieldId");
        }

        [Fact]
        public void Finds_select_field()
        {
            Field("select field linked by for").Id.ShouldBe("forLabeledSelectFieldId");
        }

        [Fact]
        public void Finds_checkbox()
        {
            Field("checkbox field linked by for").Id.ShouldBe("forLabeledCheckboxFieldId");
        }

        [Fact]
        public void Finds_radio_button()
        {
            Field("radio field linked by for").Id.ShouldBe("forLabeledRadioFieldId");
        }

        [Fact]
        public void Finds_textarea()
        {
            Field("textarea field linked by for").Id.ShouldBe("forLabeledTextareaFieldId");
        }

        [Fact]
        public void Finds_file_input()
        {
            Field("file field linked by for").Id.ShouldBe("forLabeledFileFieldId");
        }

        [Fact]
        public void Finds_by_substring_text()
        {
            var fields = new FieldFinder(Driver, "Some for labeled radio option", Root, DefaultOptions).Find(Options.Substring);
            Assert.Equal(new[]
                {
                    "forLabeledRadioFieldExactMatchId",
                    "forLabeledRadioFieldPartialMatchId"
                }, fields.Select(e => e.Id).OrderBy(id => id));
        }

        [Fact]
        public void Finds_by_exact_text()
        {
            var fields = new FieldFinder(Driver, "Some for labeled radio option", Root, DefaultOptions).Find(Options.Exact);
            Assert.Equal(new[]
                {
                    "forLabeledRadioFieldExactMatchId"
                }, fields.Select(e => e.Id).OrderBy(id => id));
        }
    }
}