using Coypu.Drivers.Watin;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    [NotSupportedBy(typeof(WatiNDriver))]
    internal class When_inspecting_css : DriverSpecs
    {
        internal override void Specs()
        {
            it["does not find missing examples"] = () => 
            {
                const string shouldNotFind = "#inspectingContent p.css-missing-test";
                Assert.That(driver.HasCss(shouldNotFind), Is.False, "Expected not to find something at: " + shouldNotFind);
            };

            it["finds present examples"] = () => 
            {
                var shouldFind = "#inspectingContent p.css-test span";
                Assert.That(driver.HasCss(shouldFind), "Expected to find something at: " + shouldFind);

                shouldFind = "ul#cssTest li:nth-child(3)";
                Assert.That(driver.HasCss(shouldFind), "Expected to find something at: " + shouldFind);
            };

            it["only finds visible elements"] = () =>
            {
                const string shouldNotFind = "#inspectingContent p.css-test img.invisible";
                Assert.That(driver.HasCss(shouldNotFind), Is.False, "Expected not to find something at: " + shouldNotFind);
            };
        }
    }
}