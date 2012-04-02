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

        public BrowserWindow FindWindow(string titleOrName, Options options = null)
        {
            return new RobustWindowScope(driver, SessionConfiguration, robustWrapper, waiter, urlBuilder, SetOptions(options), new WindowFinder(driver, titleOrName, this));
        }

        /// <summary>
        /// Disposes the current session, closing any open browser.
        /// </summary>
        public void Dispose()
        {
            if (WasDisposed)
                return;

            Console.WriteLine("Disposing driver");

            driver.Dispose();

            Console.WriteLine("Disposed");
            ActivatorDriverFactory.OpenDrivers--;
            Console.WriteLine(ActivatorDriverFactory.OpenDrivers + " driver(s) open.");

            WasDisposed = true;
        }

        // The following methods are obsolete in 0.8.0 and up, and will throw NotSupportedException
        // They to be kept here for a while to ease transition to 0.8.0 and should be removed in 0.9.0

        public const string SEE_README_SCOPE = Session.SEE_README + "\r\nE.g. browser.FindFieldset(\"Search\").Select(\"Mazda\").From(\"Make\");";
        public const string SEE_README_CONFIG = Session.SEE_README + "\r\n  Pass SessionConfiguration to new BrowserSession(), or override Options on each call.";
        [Obsolete(SEE_README_SCOPE)]
        public void Within(Func<Element> finder, Action action)
        {
            throw new NotSupportedException(SEE_README_SCOPE);
        }
        [Obsolete(SEE_README_SCOPE)]
        public void WithinFieldset(string locator, Action action)
        {
            throw new NotSupportedException(SEE_README_SCOPE);
        }
        [Obsolete(SEE_README_SCOPE)]
        public void WithinSection(string locator, Action action)
        {
            throw new NotSupportedException(SEE_README_SCOPE);
        }
        [Obsolete(SEE_README_SCOPE)]
        public void WithinIFrame(string locator, Action action)
        {
            throw new NotSupportedException(SEE_README_SCOPE);
        }
        [Obsolete(SEE_README_CONFIG)]
        public void WithIndividualTimeout(TimeSpan timeout, Action action)
        {
            throw new NotSupportedException(SEE_README_CONFIG);
        }
        [Obsolete(SEE_README_CONFIG)]
        public T WithIndividualTimeout<T>(Func<T> func)
        {
            throw new NotSupportedException(SEE_README_CONFIG);
        }
        [Obsolete(SEE_README_CONFIG)]
        public void ConsideringInvisibleElements(Action action)
        {
            throw new NotSupportedException(SEE_README_CONFIG);
        }
        [Obsolete(SEE_README_CONFIG)]
        public T ConsideringInvisibleElements<T>(Func<T> func)
        {
            throw new NotSupportedException(SEE_README_CONFIG);
        }
        const string SEE_README_EXISTS = Session.SEE_README + "\r\n  Use browser.FindX(...).Exists()";
        [Obsolete(SEE_README_EXISTS)]
        public bool Has(Func<Element> finder)
        {
            throw new NotSupportedException(SEE_README_EXISTS);
        }
        const string SEE_README_MISSING = Session.SEE_README + "\r\n  Use browser.FindX(...).Missing()";
        [Obsolete(SEE_README_MISSING)]
        public bool HasNo(Func<Element> finder)
        {
            throw new NotSupportedException(SEE_README_MISSING);
        }
        const string SEE_README_CLICK = Session.SEE_README + "\r\n  Use browser.FindX(...).Click()";
        [Obsolete(SEE_README_CLICK)]
        public void Click(Element element)
        {
            throw new NotSupportedException(SEE_README_CLICK);
        }
        [Obsolete(SEE_README_CLICK)]
        public void Click(Func<Element> finder)
        {
            throw new NotSupportedException(SEE_README_CLICK);
        }
        const string SEE_README_HOVER = Session.SEE_README + "\r\n  Use browser.FindX(...).Hover()";

        [Obsolete(SEE_README_HOVER)]
        public void Hover(Element element)
        {
            throw new NotSupportedException(SEE_README_HOVER);
        }
        [Obsolete(SEE_README_HOVER)]
        public void Hover(Func<Element> finder)
        {
            throw new NotSupportedException(SEE_README_HOVER);
        }
    }

    [Obsolete(USE_NEW_BROWSER_SESSION)]
    public class Session : BrowserSession
    {
        public const string OBSOLETE_IN_0_8 = "No longer supported in 0.8.0 and above - will be removed in the next minor release.";
        public const string SEE_README = OBSOLETE_IN_0_8 + "\r\n  See README for latest DSL: https://github.com/coypu/coypu/blob/master/README.md";
        public const string USE_NEW_BROWSER_SESSION = SEE_README + "\r\n  Use constructor: new BrowerSession(SessionConfiguration) instead.";

        [Obsolete(USE_NEW_BROWSER_SESSION)]
        private Session()
        {
        }
    }
    [Obsolete(Session.USE_NEW_BROWSER_SESSION)]
    public class Browser
    {
        [Obsolete(Session.USE_NEW_BROWSER_SESSION)]
        public static Session Session
        {
            get { throw new NotSupportedException(Session.USE_NEW_BROWSER_SESSION); }
        }
        [Obsolete(Session.USE_NEW_BROWSER_SESSION)]
        public static Session NewSession()
        {
            throw new NotSupportedException(Session.USE_NEW_BROWSER_SESSION);
        }
        [Obsolete(Session.USE_NEW_BROWSER_SESSION)]
        public static Session EndSession()
        {
            throw new NotSupportedException(Session.USE_NEW_BROWSER_SESSION);
        }
    }

    [Obsolete(Session.USE_NEW_BROWSER_SESSION)]
    public class Configuration
    {
        [Obsolete(Session.USE_NEW_BROWSER_SESSION)]
        public static Drivers.Browser Browser
        {
            get { throw new NotSupportedException(Session.USE_NEW_BROWSER_SESSION); }
            set { throw new NotSupportedException(Session.USE_NEW_BROWSER_SESSION); }
        }
        [Obsolete(Session.USE_NEW_BROWSER_SESSION)]
        public static Type Driver
        {
            get { throw new NotSupportedException(Session.USE_NEW_BROWSER_SESSION); }
            set { throw new NotSupportedException(Session.USE_NEW_BROWSER_SESSION); }
        }
        [Obsolete(Session.USE_NEW_BROWSER_SESSION)]
        public static string AppHost
        {
            get{throw new NotSupportedException(Session.USE_NEW_BROWSER_SESSION);}
            set{throw new NotSupportedException(Session.USE_NEW_BROWSER_SESSION);}
        }
        [Obsolete(Session.USE_NEW_BROWSER_SESSION)]
        public static int Port
        {
            get { throw new NotSupportedException(Session.USE_NEW_BROWSER_SESSION); }
            set { throw new NotSupportedException(Session.USE_NEW_BROWSER_SESSION); }
        }
        [Obsolete(Session.USE_NEW_BROWSER_SESSION)]
        public static bool SSL
        {
            get { throw new NotSupportedException(Session.USE_NEW_BROWSER_SESSION); }
            set { throw new NotSupportedException(Session.USE_NEW_BROWSER_SESSION); }
        }
        [Obsolete(Session.USE_NEW_BROWSER_SESSION)]
        public static TimeSpan Timeout
        {
            get { throw new NotSupportedException(Session.USE_NEW_BROWSER_SESSION); }
            set { throw new NotSupportedException(Session.USE_NEW_BROWSER_SESSION); }
        }
        [Obsolete(Session.USE_NEW_BROWSER_SESSION)]
        public static TimeSpan RetryInterval
        {
            get { throw new NotSupportedException(Session.USE_NEW_BROWSER_SESSION); }
            set { throw new NotSupportedException(Session.USE_NEW_BROWSER_SESSION); }
        }
        [Obsolete(Session.USE_NEW_BROWSER_SESSION)]
        public static TimeSpan WaitBeforeClick
        {
            get { throw new NotSupportedException(Session.USE_NEW_BROWSER_SESSION); }
            set { throw new NotSupportedException(Session.USE_NEW_BROWSER_SESSION); }
        }
        [Obsolete(Session.USE_NEW_BROWSER_SESSION)]
        public static bool ConsiderInvisibleElements
        {
            get { throw new NotSupportedException(Session.USE_NEW_BROWSER_SESSION); }
            set { throw new NotSupportedException(Session.USE_NEW_BROWSER_SESSION); }
        }
        [Obsolete(Session.USE_NEW_BROWSER_SESSION)]
        public static void Reset()
        {
            throw new NotSupportedException(Session.USE_NEW_BROWSER_SESSION);
        }
    }
}