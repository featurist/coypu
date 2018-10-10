using System;
using Coypu.Drivers;
using Coypu.Drivers.Selenium;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Remote;
using OpenQA.Selenium.Safari;

namespace Coypu.AcceptanceTests.Examples
{
    internal class CustomBrowserSession
    {
        [TestCase("OS X 10.11", "safari", "11")]
        [TestCase("Windows 10", "edge", "17")]
        [TestCase("Windows 7", "chrome", "69")]
        public void CustomBrowserWithCustomRemoteDriver(string platformName,
                                                        string browserName,
                                                        string browserVersion)
        {
            DriverOptions options;
            switch (browserName)
            {
                case "edge":
                    options = new EdgeOptions();
                    break;
                case "safari":
                    options = new SafariOptions();
                    break;
                default:
                    options = new ChromeOptions();
                    break;
            }

            options.PlatformName = platformName;
            options.BrowserVersion = browserVersion;

            IDriver driver = new CustomRemoteDriver(Browser.Parse(browserName), options.ToCapabilities());
            using (var custom = new BrowserSession(driver))
            {
                custom.Visit("https://saucelabs.com/test/guinea-pig");
                Assert.That(custom.ExecuteScript("return 0;"), Is.EqualTo(0));
            }
        }

        [TestCase("chrome")]
        [TestCase("internet explorer")]
        [TestCase("edge")]
        public void CustomBrowser(string browserName)
        {
            var driver = new SeleniumWebDriver(Browser.Parse(browserName));
            using (var custom = new BrowserSession(driver))
            {
                custom.Visit("https://saucelabs.com/test/guinea-pig");
                Assert.That(custom.ExecuteScript("return 0;"), Is.EqualTo(0));
            }
        }

        public class CustomRemoteDriver : SeleniumWebDriver
        {
            public CustomRemoteDriver(Browser browser,
                                      ICapabilities capabilities)
                : base(CustomWebDriver(capabilities), browser) { }

            private static RemoteWebDriver CustomWebDriver(ICapabilities capabilities)
            {
                return new RemoteWebDriver(new Uri("http://appiumci:af4fbd21-6aee-4a01-857f-c7ffba2f0a50@ondemand.saucelabs.com:80/wd/hub"), capabilities);
            }
        }
    }
}