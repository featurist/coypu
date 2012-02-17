using Coypu.Finders;
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
                var iframeOne = new DriverScope(new IFrameFinder(driver, "I am iframe one", Root), null);
                var iframeTwo = new DriverScope(new IFrameFinder(driver, "I am iframe two", Root), null);

                driver.FindButton("scoped button", iframeOne).Id.should_be("iframe1ButtonId");
                driver.FindButton("scoped button", iframeTwo).Id.should_be("iframe2ButtonId");
            };

            it["finds clears scope back to the whole window"] = () =>
            {
                var iframeOne = new DriverScope(new IFrameFinder(driver, "I am iframe one", Root), null);
                driver.FindButton("scoped button", iframeOne).Id.should_be("iframe1ButtonId");

                driver.FindButton("scoped button", Root).Id.should_be("scope1ButtonId");
            };

            it["can fill in a text input within an IFrame"] = () =>
            {
                var iframeOne = new DriverScope(new IFrameFinder(driver, "I am iframe one", Root), null);
                driver.Set(driver.FindField("text input in iframe", iframeOne), "filled in");

                Assert.That(driver.FindField("text input in iframe", iframeOne).Value, Is.EqualTo("filled in"));
            };
        }
    }

}