using System;
using NSpec;

namespace Coypu.Drivers.Tests
{
	internal class When_inspecting_content_within_scope : DriverSpecs
	{
		internal override void Specs()
		{
			describe["scope 1"] = () =>
            {
          	    before = () => driver.SetScope(() => driver.FindCss("#scope1"));

				it["should find content within scope"] = () =>
				{
					driver.HasContent("Scope 1").should_be_true();
				};
				it["should not find content outside scope"] = () =>
				{
					driver.HasContent("Scope 2").should_be_false();
				};
			};
			describe["scope 2"] = () =>
			{
				before = () => driver.SetScope(() => driver.FindCss("#scope2"));

				it["should find content within scope"] = () =>
				{
					driver.HasContent("Scope 2").should_be_true();
				};
				it["should not find content outside scope"] = () =>
				{
					driver.HasContent("Scope 1").should_be_false();
				};
			};
		}
	}
}