using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.DevTools.V85.HeadlessExperimental;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Opera;
using OpenQA.Selenium.Safari;

namespace Coypu.Drivers.Selenium
{
    internal class DriverFactory
    {
        public IWebDriver NewWebDriver(Browser browser, bool headless)
        {
            var firefoxOptions = new FirefoxOptions();
            var chromeOptions = new ChromeOptions();
            EdgeOptions edgeOptions = new EdgeOptions();
            if (headless) {
              firefoxOptions.AddArgument("--headless");
              chromeOptions.AddArgument("--headless=new");
              edgeOptions.AddArgument("headless");
              edgeOptions.AddArgument("disable-gpu");
              if (browser == Browser.Safari) {
                throw new NotSupportedException("Safari does not support headless mode");
              }
              if (browser == Browser.Safari) {
                throw new NotSupportedException("Opera does not support headless mode");
              }
              if (browser == Browser.InternetExplorer) {
                throw new NotSupportedException("Internet Explorer does not support headless mode");
              }
            }
            if (browser == Browser.Firefox) return new FirefoxDriver(firefoxOptions);
            if (browser == Browser.Chrome) return new ChromeDriver(chromeOptions);
            if (browser == Browser.Edge) return new EdgeDriver(edgeOptions);
            if (browser == Browser.Opera) return new OperaDriver();
            if (browser == Browser.Safari) return new SafariDriver();
            return browser == Browser.InternetExplorer
                       ? new InternetExplorerDriver(new InternetExplorerOptions
                                                    {
                                                        IntroduceInstabilityByIgnoringProtectedModeSettings = true,
                                                        EnableNativeEvents = true,
                                                        IgnoreZoomLevel = true
                                                    })
                       : BrowserNotSupported(browser, null);
        }

        private IWebDriver BrowserNotSupported(Browser browser,
                                               Exception inner)
        {
            throw new BrowserNotSupportedException(browser, GetType(), inner);
        }
    }
}
