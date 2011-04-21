using Coypu.Drivers;
using NUnit.Framework;

namespace Coypu.DriverImplementationTests.Drivers
{
	[TestFixture]
	public class FirefoxSeleniumWebDriverTests : DriverImplementationTests
	{
		protected override Driver GetDriver() { return new SeleniumWebDriver(Browser.Firefox); }
	}

	[TestFixture]
	public class InternetExplorerSeleniumWebDriverTests : DriverImplementationTests
	{
		protected override Driver GetDriver() { return new SeleniumWebDriver(Browser.InternetExplorer); }
	}

	[TestFixture]
	public class ChromeSeleniumWebDriverTests : DriverImplementationTests
	{
		protected override Driver GetDriver() { return new SeleniumWebDriver(Browser.Chrome); }
	}

}