using NSpec;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_iframes : DriverSpecs
    {
        internal override void Specs()
        {
            it["finds by header text"] = () =>
            {
                driver.FindIFrame("I am iframe one", Root).Id.should_be("iframe1");
                driver.FindIFrame("I am iframe two", Root).Id.should_be("iframe2");
            };

            it["finds by id"] = () =>
            {
                driver.FindIFrame("iframe1", Root).Id.should_be("iframe1");
                driver.FindIFrame("iframe2", Root).Id.should_be("iframe2");
            };

            it["finds by title"] = () =>
            {
                driver.FindIFrame("iframe one title", Root).Id.should_be("iframe1");
                driver.FindIFrame("iframe two title", Root).Id.should_be("iframe2");
            };
        }
    }

}