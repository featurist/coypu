using System;
using NSpec;
using NSpec.Domain;

namespace Coypu.Drivers.Tests
{
	internal class When_inspecting_dialog_text : DriverSpecs
	{
		public Action Specs(Func<Driver> driver, ActionRegister it)
		{
			return () =>
			{
				it["should find exact text in alert"] = () => 
				{
					using (driver())
					{
						driver().Click(driver().FindLink("Trigger an alert"));
						driver().HasDialog("You have triggered an alert and this is the text.");
						driver().HasDialog("You have triggered a different alert and this is the different text.").should_be_false();
					}
				};

				it["should find exact text in confirm"] = () =>
				{
					using (driver())
					{
						driver().Click(driver().FindLink("Trigger a confirm"));
						driver().HasDialog("You have triggered a confirm and this is the text.");
						driver().HasDialog("You have triggered a different confirm and this is the different text.").should_be_false();
					}
				};
			};
		}
	}
}