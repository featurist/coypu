using System;
using Coypu.Finders;
using NSpec;

namespace Coypu.Drivers.Tests
{
    internal class When_inspecting_content_within_scope : DriverSpecs
    {
        internal override void Specs()
        {
            DriverScope scope = null;
            describe["scope 1"] = () =>
            {
                before = () => scope = new DriverScope(new IdFinder(driver, "scope1", Root), null);

                it["finds content within scope"] = () =>
                {
                    driver.HasContent("Scope 1", scope).should_be_true();
                };
                it["does not find content outside scope"] = () =>
                {
                    driver.HasContent("Scope 2", scope).should_be_false();
                };
            };
            describe["scope 2"] = () =>
            {
                before = () => scope = new DriverScope(new IdFinder(driver, "scope2", Root), null);

                it["finds content within scope"] = () =>
                {
                    driver.HasContent("Scope 2", scope).should_be_true();
                };
                it["does not find content outside scope"] = () =>
                {
                    driver.HasContent("Scope 1", scope).should_be_false();
                };
            };
        }
    }
}