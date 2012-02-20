using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_choosing : DriverSpecs
    {
        [Test]
        public void Chooses_radio_button_from_list()
        {
            var radioButton1 = Driver.FindField("chooseRadio1", Root);
            radioButton1.Selected.should_be_false();

            // Choose 1
            Driver.Choose(radioButton1);

            var radioButton2 = Driver.FindField("chooseRadio2", Root);
            radioButton2.Selected.should_be_false();

            // Choose 2
            Driver.Choose(radioButton2);

            // New choice is now selected
            radioButton2 = Driver.FindField("chooseRadio2", Root);
            radioButton2.Selected.should_be_true();

            // Originally selected is no longer selected
            radioButton1 = Driver.FindField("chooseRadio1", Root);
            radioButton1.Selected.should_be_false();
        }


        [Test]
        public void Fires_onclick_event()
        {
            var radio = Driver.FindField("chooseRadio2", Root);
            radio.Value.should_be("Radio buttons - 2nd value");

            Driver.Choose(radio);

            Driver.FindField("chooseRadio2", Root).Value.should_be("Radio buttons - 2nd value - clicked");
        }
    }
}