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
                    Assert.Throws<MissingHtmlException>(() => driver.FindButton("iframe1ButtonId"));
                };
            };

            describe["within iframe one"] = () =>
            {
                before = () =>
                {
                    driver.SetScope(() => driver.FindIFrame("I am iframe one"));
                };

                it["finds the element in the current scope"] = () =>
                {
                    driver.FindButton("scoped button").Id.should_be("iframe1ButtonId");
                };
            };
            describe["within iframe two"] = () =>
            {
                before = () =>
                {
                    driver.SetScope(() => driver.FindIFrame("I am iframe two"));
                };

                it["finds the element in the current scope"] = () =>
                {
                    driver.FindButton("scoped button").Id.should_be("iframe2ButtonId");
                };
            };
        }
    }

}