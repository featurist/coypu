using NSpec;

namespace Coypu.Drivers.Tests
{
	internal class When_accessing_attributes : DriverSpecs
	{
		internal override void Specs()
		{
			it["exposes element attributes"] = () => 
			{
				var formWithAttributesToTest = driver.FindCss("#attributeTestForm");
				formWithAttributesToTest["id"].should_be("attributeTestForm");
				formWithAttributesToTest["method"].should_be("post");
				formWithAttributesToTest["action"].should_be("http://somesite.com/action.htm");
				formWithAttributesToTest["target"].should_be("_parent");
			};
		}
	}
}