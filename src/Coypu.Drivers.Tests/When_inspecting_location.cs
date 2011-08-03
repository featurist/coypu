using System;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_inspecting_location : DriverSpecs
    {
        internal override void Specs()
        {
            it["gets the current browser location"] = () =>
            {
                driver.Visit("http://localhost:4567");
                Assert.That(driver.Location, Is.EqualTo(new Uri("http://localhost:4567/")));

                driver.Visit("http://localhost:4567/auto_login");
                Assert.That(driver.Location, Is.EqualTo(new Uri("http://localhost:4567/auto_login")));
            };

            it["not just when set by visit"] = () =>
            {
                driver.Visit("http://localhost:4567/auto_login");
                driver.ExecuteScript("document.location.href = 'http://localhost:4567/resource/js_redirect'");

                Assert.That(driver.Location, Is.EqualTo(new Uri("http://localhost:4567/resource/js_redirect")));
            };
        }
    }
}