using System;
using NSpec;
using NSpec.Domain;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
	public class When_finding_links : DriverSpecs
	{
		public Action Specs(Func<Driver> driver, ActionRegister it)
		{
			return () =>
			{
				it["should find link by text"] = () =>
				{
					driver().FindLink("first link").Id.should_be("firstLinkId");
					driver().FindLink("second link").Id.should_be("secondLinkId");
				};

				it["should not find display:none"] = () =>
				 {
					 Assert.Throws<MissingHtmlException>(() => driver().FindLink("I am an invisible link by display"));
				 };

				it["should not find visibility:hidden links"] =	() =>
				{
					Assert.Throws<MissingHtmlException>(() => driver().FindLink("I am an invisible link by visibility"));
				};
			};
		}
	}
}