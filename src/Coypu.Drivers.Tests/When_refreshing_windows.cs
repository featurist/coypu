using System;
using Coypu.Finders;
using Coypu.Timing;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_refreshing_windows : DriverSpecs
    {
        [Test]
        public void RefreshCausesPageToReload()
        {
            RefreshCausesScopeToReload(Root);
        }

        [Test]
        public void RefreshesCorrectWindowScope()
        {
            Driver.Click(Link("Open pop up window"));
            var popUp = new BrowserWindow(DefaultSessionConfiguration, new WindowFinder(Driver,"Pop Up Window",Root,DefaultOptions), Driver, null, null, null, DisambiguationStrategy);
            RetryUntilTimeoutTimingStrategy.Retry(() => popUp.Now());
            RefreshCausesScopeToReload(popUp);
        }

        private static void RefreshCausesScopeToReload(DriverScope driverScope)
        {
            var tickBeforeRefresh = long.Parse(Driver.ExecuteScript("return window.SpecData.CurrentTick;", driverScope).ToString());

            Driver.Refresh(driverScope);

            var tickAfterRefresh = long.Parse(Driver.ExecuteScript("return window.SpecData.CurrentTick;", driverScope).ToString());

            Assert.That(tickAfterRefresh, Is.GreaterThan(tickBeforeRefresh));
        }
    }
}
