using System;
using NSpec;
using NSpec.Domain;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
	internal class When_finding_an_element_by_css : DriverSpecs
	{
		public Action Specs(Func<Driver> driver, ActionRegister describe, ActionRegister it)
		{
			return () =>
			{
				it["should find present examples"] = () => 
				{
					var shouldFind = "#inspectingContent p.css-test span";
					driver().FindCss(shouldFind).Text.should_be("This");

					shouldFind = "ul#cssTest li:nth-child(3)";
					driver().FindCss(shouldFind).Text.should_be("Me! Pick me!");
				};

				it["should not find missing examples"] = () =>
				{
					const string shouldNotFind = "#inspectingContent p.css-missing-test";
					Assert.Throws<MissingHtmlException>(() => driver().FindCss(shouldNotFind), "Expected not to find something at: " + shouldNotFind);
				};

				it["should only find visible elements"] = () =>
				{
					const string shouldNotFind = "#inspectingContent p.css-test img.invisible";
					Assert.Throws<MissingHtmlException>(() => driver().FindCss(shouldNotFind), "Expected not to find something at: " + shouldNotFind);
				};
			};
		}
	}
}