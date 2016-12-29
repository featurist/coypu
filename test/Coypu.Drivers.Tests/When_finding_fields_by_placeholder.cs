using NSpec;
using Xunit;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_fields_by_placeholder : DriverSpecs
    {
        [Fact]
        public void Finds_text_field_by_placeholder()
        {
            Field("text input field with a placeholder").Id.should_be("textInputFieldWithPlaceholder");
            Field("textarea field with a placeholder").Id.should_be("textareaFieldWithPlaceholder");
        }
    }
}