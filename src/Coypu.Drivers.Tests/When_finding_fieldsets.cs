using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_fieldsets : DriverSpecs
    {
        [Test]
        public void Finds_by_legend_text()
        {
            Driver.FindFieldset("Scope 1", Root).Id.should_be("fieldsetScope1");
            Driver.FindFieldset("Scope 2", Root).Id.should_be("fieldsetScope2");
        }

        [Test]
        public void Finds_by_id()
        {
            Driver.FindFieldset("fieldsetScope1", Root).Native.should_be(Driver.FindFieldset("Scope 1", Root).Native);
            Driver.FindFieldset("fieldsetScope2", Root).Native.should_be(Driver.FindFieldset("Scope 2", Root).Native);
        }

        [Test]
        public void Finds_only_fieldsets()
        {
            Assert.Throws<MissingHtmlException>(() => Driver.FindFieldset("scope1TextInputFieldId", Root));
            Assert.Throws<MissingHtmlException>(() => Driver.FindFieldset("sectionOne", Root));
        }
    }
}