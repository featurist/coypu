using System;
using Coypu.Finders;
using Coypu.Timing;
using Coypu.WebRequests;

namespace Coypu
{
    /// <summary>
    ///     A browser session
    /// </summary>
    public class BrowserSession : BrowserWindow, IDisposable
    {
        private readonly RestrictedResourceDownloader _restrictedResourceDownloader;

        /// <summary>
        ///     A new browser session. Control the lifecycle of this session with using{} / session.Dispose()
        /// </summary>
        /// <returns>The new session with default configuration </returns>
        public BrowserSession()
            : this(new SessionConfiguration()) { }

        /// <summary>
        ///     A new browser session. Control the lifecycle of this session with using{} / session.Dispose()
        /// </summary>
        /// <param name="sessionConfiguration">configuration for this session</param>
        /// <returns>The new session</returns>
        public BrowserSession(SessionConfiguration sessionConfiguration)
            : this(sessionConfiguration,
                   new ActivatorDriverFactory(),
                   new RetryUntilTimeoutTimingStrategy(),
                   new StopwatchWaiter(),
                   new FullyQualifiedUrlBuilder(),
                   new FinderOptionsDisambiguationStrategy(),
                   new WebClientWithCookies()) { }

        /// <summary>
        ///     A new browser session with defined driver.
        ///     Replaces sessionConfiguration driver.
        /// </summary>
        /// <param name="driver"></param>
        public BrowserSession(IDriver driver)
            : this(new SessionConfiguration(),
                   driver,
                   new RetryUntilTimeoutTimingStrategy(),
                   new StopwatchWaiter(),
                   new FullyQualifiedUrlBuilder(),
                   new FinderOptionsDisambiguationStrategy(),
                   new WebClientWithCookies()) { }

        /// <summary>
        ///     A new browser session with defined driver.
        ///     Replaces sessionConfiguration driver.
        /// </summary>
        /// <param name="sessionConfiguration"></param>
        /// <param name="driver"></param>
        public BrowserSession(SessionConfiguration sessionConfiguration,
                              IDriver driver)
            : this(sessionConfiguration,
                   driver,
                   new RetryUntilTimeoutTimingStrategy(),
                   new StopwatchWaiter(),
                   new FullyQualifiedUrlBuilder(),
                   new FinderOptionsDisambiguationStrategy(),
                   new WebClientWithCookies()) { }

        internal BrowserSession(SessionConfiguration sessionConfiguration,
                                DriverFactory driverFactory,
                                TimingStrategy timingStrategy,
                                Waiter waiter,
                                UrlBuilder urlBuilder,
                                DisambiguationStrategy disambiguationStrategy,
                                RestrictedResourceDownloader restrictedResourceDownloader)
            : base(sessionConfiguration,
                   null,
                   driverFactory.NewWebDriver(sessionConfiguration.Driver, sessionConfiguration.Browser),
                   timingStrategy,
                   waiter,
                   urlBuilder,
                   disambiguationStrategy)
        {
            this._restrictedResourceDownloader = restrictedResourceDownloader;
        }

        internal BrowserSession(SessionConfiguration sessionConfiguration,
                                IDriver driver,
                                TimingStrategy timingStrategy,
                                Waiter waiter,
                                UrlBuilder urlBuilder,
                                DisambiguationStrategy disambiguationStrategy,
                                RestrictedResourceDownloader restrictedResourceDownloader)
            : base(sessionConfiguration,
                   null,
                   driver,
                   timingStrategy,
                   waiter,
                   urlBuilder,
                   disambiguationStrategy)
        {
            this._restrictedResourceDownloader = restrictedResourceDownloader;
        }

        /// <summary>
        ///     Access to grand-parent DriverScope's driver.
        /// </summary>
        public IDriver Driver => _driver;

        /// <summary>
        ///     The native driver for the Coypu.Driver / browser combination you supplied. E.g. for SeleniumWebDriver + Firefox it
        ///     will currently be a OpenQA.Selenium.Firefox.FirefoxDriver.
        /// </summary>
        public object Native => _driver.Native;

        internal bool WasDisposed { get; private set; }

        /// <summary>
        ///     Disposes the current session, closing any open browser.
        /// </summary>
        public void Dispose()
        {
            if (WasDisposed) return;
            _driver.Dispose();
            ActivatorDriverFactory.OpenDrivers--;
            WasDisposed = true;
        }

        /// <summary>
        ///     Saves a resource from the web to a local file using the cookies from the current browser session.
        ///     Allows you to sign in through the browser and then directly download a resource restricted to signed-in users.
        /// </summary>
        /// <param name="resource"> The location of the resource to download</param>
        /// <param name="saveAs">Path to save the file to</param>
        public void SaveWebResource(string resource,
                                    string saveAs)
        {
            _restrictedResourceDownloader.SetCookies(_driver.GetBrowserCookies());
            _restrictedResourceDownloader.DownloadFile(UrlBuilder.GetFullyQualifiedUrl(resource, SessionConfiguration), saveAs);
        }

        /// <summary>
        ///     Find an open browser window or tab by its title or name. If no exact match is found a partial match on title will
        ///     be considered.
        /// </summary>
        /// <param name="locator">Window title or name</param>
        /// <param name="options">
        ///     <para>Override the way Coypu is configured to find elements for this call only.</para>
        ///     <para>E.g. A longer wait:</para>
        ///     <code>new Options{Timeout = TimeSpan.FromSeconds(60)}</code>
        /// </param>
        /// <returns>The matching BrowserWindow scope</returns>
        public BrowserWindow FindWindow(string locator,
                                        Options options = null)
        {
            return new BrowserWindow(new WindowFinder(_driver, locator, this, Merge(options)), this);
        }
    }
}