using System;
using NSpec;
using NSpec.Domain;

namespace Coypu.Drivers.Tests
{
	internal class When_accessing_attributes : DriverSpecs
	{
		public Action Specs(Func<Driver> driver, ActionRegister describe, ActionRegister it)
		{
			return () =>
			{
				it["should expose element attributes"] = () => 
				{
					var formWithAttributesToTest = driver().FindCss("#attributeTestForm");
					formWithAttributesToTest["id"].should_be("attributeTestForm");
					formWithAttributesToTest["method"].should_be("post");
					formWithAttributesToTest["action"].should_be("http://somesite.com/action.htm");
					formWithAttributesToTest["target"].should_be("_parent");
				};
			};
		}
	}
}