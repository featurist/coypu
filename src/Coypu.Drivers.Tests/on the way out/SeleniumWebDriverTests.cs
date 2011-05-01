//using Coypu.Drivers.Selenium;
//using NUnit.Framework;
//
//namespace Coypu.Drivers.Tests
//{
//	[TestFixture]
//	public class FirefoxSeleniumWebDriverTests : DriverImplementationTests
//	{
//		protected override Driver GetDriver()
//		{
//			Configuration.Browser = Browser.Firefox;
//			return new SeleniumWebDriver();
//		}
//	}
//
//	[TestFixture]
//	public class InternetExplorerSeleniumWebDriverTests : DriverImplementationTests
//	{
//		protected override Driver GetDriver()
//		{
//			Configuration.Browser = Browser.InternetExplorer;
//			return new SeleniumWebDriver();
//		}
//	}
//
//	[TestFixture]
//	public class ChromeSeleniumWebDriverTests : DriverImplementationTests
//	{
//		protected override Driver GetDriver()
//		{
//			Configuration.Browser = Browser.Chrome;
//			return new SeleniumWebDriver();
//		}
//	}
//}