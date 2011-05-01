using System;
using NSpec.Domain;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
	internal class When_inspecting_css : DriverSpecs
	{
		public Action Specs(Func<Driver> driver, ActionRegister it)
		{
			return () =>
			{
				it["should not find missing examples"] = () => 
				{
					const string shouldNotFind = "#inspectingContent p.css-missing-test";
					Assert.That(driver().HasCss(shouldNotFind), Is.False, "Expected not to find something at: " + shouldNotFind);
				};

				it["should find present examples"] = () => 
				{
					var shouldFind = "#inspectingContent p.css-test span";
					Assert.That(driver().HasCss(shouldFind), "Expected to find something at: " + shouldFind);

					shouldFind = "ul#cssTest li:nth-child(3)";
					Assert.That(driver().HasCss(shouldFind), "Expected to find something at: " + shouldFind);
				};

				it["should only finds visible elements"] = () =>
				{
					const string shouldNotFind = "#inspectingContent p.css-test img.invisible";
					Assert.That(driver().HasCss(shouldNotFind), Is.False, "Expected not to find something at: " + shouldNotFind);
				};
			};
		}
	}
}