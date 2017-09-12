using Shouldly;
using Xunit;

namespace Coypu.Drivers.Tests
{
    public class When_finding_fields_by_placeholder : DriverSpecs
    {
        [Fact]
        public void Finds_text_field_by_placeholder()
        {
            Field("text input field with a placeholder").Id.ShouldBe("textInputFieldWithPlaceholder");
            Field("textarea field with a placeholder").Id.ShouldBe("textareaFieldWithPlaceholder");
        }
    }
}