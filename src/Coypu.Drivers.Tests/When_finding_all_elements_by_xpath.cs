using System.Linq;
using Coypu.Finders;
using Shouldly;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_all_elements_by_xpath : DriverSpecs
    {
        [Test]
        public void Returns_empty_if_no_matches()
        {
            const string shouldNotFind = "//*[@id = 'inspectingContent']//p[@class='css-missing-test']";
            Assert.That(Driver.FindAllXPath(shouldNotFind, Root, DefaultOptions), Is.Empty);
        }

        [Test]
        public void Returns_all_matches_by_xpath()
        {
            const string shouldFind = "//*[@id='inspectingContent']//ul[@id='cssTest']/li";
            var all = Driver.FindAllXPath(shouldFind, Root, DefaultOptions);
            all.Count().ShouldBe(3);
            all.ElementAt(1).Text.ShouldBe("two");
            all.ElementAt(2).Text.ShouldBe("Me! Pick me!");
        }
    }
}