using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_using_iframes_as_scope : DriverSpecs
    {
        internal override void Specs()
        {
            describe["elements withing iframes are not found in outer scope"] = () =>
            {
                it["does not find something in an iframe"] = () =>
                {
                    Assert.Throws<MissingHtmlException>(() => driver.FindButton("iframe1ButtonId", Root));
                };
            };

            it["finds elements among multiple scopes"] = () =>
            {
                driver.SetScope(() => driver.FindIFrame("I am iframe one", Root));
                driver.FindButton("scoped button", Root).Id.should_be("iframe1ButtonId");

                driver.ClearScope();

                driver.SetScope(() => driver.FindIFrame("I am iframe two", Root));
                driver.FindButton("scoped button", Root).Id.should_be("iframe2ButtonId");
            };

            it["finds clears scope back to the whole window"] = () =>
            {
                driver.SetScope(() => driver.FindIFrame("I am iframe one", Root));
                driver.FindButton("scoped button", Root).Id.should_be("iframe1ButtonId");

                driver.ClearScope();

                driver.FindButton("scoped button", Root).Id.should_be("scope1ButtonId");
            };

            it["can fill in a text input within an IFrame"] = () =>
            {
                driver.SetScope(() => driver.FindIFrame("I am iframe one", Root));
                driver.Set(driver.FindField("text input in iframe", Root), "filled in");

                Assert.That(driver.FindField("text input in iframe", Root).Value, Is.EqualTo("filled in"));
            };
        }
    }

}