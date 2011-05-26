using NSpec;

namespace Coypu.Drivers.Tests
{
    public class When_finding_buttons_within_scope : DriverSpecs
    {
        internal override void Specs()
        {
            describe["within scope1"] = () =>
            {
                before = () => driver.SetScope(() => driver.FindId("scope1"));

                it["finds button by name"] = () =>
                {
                    driver.FindButton("scopedButtonName").Id.should_be("scope1ButtonId");
                };
                it["finds input button by value"] = () =>
                {
                    driver.FindButton("scoped input button").Id.should_be("scope1InputButtonId");
                };
                it["finds button by text"] = () =>
                {
                    driver.FindButton("scoped button").Id.should_be("scope1ButtonId");
                };
            };
            describe["within scope2"] = () =>
            {
                before = () => driver.SetScope(() => driver.FindId("scope2"));

                it["finds button by name"] = () =>
                {
                    driver.FindButton("scopedButtonName").Id.should_be("scope2ButtonId");
                };
                it["finds input button by value"] = () =>
                {
                    driver.FindButton("scoped input button").Id.should_be("scope2InputButtonId");
                };
                it["finds button by text"] = () =>
                {
                    driver.FindButton("scoped button").Id.should_be("scope2ButtonId");
                };
            };
        }
    }
}