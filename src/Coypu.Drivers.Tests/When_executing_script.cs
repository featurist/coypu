using Coypu.Drivers.Watin;
using NSpec;

namespace Coypu.Drivers.Tests
{
    [NotSupportedBy(typeof(WatiNDriver))]
	internal class When_executing_script : DriverSpecs
	{
		internal override void Specs()
		{
            it["should run the script in the browser"] = () =>
            {
                driver.FindButton("firstButtonId").Text.should_be("first button");

                driver.ExecuteScript("document.getElementById('firstButtonId').innerHTML = 'script executed';");

                driver.FindButton("firstButtonId").Text.should_be("script executed");
            };

            it["should return the result"] = () =>
            {
                driver.ExecuteScript("return document.getElementById('firstButtonId').innerHTML;").should_be("first button");
            };
		}
	}
}