using NSpec;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_iframes : DriverSpecs
    {
        internal override void Specs()
        {
            it["finds by header text"] = () =>
            {
                driver.FindIFrame("I am iframe one").Id.should_be("iframe1");
                driver.FindIFrame("I am iframe two").Id.should_be("iframe2");
            };

            it["finds by id"] = () =>
            {
                driver.FindIFrame("iframe1").Id.should_be("iframe1");
                driver.FindIFrame("iframe2").Id.should_be("iframe2");
            };

            it["finds by title"] = () =>
            {
                driver.FindIFrame("iframe one title").Id.should_be("iframe1");
                driver.FindIFrame("iframe two title").Id.should_be("iframe2");
            };
        }
    }

}