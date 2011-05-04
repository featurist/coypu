using System;
using Coypu.Drivers.Watin;
using NSpec;
using NSpec.Domain;

namespace Coypu.Drivers.Tests
{
    [NotSupportedBy(typeof(WatiNDriver))]
    public class When_finding_buttons_within_scope : DriverSpecs
    {
        public Action Specs(Func<Driver> driver, ActionRegister describe, ActionRegister it, Action<Action> setBefore)
        {
            return () =>
            {
                describe["within scope1"] = () =>
                {
                    setBefore(() => driver().SetScope(() => driver().FindCss("#scope1")));

                    it["should find button by name"] = () =>
                    {
                        driver().FindButton("scopedButtonName").Id.should_be("scope1ButtonId");
                    };
                    it["should find input button by value"] = () =>
                    {
                        driver().FindButton("scoped input button").Id.should_be("scope1InputButtonId");
                    };
                    it["should find button by text"] = () =>
                    {
                        driver().FindButton("scoped button").Id.should_be("scope1ButtonId");
                    };
                };
                describe["within scope2"] = () =>
                {
                    setBefore(() => driver().SetScope(() => driver().FindCss("#scope2")));

                    it["should find button by name"] = () =>
                    {
                        driver().FindButton("scopedButtonName").Id.should_be("scope2ButtonId");
                    };
                    it["should find input button by value"] = () =>
                    {
                        driver().FindButton("scoped input button").Id.should_be("scope2InputButtonId");
                    };
                    it["should find button by text"] = () =>
                    {
                        driver().FindButton("scoped button").Id.should_be("scope2ButtonId");
                    };
                };
            };
        }
    }
}