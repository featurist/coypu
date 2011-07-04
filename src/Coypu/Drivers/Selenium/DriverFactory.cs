using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Android;
using OpenQA.Selenium.Remote;

namespace Coypu.Drivers.Selenium
{
    internal class DriverFactory
    {
        public RemoteWebDriver NewRemoteWebDriver()
        {
            switch (Configuration.Browser)
            {
                case (Browser.Firefox):
                    return new FirefoxDriver();
                case (Browser.InternetExplorer):
                    {
                        DesiredCapabilities ieCapabilities = DesiredCapabilities.InternetExplorer();
                        ieCapabilities.SetCapability("ignoreProtectedModeSettings", true);
                        return new InternetExplorerDriver(ieCapabilities);
                    }
                case (Browser.Chrome):
                    return new ChromeDriver();
                case (Browser.Android):
                    return new AndroidDriver();
                default:
                    throw new BrowserNotSupportedException(Configuration.Browser, GetType());
            }
        }
    }
}