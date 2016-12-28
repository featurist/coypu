using System;
using Xunit;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    public class When_inspecting_location : BrowserInteractionTests
    {
        [Fact]
        public void It_returns_the_driver_url()
        {
            var driverLocation = new Uri("https://blank.org:8080/actual_location");
            driver.StubLocation(driverLocation,browserSession);
            Assert.Equal(driverLocation, browserSession.Location);
        }
    }
}