using System;
using Coypu.Drivers.Selenium;
using Coypu.Drivers.Watin;
using NSpec;

namespace Coypu.Drivers.Tests
{
    [NotSupportedBy(typeof(WatiNDriver), typeof(SeleniumWebDriver))] // Doesnt work with webdriver + FF 4
    internal class When_interacting_with_dialogs : DriverSpecs
    {
        internal override void Specs()
        {
            it["accepts alerts"] = () => 
            {
                DisposeOnError(() =>
                {
                    driver.Click(driver.FindLink("Trigger an alert"));
                    driver.HasDialog("You have triggered an alert and this is the text.").should_be_true();
                    driver.AcceptModalDialog();
                    driver.HasDialog("You have triggered an alert and this is the text.").should_be_false();
                });
            };

            describe["confirms"] = () =>
            {
                describe["when accepting"] = () =>
                {
                    it["clears dialog"] = () =>
                    {
                        DisposeOnError(() => 
                        {
                            driver.Click(driver.FindLink("Trigger a confirm"));
                            driver.HasDialog("You have triggered a confirm and this is the text.").should_be_true();
                            driver.AcceptModalDialog();
                            driver.HasDialog("You have triggered a confirm and this is the text.").should_be_false();
                        });
                    };
                    it["returns true"] = () =>
                    {
                        DisposeOnError(() => 
                        {
                            driver.Click(driver.FindLink("Trigger a confirm"));
                            driver.AcceptModalDialog();
                            driver.FindLink("Trigger a confirm - accepted").should_not_be_null();
                        });
                    };
                };
                describe["when cancelling"] = () =>
                {
                    it["clears dialog"] = () =>
                    {
                        DisposeOnError(() =>
                        {
                            driver.Click(driver.FindLink("Trigger a confirm"));
                            driver.HasDialog("You have triggered a confirm and this is the text.").should_be_true();
                            driver.CancelModalDialog();
                            driver.HasDialog("You have triggered a confirm and this is the text.").should_be_false();
                        });
                    };
                    it["returns false"] = () =>
                    {
                        DisposeOnError(() =>
                        {
                            driver.Click(driver.FindLink("Trigger a confirm"));
                            driver.CancelModalDialog();
                            driver.FindLink("Trigger a confirm - cancelled").should_not_be_null();
                        });
                    };
                };
            };
        }

        void DisposeOnError(Action action)
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