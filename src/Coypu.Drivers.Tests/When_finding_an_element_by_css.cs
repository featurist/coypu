using Coypu.Drivers.Watin;
using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
	[NotSupportedBy(typeof(WatiNDriver))]
	internal class When_finding_an_element_by_css : DriverSpecs
	{
		internal override void Specs()
		{
			it["finds present examples"] = () => 
			{
				var shouldFind = "#inspectingContent p.css-test span";
				driver.FindCss(shouldFind).Text.should_be("This");

				shouldFind = "ul#cssTest li:nth-child(3)";
				driver.FindCss(shouldFind).Text.should_be("Me! Pick me!");
			};

			it["does not find missing examples"] = () =>
			{
				const string shouldNotFind = "#inspectingContent p.css-missing-test";
				Assert.Throws<MissingHtmlException>(() => driver.FindCss(shouldNotFind), "Expected not to find something at: " + shouldNotFind);
			};

			it["only finds visible elements"] = () =>
			{
				const string shouldNotFind = "#inspectingContent p.css-test img.invisible";
				Assert.Throws<MissingHtmlException>(() => driver.FindCss(shouldNotFind), "Expected not to find something at: " + shouldNotFind);
			};
		}
	}
}