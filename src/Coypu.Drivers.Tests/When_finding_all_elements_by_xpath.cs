using System;
using System.Linq;
using Coypu.Drivers.Watin;
using NSpec;
using NSpec.Domain;

namespace Coypu.Drivers.Tests
{
	[NotSupportedBy(typeof(WatiNDriver))]
	internal class When_finding_all_elements_by_xpath : DriverSpecs
	{
		public Action Specs(Func<Driver> driver, ActionRegister describe, ActionRegister it)
		{
			return () =>
			{
				it["should return empty if no matches"] = () =>
				{
					const string shouldNotFind = "//*[@id = 'inspectingContent']//p[@class='css-missing-test']";
					driver().FindAllXPath(shouldNotFind).should_be_empty();
				};

				it["should return all matches by xpath"] = () =>
				{
					const string shouldNotFind = "//*[@id='inspectingContent']//ul[@id='cssTest']/li";
					var all = driver().FindAllXPath(shouldNotFind);
					all.Count().should_be(3);
					all.ElementAt(1).Text.should_be("two");
					all.ElementAt(2).Text.should_be("Me! Pick me!");
				};
			};
		}
	}
}