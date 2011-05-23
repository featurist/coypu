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
				driver.FindFieldset("Scope 1").Id.should_be("fieldsetScope1");
				driver.FindFieldset("Scope 2").Id.should_be("fieldsetScope2");
			};
			it["finds by id"] = () =>
			{
				driver.FindFieldset("fieldsetScope1").Native.should_be(driver.FindFieldset("Scope 1").Native);
				driver.FindFieldset("fieldsetScope2").Native.should_be(driver.FindFieldset("Scope 2").Native);
			};
        	it["finds only fieldsets"] = () =>
    		{
    			Assert.Throws<MissingHtmlException>(() => driver.FindFieldset("scope1TextInputFieldId"));
				Assert.Throws<MissingHtmlException>(() => driver.FindFieldset("sectionOne"));
    		};
        }
    }
}