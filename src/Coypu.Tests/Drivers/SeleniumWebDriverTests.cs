using Coypu.Drivers;
using Coypu.Drivers.Selenium;
using NUnit.Framework;

namespace Coypu.Tests.Drivers
{
	[TestFixture]
	public class FirefoxSeleniumWebDriverTests : DriverImplementationTests
	{
		protected override Driver GetDriver()
		{
			Configuration.Browser = Coypu.Drivers.Browser.Firefox;
			return new SeleniumWebDriver();
		}
	}

	[TestFixture]
	public class InternetExplorerSeleniumWebDriverTests : DriverImplementationTests
	{
		protected override Driver GetDriver()
		{
			Configuration.Browser = Coypu.Drivers.Browser.InternetExplorer;
			return new SeleniumWebDriver();
		}
	}

	[TestFixture]
	public class ChromeSeleniumWebDriverTests : DriverImplementationTests
	{
		protected override Driver GetDriver()
		{
			Configuration.Browser = Coypu.Drivers.Browser.Chrome;
			return new SeleniumWebDriver();
		}
	}
}