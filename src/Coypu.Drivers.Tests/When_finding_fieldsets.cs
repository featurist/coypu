using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_fieldsets : DriverSpecs
    {
        [Test]
        public void Finds_by_legend_text()
        {
            Fieldset("Scope 1").Id.should_be("fieldsetScope1");
            Fieldset("Scope 2").Id.should_be("fieldsetScope2");
        }

        [Test]
        public void Finds_by_id()
        {
            Fieldset("fieldsetScope1").Native.should_be(Fieldset("Scope 1").Native);
            Fieldset("fieldsetScope2").Native.should_be(Fieldset("Scope 2").Native);
        }

        [Test]
        public void Finds_only_fieldsets()
        {
            Assert.Throws<MissingHtmlException>(() => Fieldset("scope1TextInputFieldId"));
            Assert.Throws<MissingHtmlException>(() => Fieldset("sectionOne"));
        }
    }
}