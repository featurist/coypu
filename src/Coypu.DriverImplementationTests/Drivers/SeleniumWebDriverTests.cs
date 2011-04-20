using Coypu.Drivers;
using NUnit.Framework;

namespace Coypu.DriverImplementationTests.Drivers
{
	[TestFixture]
	public class FirefoxSeleniumWebDriverTests : DriverImplementationTests
	{
		protected override Driver GetDriver() { return new SeleniumWebDriver(Browser.Firefox); }
	}

	[TestFixture, Ignore("Need to host on a website to get round security issues I think")]
	public class InternetExplorerSeleniumWebDriverTests : DriverImplementationTests
	{
		protected override Driver GetDriver() { return new SeleniumWebDriver(Browser.InternetExplorer); }
	}

	[TestFixture,Ignore("Need to ensure chrome installed")]
	public class ChromeSeleniumWebDriverTests : DriverImplementationTests
	{
		protected override Driver GetDriver() { return new SeleniumWebDriver(Browser.Chrome); }
	}

}