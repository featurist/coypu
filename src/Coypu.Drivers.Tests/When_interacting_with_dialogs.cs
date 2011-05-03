using System;
using Coypu.Drivers.Selenium;
using Coypu.Drivers.Watin;
using NSpec;
using NSpec.Domain;

namespace Coypu.Drivers.Tests
{
	[NotSupportedBy(typeof(WatiNDriver), typeof(SeleniumWebDriver))]
	internal class When_interacting_with_dialogs : DriverSpecs
	{
		public Action Specs(Func<Driver> driver, ActionRegister describe, ActionRegister it)
		{
			return () =>
		       {
				it["should accept alerts"] = () => 
				{
					DisposeOnError(driver(), () =>
					{
						driver().Click(driver().FindLink("Trigger an alert"));
						driver().HasDialog("You have triggered an alert and this is the text.").should_be_true();
						driver().AcceptModalDialog();
						driver().HasDialog("You have triggered an alert and this is the text.").should_be_false();
					});
				};

				describe["confirms"] = () =>
				{
					describe["when accepting"] = () =>
					{
						it["should clear dialog"] = () =>
						{
							DisposeOnError(driver(), () => 
							{
								driver().Click(driver().FindLink("Trigger a confirm"));
								driver().HasDialog("You have triggered a confirm and this is the text.").should_be_true();
								driver().AcceptModalDialog();
								driver().HasDialog("You have triggered a confirm and this is the text.").should_be_false();
							});
						};
						it["should return true"] = () =>
						{
							DisposeOnError(driver(), () => 
							{
								driver().Click(driver().FindLink("Trigger a confirm"));
								driver().AcceptModalDialog();
								driver().FindLink("Trigger a confirm - accepted").should_not_be_null();
							});
						};
					};
					describe["when cancelling"] = () =>
					{
						it["should clear dialog"] = () =>
						{
							DisposeOnError(driver(), () =>
							{
								driver().Click(driver().FindLink("Trigger a confirm"));
								driver().HasDialog("You have triggered a confirm and this is the text.").should_be_true();
								driver().CancelModalDialog();
								driver().HasDialog("You have triggered a confirm and this is the text.").should_be_false();
							});
						};
						it["should return false"] = () =>
						{
							DisposeOnError(driver(), () =>
							{
								driver().Click(driver().FindLink("Trigger a confirm"));
								driver().CancelModalDialog();
								driver().FindLink("Trigger a confirm - cancelled").should_not_be_null();
							});
						};
					};
				};
			};
		}

		void DisposeOnError(IDisposable driver, Action action)
		{
			try
			{
				action();
			}
			catch (Exception)
			{
				driver.Dispose();
				throw;
			}
		}
	}
}