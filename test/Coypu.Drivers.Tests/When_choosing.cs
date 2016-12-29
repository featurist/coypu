using NSpec;
using Xunit;

namespace Coypu.Drivers.Tests
{
    internal class When_choosing : DriverSpecs
    {
        [Fact]
        public void Chooses_radio_button_from_list()
        {
            var radioButton1 = Field("chooseRadio1");
            radioButton1.Selected.should_be_false();

            // Choose 1
            Driver.Choose(radioButton1);

            var radioButton2 = Field("chooseRadio2");
            radioButton2.Selected.should_be_false();

            // Choose 2
            Driver.Choose(radioButton2);

            // New choice is now selected
            radioButton2 = Field("chooseRadio2");
            radioButton2.Selected.should_be_true();

            // Originally selected is no longer selected
            radioButton1 = Field("chooseRadio1");
            radioButton1.Selected.should_be_false();
        }


        [Fact]
        public void Fires_onclick_event()
        {
            var radio = Field("chooseRadio2");
            radio.Value.should_be("Radio buttons - 2nd value");

            Driver.Choose(radio);

            Field("chooseRadio2", Root).Value.should_be("Radio buttons - 2nd value - clicked");
        }
    }
}