using System;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_inspecting_xpath : DriverSpecs
    {
        internal override void Specs()
        {
            it["does not find missing examples"] = () =>
            {
                const string shouldNotFind = "//*[@id = 'inspectingContent']//p[@class='css-missing-test']";
                Assert.That(driver.HasXPath(shouldNotFind,Root), Is.False, "Expected not to find something at: " + shouldNotFind);
            };

            it["only finds visible elements"] = () =>
            {
                const string shouldNotFind = "//*[@id = 'inspectingContent']//p[@class='css-test']/img";
                Assert.That(driver.HasXPath(shouldNotFind,Root), Is.False, "Expected not to find something at: " + shouldNotFind);
            };

            it["finds present examples"] = () =>
            {
                var shouldFind = "//*[@id = 'inspectingContent']//p[@class='css-test']/span";
                Assert.That(driver.HasXPath(shouldFind,Root), "Expected to find something at: " + shouldFind);

                shouldFind = "//ul[@id='cssTest']/li[3]";
                Assert.That(driver.HasXPath(shouldFind, Root), "Expected to find something at: " + shouldFind);
            };
        }
    }
}