using NSpec;

namespace Coypu.Drivers.Tests
{
    internal class When_choosing : DriverSpecs
    {
        internal override void Specs()
        {
            it["chooses radio button from list"] = () =>
            {
                var radioButton1 = driver.FindField("chooseRadio1", Root);
                radioButton1.Selected.should_be_false();

                // Choose 1
                driver.Choose(radioButton1);

                var radioButton2 = driver.FindField("chooseRadio2", Root);
                radioButton2.Selected.should_be_false();

                // Choose 2
                driver.Choose(radioButton2);

                // New choice is now selected
                radioButton2 = driver.FindField("chooseRadio2", Root);
                radioButton2.Selected.should_be_true();

                // Originally selected is no longer selected
                radioButton1 = driver.FindField("chooseRadio1", Root);
                radioButton1.Selected.should_be_false();
            };

            it["fires onclick event"] = () =>
            {
                var radio = driver.FindField("chooseRadio2", Root);
                radio.Value.should_be("Radio buttons - 2nd value");
    
                driver.Choose(radio);

                driver.FindField("chooseRadio2", Root).Value.should_be("Radio buttons - 2nd value - clicked");
            };
        }
    }
}