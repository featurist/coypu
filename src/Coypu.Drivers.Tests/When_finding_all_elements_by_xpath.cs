using System.Linq;
using NSpec;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_all_elements_by_xpath : DriverSpecs
    {       
  [Test]
  public void Returns_empty_if_no_matches()

            {
                const string shouldNotFind = "//*[@id = 'inspectingContent']//p[@class='css-missing-test']";
                Assert.That(Driver.FindAllXPath(shouldNotFind,Root), Is.Empty);
            }

            
    [Test]
    public void Returns_all_matches_by_xpath()
  
            {
                const string shouldFind = "//*[@id='inspectingContent']//ul[@id='cssTest']/li";
                var all = Driver.FindAllXPath(shouldFind,Root);
                all.Count().should_be(3);
                all.ElementAt(1).Text.should_be("two");
                all.ElementAt(2).Text.should_be("Me! Pick me!");
            }
        }
}