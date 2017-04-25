using System;
using System.IO;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Remote;

namespace Coypu.Drivers.Selenium
{
    internal class DriverFactory
    {
        public IWebDriver NewWebDriver(Browser browser)  {
            if (browser == Browser.Firefox)
                return new FirefoxDriver(FirefoxDriverService.CreateDefaultService(Directory.GetCurrentDirectory()));
            if (browser == Browser.InternetExplorer)
            {
                var options = new InternetExplorerOptions
                {
                    IntroduceInstabilityByIgnoringProtectedModeSettings = true,
                    EnableNativeEvents = true,
                    IgnoreZoomLevel = true
                };
                return new InternetExplorerDriver(InternetExplorerDriverService.CreateDefaultService(Directory.GetCurrentDirectory()), options);
            }
            if (browser == Browser.Chrome)
                return new ChromeDriver(ChromeDriverService.CreateDefaultService(Directory.GetCurrentDirectory()));
            if (browser == Browser.HtmlUnit)
                return new RemoteWebDriver(DesiredCapabilities.HtmlUnit());
            if (browser == Browser.HtmlUnitWithJavaScript) {
                DesiredCapabilities desiredCapabilities = DesiredCapabilities.HtmlUnit();
                desiredCapabilities.IsJavaScriptEnabled = true;
                return new RemoteWebDriver(desiredCapabilities);
            }
            if (browser == Browser.PhantomJS)
                return new PhantomJSDriver();
            return browserNotSupported(browser,null);
        }

        private IWebDriver browserNotSupported(Browser browser, Exception inner) {
            throw new BrowserNotSupportedException(browser, GetType(), inner);
        }
    }
}