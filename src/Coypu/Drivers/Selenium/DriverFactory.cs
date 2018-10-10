using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Opera;
using OpenQA.Selenium.Safari;

namespace Coypu.Drivers.Selenium
{
    internal class DriverFactory
    {
        public IWebDriver NewWebDriver(Browser browser)
        {
            if (browser == Browser.Firefox) return new FirefoxDriver();
            if (browser == Browser.Chrome) return new ChromeDriver();
            if (browser == Browser.Edge) return new EdgeDriver();
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