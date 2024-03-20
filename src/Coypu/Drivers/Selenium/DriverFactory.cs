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
        public IWebDriver NewWebDriver(SessionConfiguration sessionConfiguration)
        {
            var browser = sessionConfiguration.Browser;
            
            var firefoxOptions = new FirefoxOptions
            {
                Proxy = MapProxy(sessionConfiguration.Proxy)
            };
            
            var chromeOptions = new ChromeOptions
            {
                Proxy = MapProxy(sessionConfiguration.Proxy)
            };
            
            var edgeOptions = new EdgeOptions
            {
                Proxy = MapProxy(sessionConfiguration.Proxy)
            };
            
            if (sessionConfiguration.Headless)
            {
                firefoxOptions.AddArgument("--headless");
                chromeOptions.AddArgument("--headless=new");
                edgeOptions.AddArgument("headless");
                edgeOptions.AddArgument("disable-gpu");
                
                if (browser == Browser.Safari)
                {
                    throw new NotSupportedException("Safari does not support headless mode");
                }

                if (browser == Browser.Opera)
                {
                    throw new NotSupportedException("Opera does not support headless mode");
                }

                if (browser == Browser.InternetExplorer)
                {
                    throw new NotSupportedException("Internet Explorer does not support headless mode");
                }
            }

            if (browser == Browser.Firefox)
            {
                return new FirefoxDriver(firefoxOptions);
            }

            if (browser == Browser.Chrome)
            {
                return new ChromeDriver(chromeOptions);
            }

            if (browser == Browser.Edge)
            {
                return new EdgeDriver(edgeOptions);
            }

            if (browser == Browser.Opera)
            {
                return new OperaDriver(new OperaOptions
                {
                    Proxy = MapProxy(sessionConfiguration.Proxy)
                });
            }

            if (browser == Browser.Safari)
            {
                return new SafariDriver(new SafariOptions
                {
                    Proxy = MapProxy(sessionConfiguration.Proxy)
                });
            }

            return browser == Browser.InternetExplorer
                ? new InternetExplorerDriver(new InternetExplorerOptions
                {
                    IntroduceInstabilityByIgnoringProtectedModeSettings = true,
                    EnableNativeEvents = true,
                    IgnoreZoomLevel = true,
                    Proxy = MapProxy(sessionConfiguration.Proxy)
                })
                : BrowserNotSupported(browser, null);
        }

        private Proxy MapProxy(DriverProxy driverProxy)
        {
            if (driverProxy is null)
            {
                return null;
            }

            var proxy = new Proxy
            {
                SocksProxy = driverProxy.Type == DriverProxyType.Socks ? driverProxy.Server : null,
                HttpProxy = driverProxy.Type == DriverProxyType.Http ? driverProxy.Server : null,
                SslProxy = driverProxy.Ssl && driverProxy.Type == DriverProxyType.Http ? driverProxy.Server : null,
                SocksUserName = driverProxy.Username,
                SocksPassword = driverProxy.Password,
            };

            if (driverProxy.Ssl)
            {
                proxy.SslProxy = driverProxy.Server;
            }
            
            proxy.AddBypassAddresses(driverProxy.BypassAddresses ?? Array.Empty<string>());

            return proxy;
        }

        private IWebDriver BrowserNotSupported(Browser browser,
            Exception inner)
        {
            throw new BrowserNotSupportedException(browser, GetType(), inner);
        }
    }
}