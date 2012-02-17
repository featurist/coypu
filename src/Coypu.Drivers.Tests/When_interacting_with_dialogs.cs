using System;
using NSpec;

namespace Coypu.Drivers.Tests
{
    internal class When_interacting_with_dialogs : DriverSpecs
    {
        internal override void Specs()
        {
            it["accepts alerts"] = () => 
            {
                using(driver)
                {
                    driver.Click(driver.FindLink("Trigger an alert", Root));
                    driver.HasDialog("You have triggered an alert and this is the text.").should_be_true();
                    driver.AcceptModalDialog();
                    driver.HasDialog("You have triggered an alert and this is the text.").should_be_false();
                };
            };

            describe["confirms"] = () =>
            {
                describe["when accepting"] = () =>
                {
                    it["clears dialog"] = () =>
                    {
                        using(driver)
                        {
                            driver.Click(driver.FindLink("Trigger a confirm", Root));
                            driver.HasDialog("You have triggered a confirm and this is the text.").should_be_true();
                            driver.AcceptModalDialog();
                            driver.HasDialog("You have triggered a confirm and this is the text.").should_be_false();
                        };
                    };
                    it["returns true"] = () =>
                    {
                        using(driver)
                        {
                            driver.Click(driver.FindLink("Trigger a confirm", Root));
                            driver.AcceptModalDialog();
                            driver.FindLink("Trigger a confirm - accepted", Root).should_not_be_null();
                        };
                    };
                };
                describe["when cancelling"] = () =>
                {
                    it["clears dialog"] = () =>
                    {
                        using(driver)
                        {
                            driver.Click(driver.FindLink("Trigger a confirm", Root));
                            driver.HasDialog("You have triggered a confirm and this is the text.").should_be_true();
                            driver.CancelModalDialog();
                            driver.HasDialog("You have triggered a confirm and this is the text.").should_be_false();
                        };
                    };
                    it["returns false"] = () =>
                    {
                        using(driver)
                        {
                            driver.Click(driver.FindLink("Trigger a confirm", Root));
                            driver.CancelModalDialog();
                            driver.FindLink("Trigger a confirm - cancelled", Root).should_not_be_null();
                        };
                    };
                };
            };
        }
    }
}