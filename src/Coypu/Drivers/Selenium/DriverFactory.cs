using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Android;
using OpenQA.Selenium.Remote;

namespace Coypu.Drivers.Selenium
{
    internal class DriverFactory
    {
        public RemoteWebDriver NewRemoteWebDriver(Browser browser)
        {
            switch (browser)
            {
                case (Browser.Firefox):
                    return new FirefoxDriver();
                case (Browser.InternetExplorer):
                    {
                        return new InternetExplorerDriver(new InternetExplorerOptions{IntroduceInstabilityByIgnoringProtectedModeSettings = true});
                    }
                case (Browser.Chrome):
                    return new ChromeDriver();
                case (Browser.Android):
                    return new AndroidDriver();
                case (Browser.HtmlUnit):
                    return new RemoteWebDriver(DesiredCapabilities.HtmlUnit());
                case (Browser.HtmlUnitWithJavaScript):
                    return new RemoteWebDriver(DesiredCapabilities.HtmlUnitWithJavaScript());
                default:
                    throw new BrowserNotSupportedException(browser, GetType());
            }
        }
    }
}