using System;
using Coypu.Finders;
using Xunit;

namespace Coypu.Drivers.Tests
{
    internal class When_refreshing_windows : DriverSpecs
    {
        [Fact]
        public void RefreshCausesPageToReload()
        {
            RefreshCausesScopeToReload(Root);
        }

        [Fact]
        public void RefreshesCorrectWindowScope()
        {
            Driver.Click(Link("Open pop up window"));
            var popUp = new BrowserWindow(DefaultSessionConfiguration, new WindowFinder(Driver,"Pop Up Window",Root,DefaultOptions), Driver, null, null, null, DisambiguationStrategy);

            try
            {
                RefreshCausesScopeToReload(popUp);
            }
            finally
            {
                Driver.ExecuteScript("return self.close();", popUp);
            }
        }

        private static void RefreshCausesScopeToReload(DriverScope driverScope)
        {
            var tickBeforeRefresh = (Int64) Driver.ExecuteScript("return window.SpecData.CurrentTick;", driverScope);

            Driver.Refresh(driverScope);

            var tickAfterRefresh = (Int64) Driver.ExecuteScript("return window.SpecData.CurrentTick;", driverScope);

            Assert.That(tickAfterRefresh, Is.GreaterThan(tickBeforeRefresh));
        }
    }
}