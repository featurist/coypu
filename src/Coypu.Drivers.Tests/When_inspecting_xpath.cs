using System;
using Coypu.Drivers.Watin;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
	[NotSupportedBy(typeof(WatiNDriver))]
	internal class When_inspecting_xpath : DriverSpecs
	{
		internal override Action Specs()
		{
			return () =>
			{
				it["should not find missing examples"] = () =>
				{
					const string shouldNotFind = "//*[@id = 'inspectingContent']//p[@class='css-missing-test']";
					Assert.That(driver.HasXPath(shouldNotFind), Is.False, "Expected not to find something at: " + shouldNotFind);
				};

				it["should only finds visible elements"] = () =>
				{
					const string shouldNotFind = "//*[@id = 'inspectingContent']//p[@class='css-test']/img";
					Assert.That(driver.HasXPath(shouldNotFind), Is.False, "Expected not to find something at: " + shouldNotFind);
				};

				it["should find present examples"] = () =>
				{
					var shouldFind = "//*[@id = 'inspectingContent']//p[@class='css-test']/span";
					Assert.That(driver.HasXPath(shouldFind), "Expected to find something at: " + shouldFind);

					shouldFind = "//ul[@id='cssTest']/li[3]";
					Assert.That(driver.HasXPath(shouldFind), "Expected to find something at: " + shouldFind);
				};
			};
		}
	}
}