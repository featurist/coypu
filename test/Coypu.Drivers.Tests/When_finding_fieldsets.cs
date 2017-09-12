using Shouldly;
using Xunit;

namespace Coypu.Drivers.Tests
{
    public class When_finding_fieldsets : DriverSpecs
    {
        [Fact]
        public void Finds_by_legend_text()
        {
            Fieldset("Scope 1").Id.ShouldBe("fieldsetScope1");
            Fieldset("Scope 2").Id.ShouldBe("fieldsetScope2");
        }

        [Fact]
        public void Finds_by_id()
        {
            Fieldset("fieldsetScope1").Native.ShouldBe(Fieldset("Scope 1").Native);
            Fieldset("fieldsetScope2").Native.ShouldBe(Fieldset("Scope 2").Native);
        }

        [Fact]
        public void Finds_only_fieldsets()
        {
            Assert.Throws<MissingHtmlException>(() => Fieldset("scope1TextInputFieldId"));
            Assert.Throws<MissingHtmlException>(() => Fieldset("sectionOne"));
        }
    }
}