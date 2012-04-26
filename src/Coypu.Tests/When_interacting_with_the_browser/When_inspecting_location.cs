using System;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    [TestFixture]
    public class When_inspecting_location : BrowserInteractionTests
    {
        [Test]
        public void It_returns_the_driver_url()
        {
            var driverLocation = new Uri("https://blank.org:8080/actual_location");
            driver.StubLocation(driverLocation,browserSession);
            Assert.That(browserSession.Location, Is.EqualTo(driverLocation));
        }
    }
}