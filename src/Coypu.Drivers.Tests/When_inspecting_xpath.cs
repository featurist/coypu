using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_inspecting_xpath : DriverSpecs
    {
        [Test]
        public void Does_not_find_missing_examples()

        {
            const string shouldNotFind = "//*[@id = 'inspectingContent']//p[@class='css-missing-test']";
            Assert.That(Driver.HasXPath(shouldNotFind, Root), Is.False, "Expected not to find something at: " + shouldNotFind);
        }


        [Test]
        public void Only_finds_visible_elements()

        {
            const string shouldNotFind = "//*[@id = 'inspectingContent']//p[@class='css-test']/img";
            Assert.That(Driver.HasXPath(shouldNotFind, Root), Is.False, "Expected not to find something at: " + shouldNotFind);
        }


        [Test]
        public void Finds_present_examples()

        {
            var shouldFind = "//*[@id = 'inspectingContent']//p[@class='css-test']/span";
            Assert.That(Driver.HasXPath(shouldFind, Root), "Expected to find something at: " + shouldFind);

            shouldFind = "//ul[@id='cssTest']/li[3]";
            Assert.That(Driver.HasXPath(shouldFind, Root), "Expected to find something at: " + shouldFind);
        }
    }
}