using Nappybara.Drivers;
using NUnit.Framework;

namespace Nappybara.DriverImplementationTests.Drivers
{
    [TestFixture]
    public class SeleniumDriverTests : RealDriverImplementationTestSuite
    {
        private SeleniumWebDriver driver;

        public override void DisposeBrowser()
        {
            driver.Dispose();
        }

        protected override Driver Driver
        {
            get { return driver ?? (driver = new SeleniumWebDriver()); }
        }
    }
}