using System;
using NSpec;
using NSpec.Domain;

namespace Coypu.Drivers.Tests
{
	public class When_clicking : DriverSpecs
	{
		public Action Specs(Func<Driver> driver, ActionRegister it)
		{
			return () =>
			   {
				   it["should click the underlying node"] = () =>
				 {
					 var node = driver().FindButton("clickMeTest");
					 driver().FindButton("clickMeTest").Text.should_be("Click me");
					driver().Click(node);
					driver().FindButton("clickMeTest").Text.should_be("Click me - clicked");
				 };
			   };
		}
	}
}