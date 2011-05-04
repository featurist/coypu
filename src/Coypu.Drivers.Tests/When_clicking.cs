using System;
using NSpec;
using NSpec.Domain;

namespace Coypu.Drivers.Tests
{
	public class When_clicking : DriverSpecs
	{
		public Action Specs(Func<Driver> driver, ActionRegister describe, ActionRegister it, Action<Action> setBefore)
		{
			return () =>
			{
				it["should click the underlying element"] = () =>
				{
					var element = driver().FindButton("clickMeTest");
					driver().FindButton("clickMeTest").Text.should_be("Click me");
					driver().Click(element);
					driver().FindButton("clickMeTest").Text.should_be("Click me - clicked");
				};
			};
		}
	}
}