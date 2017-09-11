using Shouldly;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_fields_by_value : DriverSpecs
    {
        [Test]
        public void Finds_radio_button_by_value() {
            Field("radio field one val").Name.ShouldBe("forLabeledRadioFieldName");
            Field("radio field two val").Name.ShouldBe("containerLabeledRadioFieldName");
        }

        [Test]
        public void Finds_checkbox_by_value() {
            Field("checkbox one val").Name.ShouldBe("checkboxByValueOneFieldName");
            Field("checkbox two val").Name.ShouldBe("checkboxByValueTwoFieldName");
        }

    }
}
