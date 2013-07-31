using System;
using System.Drawing.Imaging;
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
        /// <param name="SessionConfigurationconfiguration for this session</param>
        /// <returns>The new session</returns>
        public BrowserSession(SessionConfiguration SessionConfiguration)
            : this(new ActivatorDriverFactory(),
                   SessionConfiguration,
                   new RetryUntilTimeoutRobustWrapper(),
                   new StopwatchWaiter(),
                   new WebClientWithCookies(),
                   new FullyQualifiedUrlBuilder())
        {
        }

        internal BrowserSession(DriverFactory driver, SessionConfiguration SessionConfiguration, RobustWrapper robustWrapper, Waiter waiter, RestrictedResourceDownloader restrictedResourceDownloader, UrlBuilder urlBuilder)
            : base(SessionConfiguration, null, driver.NewWebDriver(SessionConfiguration.Driver, SessionConfiguration.Browser), robustWrapper, waiter, urlBuilder)
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

        /// <summary>
        /// Find an open browser window or tab by its title or name. If no exact match is found a partial match on title will be considered.
        /// </summary>
        /// <param name="locator">Window title or name</param>
        /// <param name="options">
        /// <para>Override the way Coypu is configured to find elements for this call only.</para>
        /// <para>E.g. A longer wait:</para>
        /// 
        /// <code>new Options{Timeout = TimeSpan.FromSeconds(60)}</code></param>
        /// <returns>The matching BrowserWindow scope</returns>
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