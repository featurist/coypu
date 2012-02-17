using Coypu.Finders;
using Coypu.Tests.TestDoubles;
using NSpec;

namespace Coypu.Drivers.Tests
{
    public class When_finding_buttons_within_scope : DriverSpecs
    {
        internal override void Specs()
        {
            DriverScope scope = null;

            describe["within scope1"] = () =>
            {
                before = () => scope = new DriverScope(new IdFinder(driver, "scope1", Root), null);

                it["finds button by name"] = () =>
                {
                    driver.FindButton("scopedButtonName", scope).Id.should_be("scope1ButtonId");
                };
                it["finds input button by value"] = () =>
                {
                    driver.FindButton("scoped input button", scope).Id.should_be("scope1InputButtonId");
                };
                it["finds button by text"] = () =>
                {
                    driver.FindButton("scoped button", scope).Id.should_be("scope1ButtonId");
                };
            };
            describe["within scope2"] = () =>
            {
                before = () => scope = new DriverScope(new IdFinder(driver, "scope2", Root), null);

                it["finds button by name"] = () =>
                {
                    driver.FindButton("scopedButtonName", scope).Id.should_be("scope2ButtonId");
                };
                it["finds input button by value"] = () =>
                {
                    driver.FindButton("scoped input button", scope).Id.should_be("scope2InputButtonId");
                };
                it["finds button by text"] = () =>
                {
                    driver.FindButton("scoped button", scope).Id.should_be("scope2ButtonId");
                };
            };
        }
    }
}