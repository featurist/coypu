using NSpec;

namespace Coypu.Drivers.Tests
{
    internal class When_inspecting_dialog_text : DriverSpecs
    {
        internal override void Specs()
        {
            it["finds exact text in alert"] = () => 
            {
                using (driver)
                {
                    driver.Click(driver.FindLink("Trigger an alert"));
                    driver.HasDialog("You have triggered an alert and this is the text.");
                    driver.HasDialog("You have triggered a different alert and this is the different text.").should_be_false();
                }
            };

            it["finds exact text in confirm"] = () =>
            {
                using (driver)
                {
                    driver.Click(driver.FindLink("Trigger a confirm"));
                    driver.HasDialog("You have triggered a confirm and this is the text.");
                    driver.HasDialog("You have triggered a different confirm and this is the different text.").should_be_false();
                }
            };
        }
    }
}