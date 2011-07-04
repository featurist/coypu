using System.Linq;
using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_all_elements_by_xpath : DriverSpecs
    {
        internal override void Specs()
        {
            it["returns empty if no matches"] = () =>
            {
                const string shouldNotFind = "//*[@id = 'inspectingContent']//p[@class='css-missing-test']";
                Assert.That(driver.FindAllXPath(shouldNotFind), Is.Empty);
            };

            it["returns all matches by xpath"] = () =>
            {
                const string shouldFind = "//*[@id='inspectingContent']//ul[@id='cssTest']/li";
                var all = driver.FindAllXPath(shouldFind);
                all.Count().should_be(3);
                all.ElementAt(1).Text.should_be("two");
                all.ElementAt(2).Text.should_be("Me! Pick me!");
            };
        }
    }
}