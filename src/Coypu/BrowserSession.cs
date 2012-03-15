using Coypu.Finders;
using Coypu.Robustness;
using Coypu.WebRequests;

namespace Coypu
{
    /// <summary>
    /// A browser session
    /// </summary>
    public class BrowserSession : BrowserWindow
    {
        private readonly RestrictedResourceDownloader restrictedResourceDownloader;

        /// <summary>
        /// A new browser session. Control the lifecycle of this session with using{} / session.Dispose()
        /// </summary>
        /// <returns>The new session with default configuration </returns>
        public BrowserSession() : this(new Configuration())
        {
        }

        /// <summary>
        /// A new browser session. Control the lifecycle of this session with using{} / session.Dispose()
        /// </summary>
        /// <param name="configuration">Your configuration for this session</param>
        /// <returns>The new session</returns>
        public BrowserSession(Configuration configuration)
            : this(new ActivatorDriverFactory(),
                   configuration,
                   new RetryUntilTimeoutRobustWrapper(),
                   new StopwatchWaiter(),
                   new WebClientWithCookies(),
                   new FullyQualifiedUrlBuilder())
        {
        }

        internal BrowserSession(DriverFactory driver, Configuration configuration, RobustWrapper robustWrapper, Waiter waiter, RestrictedResourceDownloader restrictedResourceDownloader, UrlBuilder urlBuilder)
            : base(configuration, null, driver.NewWebDriver(configuration.Driver, configuration.Browser), robustWrapper, waiter, urlBuilder)
        {
            this.restrictedResourceDownloader = restrictedResourceDownloader;
        }

        public Driver Driver
        {
            get { return driver; }
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
            restrictedResourceDownloader.DownloadFile(urlBuilder.GetFullyQualifiedUrl(resource, configuration), saveAs);
        }

        public BrowserWindow FindWindow(string titleOrName, Options options = null)
        {
            return new RobustWindowScope(driver, configuration, robustWrapper, waiter, urlBuilder, SetOptions(options), new WindowFinder(driver, titleOrName, this));
        }
    }
}