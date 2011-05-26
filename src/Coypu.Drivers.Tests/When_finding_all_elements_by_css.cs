using System.Linq;
using Coypu.Drivers.Watin;
using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    [NotSupportedBy(typeof(WatiNDriver))]
    internal class When_finding_all_elements_by_css : DriverSpecs
    {
        internal override void Specs()
        {
            it["returns empty if no matches"] = () => 
            {
                const string shouldNotFind = "#inspectingContent p.css-missing-test";
                Assert.That(driver.FindAllCss(shouldNotFind), Is.Empty);
            };

            it["returns all matches by css"] = () =>
            {
                const string shouldNotFind = "#inspectingContent ul#cssTest li";
                var all = driver.FindAllCss(shouldNotFind);
                all.Count().should_be(3);
                all.ElementAt(1).Text.should_be("two");
                all.ElementAt(2).Text.should_be("Me! Pick me!");
            };
        }
    }
}