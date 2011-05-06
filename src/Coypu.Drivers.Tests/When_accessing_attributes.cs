using System;
using NSpec;

namespace Coypu.Drivers.Tests
{
	internal class When_accessing_attributes : DriverSpecs
	{
		internal override Action Specs()
		{
			return () =>
			{
				it["should expose element attributes"] = () => 
				{
					var formWithAttributesToTest = driver.FindCss("#attributeTestForm");
					formWithAttributesToTest["id"].should_be("attributeTestForm");
					formWithAttributesToTest["method"].should_be("post");
					formWithAttributesToTest["action"].should_be("http://somesite.com/action.htm");
					formWithAttributesToTest["target"].should_be("_parent");
				};
			};
		}
	}
}