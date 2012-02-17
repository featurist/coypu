using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_fieldsets : DriverSpecs
    {
        internal override void Specs()
        {
            it["finds by legend text"] = () =>
            {
                driver.FindFieldset("Scope 1", Root).Id.should_be("fieldsetScope1");
                driver.FindFieldset("Scope 2", Root).Id.should_be("fieldsetScope2");
            };
            it["finds by id"] = () =>
            {
                driver.FindFieldset("fieldsetScope1", Root).Native.should_be(driver.FindFieldset("Scope 1", Root).Native);
                driver.FindFieldset("fieldsetScope2", Root).Native.should_be(driver.FindFieldset("Scope 2", Root).Native);
            };
            it["finds only fieldsets"] = () =>
            {
                Assert.Throws<MissingHtmlException>(() => driver.FindFieldset("scope1TextInputFieldId", Root));
                Assert.Throws<MissingHtmlException>(() => driver.FindFieldset("sectionOne", Root));
            };
        }
    }
}