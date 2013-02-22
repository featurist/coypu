using System;
using Coypu.Finders;
using Coypu.Robustness;
using Coypu.WebRequests;

namespace Coypu
{
    /// <summary>
    /// A browser session
    /// </summary>
    public class BrowserSession : BrowserWindow, IDisposable
    {
        private readonly RestrictedResourceDownloader restrictedResourceDownloader;
        internal bool WasDisposed { get; private set; }

        /// <summary>
        /// A new browser session. Control the lifecycle of this session with using{} / session.Dispose()
        /// </summary>
        /// <returns>The new session with default configuration </returns>
        public BrowserSession() : this(new SessionConfiguration())
        {
        }

        /// <summary>
        /// A new browser session. Control the lifecycle of this session with using{} / session.Dispose()
        /// </summary>
        /// <param name="sessionConfiguration">Configuration for this session</param>
        /// <returns>The new session</returns>
        public BrowserSession(SessionConfiguration sessionConfiguration)
            : this(new ActivatorDriverFactory(),
                   sessionConfiguration,
                   new RetryUntilTimeoutRobustWrapper(),
                   new StopwatchWaiter(),
                   new WebClientWithCookies(),
                   new FullyQualifiedUrlBuilder())
        {
        }

        internal BrowserSession(DriverFactory driver, SessionConfiguration sessionConfiguration, RobustWrapper robustWrapper, Waiter waiter, RestrictedResourceDownloader restrictedResourceDownloader, UrlBuilder urlBuilder)
            : base(sessionConfiguration, null, driver.NewWebDriver(sessionConfiguration.Driver, sessionConfiguration.Browser, sessionConfiguration.BrowserOptions), robustWrapper, waiter, urlBuilder)
        {
            this.restrictedResourceDownloader = restrictedResourceDownloader;
        }

        public Driver Driver
        {
            get { return driver; }
        }

        /// <summary>
        /// The native driver for the Coypu.Driver / browser combination you supplied. E.g. for SeleniumWebDriver + Firefox it will currently be a OpenQA.Selenium.Firefox.FirefoxDriver.
        /// </summary>
        public object Native
        {
            get { return driver.Native; }
        }

        /// <summary>
        /// Saves a resource from the web to a local file using the cookies from the current browser session.
        /// Allows you to sign in through the browser and then directly download a resource restricted to signed-in users.
        /// </summary>
        /// <param name="resource"> The location of the resource to download</param>
        /// <param name="saveAs">Path to save the file to</param>
        public void SaveWebResource(string resource, string saveAs)
        {
            restrictedResourceDownloader.SetCookies(driver.GetBrowserCookies());
            restrictedResourceDownloader.DownloadFile(urlBuilder.GetFullyQualifiedUrl(resource, SessionConfiguration), saveAs);
        }

        public BrowserWindow FindWindow(string locator, Options options = null)
        {
            return new RobustWindowScope(driver, SessionConfiguration, robustWrapper, waiter, urlBuilder, SetOptions(options), new WindowFinder(driver, locator, this));
        }

        /// <summary>
        /// Disposes the current session, closing any open browser.
        /// </summary>
        public void Dispose()
        {
            if (WasDisposed)
                return;

            driver.Dispose();

            ActivatorDriverFactory.OpenDrivers--;

            WasDisposed = true;
        }

    }
}