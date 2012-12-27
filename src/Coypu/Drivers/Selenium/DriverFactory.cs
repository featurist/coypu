using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Android;
using OpenQA.Selenium.PhantomJS;
using OpenQA.Selenium.Remote;

namespace Coypu.Drivers.Selenium
{
    internal class DriverFactory
    {
        public IWebDriver NewWebDriver(Browser browser) {
            return NewWebDriver(browser, new Dictionary<Browser, object>());
        }

        public IWebDriver NewWebDriver(Browser browser, IDictionary<Browser, object> browserOptions)  {
            if (browser == Browser.Firefox)
                return new FirefoxDriver();
            if (browser == Browser.InternetExplorer)
            {
                InternetExplorerOptions options;

                if (browserOptions.ContainsKey(Browser.InternetExplorer))
                {
                    options = browserOptions[Browser.InternetExplorer] as InternetExplorerOptions;

                    // Make sure options provided are of the expected type for Internet Explorer
                    if (options == null)
                    {
                        throw new ArgumentException(
                            "Options object provided for Internet Explorer was not the expected type (Selenium's InternetExplorerOptions).");
                    }
                }
                else
                {
                    // Default options
                    options = new InternetExplorerOptions
                                  {
                                      IntroduceInstabilityByIgnoringProtectedModeSettings = true,
                                      EnableNativeEvents = true
                                  };
                }

                return new InternetExplorerDriver(options);
            }
            if (browser == Browser.Chrome)
                return new ChromeDriver();
            if (browser == Browser.Android)
                return new AndroidDriver();
            if (browser == Browser.HtmlUnit)
                return new RemoteWebDriver(DesiredCapabilities.HtmlUnit());
            if (browser == Browser.HtmlUnitWithJavaScript) {
                DesiredCapabilities desiredCapabilities = DesiredCapabilities.HtmlUnit();
                desiredCapabilities.IsJavaScriptEnabled = true;
                return new RemoteWebDriver(desiredCapabilities);
            }
            if (browser == Browser.PhantomJS)
                return GetPhantomJSDriver(browserOptions);

            return browserNotSupported(browser,null);
        }

        private static IWebDriver GetPhantomJSDriver(IDictionary<Browser, object> browserOptions)
        {
            if (browserOptions.ContainsKey(Browser.PhantomJS))
            {
                // Try processing options as pre-initialized PhantomJSDriverService from Selenium
                var driverService = browserOptions[Browser.PhantomJS] as PhantomJSDriverService;

                // PhantomJSDriverService was not provided?
                if (driverService == null)
                {
                    // Try processing options as JSON configuration for PhantomJS
                    string jsonOptions = browserOptions[Browser.PhantomJS] as string;

                    if (jsonOptions != null)
                    {
                        try
                        {
                            // Try deserializing JSON into a default driver service instance
                            driverService = JsonConvert.DeserializeObject<PhantomJSDriverService>(jsonOptions);
                        }
                        catch (Exception ex)
                        {
                            throw new SerializationException(
                                "Unable to deserialize the string value supplied as the browser options for PhantomJS into a PhantomJSDriverService instance.  Please review the PhantomJS documentation for JSON configuration, or serialize an instance of a PhantomJSDriverService instance.",
                                ex);
                        }
                    }
                }

                // Make sure options provided are of the expected type for PhantomJS
                if (driverService == null)
                {
                    throw new ArgumentException(
                        "The browser options provided for PhantomJS were not the expected type (Selenium's PhantomJSDriverService, or a JSON string containing the configuration, as specified in the PhantomJS command-line arguments documentation).");
                }

                return new PhantomJSDriver(driverService);
            }

            // Default to PhantomJS with no specific options
            return new PhantomJSDriver();
        }

        private IWebDriver browserNotSupported(Browser browser, Exception inner) {
            throw new BrowserNotSupportedException(browser, GetType(), inner);
        }
    }
}