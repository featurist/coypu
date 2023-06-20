using System;
using Coypu.Drivers;
using Coypu.Drivers.Selenium;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;

namespace Coypu.AcceptanceTests.Examples
{
    internal class CustomBrowserSession
    {
        [TestCase("OS X 10.15", "safari", "13")]
        [TestCase("Windows 10", "edge", "17")]
        [TestCase("Windows 7", "chrome", "89")]
        public void CustomBrowserWithCustomRemoteDriver(string platformName,
                                                        string browserName,
                                                        string browserVersion)
        {
            IDriver driver = new CustomRemoteDriver(Browser.Parse(browserName), ReturnBrowserOptions(platformName, browserName, browserVersion));
            using (var custom = new BrowserSession(driver))
            {
                custom.Visit("https://saucelabs.com/test/guinea-pig");
                Assert.That(custom.ExecuteScript("return 0;"), Is.EqualTo(0));
            }
        }

        [TestCase("chrome")]
        [TestCase("internet explorer")]
        [TestCase("firefox")]
        [TestCase("edge")]
        public void CustomBrowser(string browserName)
        {
            var browser = Browser.Parse(browserName);
            if (browser == Browser.InternetExplorer)
            {
                Assert.Inconclusive("This test requires Internet Explorer and will only run on Windows.");
            }
            var driver = new SeleniumWebDriver(browser);
            using (var custom = new BrowserSession(driver))
            {
                custom.Visit("https://saucelabs.com/test/guinea-pig");
                Assert.That(custom.ExecuteScript("return 0;"), Is.EqualTo(0));
            }
        }

        private static ICapabilities ReturnBrowserOptions(string platformName,
                                                   string browserName,
                                                   string browserVersion)
        {
            DriverOptions browserOptions;
            switch (browserName)
            {
                case "chrome":
                    browserOptions = new ChromeOptions();
                    break;
                case "edge":
                    browserOptions = new EdgeOptions();
                    break;
                case "firefox":
                    browserOptions = new FirefoxOptions();
                    break;
                case "safari":
                    browserOptions = new SafariOptions();
                    break;
                default:
                    throw new Exception($"Browser {browserName} not supported!");
            }

            browserOptions.PlatformName = platformName;
            browserOptions.BrowserVersion = browserVersion;
            return browserOptions.ToCapabilities();
        }

        public class CustomRemoteDriver : SeleniumWebDriver
        {
            public CustomRemoteDriver(Browser browser,
                                      ICapabilities capabilities) : base(CustomWebDriver(capabilities), browser) { }

            private static RemoteWebDriver CustomWebDriver(ICapabilities capabilities)
            {
                return new RemoteWebDriver(new Uri("http://appiumci:af4fbd21-6aee-4a01-857f-c7ffba2f0a50@ondemand.saucelabs.com:80/wd/hub"), capabilities);
            }
        }
    }
}