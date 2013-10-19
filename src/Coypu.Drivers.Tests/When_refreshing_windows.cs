using System;
using Coypu.Finders;
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
            Driver.Click(Driver.FindLink("Open pop up window", Root));
            var popUp = new DriverScope(new SessionConfiguration(), new WindowFinder(Driver, "Pop Up Window", Root), Driver, null, null, null);

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
            var tickBeforeRefresh = Int64.Parse(Driver.ExecuteScript("return window.SpecData.CurrentTick;", driverScope));

            Driver.Refresh(driverScope);

            var tickAfterRefresh = Int64.Parse(Driver.ExecuteScript("return window.SpecData.CurrentTick;", driverScope));

            Assert.That((tickAfterRefresh - tickBeforeRefresh), Is.GreaterThan(0));
        }
    }
}