using System;
using System.Linq;
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
                AcceptInsecureCertificates = sessionConfiguration.AcceptInsecureCertificates,
                Proxy = MapProxy(sessionConfiguration.Proxy)
            };
            
            var chromeOptions = new ChromeOptions
            {
                AcceptInsecureCertificates = sessionConfiguration.AcceptInsecureCertificates,
                Proxy = MapProxy(sessionConfiguration.Proxy)
            };
            
            var edgeOptions = new EdgeOptions
            {
                AcceptInsecureCertificates = sessionConfiguration.AcceptInsecureCertificates,
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
                    AcceptInsecureCertificates = sessionConfiguration.AcceptInsecureCertificates,
                    Proxy = MapProxy(sessionConfiguration.Proxy)
                });
            }

            if (browser == Browser.Safari)
            {
                return new SafariDriver(new SafariOptions
                {
                    AcceptInsecureCertificates = sessionConfiguration.AcceptInsecureCertificates,
                    Proxy = MapProxy(sessionConfiguration.Proxy)
                });
            }

            return browser == Browser.InternetExplorer
                ? new InternetExplorerDriver(new InternetExplorerOptions
                {
                    AcceptInsecureCertificates = sessionConfiguration.AcceptInsecureCertificates,
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

            var proxy = new Proxy();

            if (driverProxy.Type == DriverProxyType.Socks)
            {
                proxy.SocksProxy = driverProxy.Server;
                proxy.SocksUserName = driverProxy.Username;
                proxy.SocksPassword = driverProxy.Password;
            }

            if (driverProxy.Type == DriverProxyType.Http)
            {
                proxy.HttpProxy = driverProxy.Server;

                if (driverProxy.Ssl)
                {
                    proxy.SslProxy = driverProxy.Server;
                }
            }

            if (driverProxy.BypassAddresses?.Any() == true)
            {
                proxy.AddBypassAddresses(driverProxy.BypassAddresses);
            }

            return proxy;
        }

        private IWebDriver BrowserNotSupported(Browser browser,
            Exception inner)
        {
            throw new BrowserNotSupportedException(browser, GetType(), inner);
        }
    }
}