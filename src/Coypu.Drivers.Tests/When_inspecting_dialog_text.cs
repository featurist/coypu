using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_inspecting_dialog_text : DriverSpecs
    {
        [Test]
        public void Finds_exact_text_in_alert()

        {
            using (Driver)
            {
                Driver.Click(Driver.FindLink("Trigger an alert", Root));
                Driver.HasDialog("You have triggered an alert and this is the text.");
                Driver.HasDialog("You have triggered a different alert and this is the different text.").should_be_false();
            }
        }


        [Test]
        public void Finds_exact_text_in_confirm()

        {
            using (Driver)
            {
                Driver.Click(Driver.FindLink("Trigger a confirm", Root));
                Driver.HasDialog("You have triggered a confirm and this is the text.");
                Driver.HasDialog("You have triggered a different confirm and this is the different text.").should_be_false();
            }
        }
    }
}