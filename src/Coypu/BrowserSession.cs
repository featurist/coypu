using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Coypu.Actions;
using Coypu.Finders;
using Coypu.Queries;
using Coypu.Robustness;
using Coypu.WebRequests;

namespace Coypu
{
    /// <summary>
    /// A browser session
    /// </summary>
    public class BrowserSession : Scope<BrowserSession>, IDisposable
    {
        private readonly Driver driver;
        private readonly Configuration configuration;
        private readonly RestrictedResourceDownloader restrictedResourceDownloader;
        private readonly UrlBuilder urlBuilder;

        internal bool WasDisposed { get; private set; }
        private readonly DriverScope driverScope;

        /// <summary>
        /// A new browser session. Control the lifecycle of this session with using{} / session.Dispose()
        /// </summary>
        /// <returns>The new session with default configuration </returns>
        public BrowserSession() : this(Configuration.Default())
        {
        }

        /// <summary>
        /// A new browser session. Control the lifecycle of this session with using{} / session.Dispose()
        /// </summary>
        /// <param name="configuration">Your configuration for this session</param>
        /// <returns>The new session</returns>
        public BrowserSession(Configuration configuration) : this(new ActivatorDriverFactory(),
                               configuration,
                               new RetryUntilTimeoutRobustWrapper(),
                               new ThreadSleepWaiter(),
                               new WebClientWithCookies(),
                               new FullyQualifiedUrlBuilder())
        {
        }

        internal BrowserSession(DriverFactory driverFactory, Configuration configuration, RobustWrapper robustWrapper, Waiter waiter, RestrictedResourceDownloader restrictedResourceDownloader, UrlBuilder urlBuilder)
        {
            driver = driverFactory.NewWebDriver(configuration.Driver, configuration.Browser);
            driverScope = new DriverScope(configuration,new DocumentElementFinder(driver), driver, robustWrapper, waiter, urlBuilder);
            this.configuration = configuration;
            this.restrictedResourceDownloader = restrictedResourceDownloader;
            this.urlBuilder = urlBuilder;
        }

        internal DriverScope DriverScope
        {
            get { return driverScope; }
        }

        internal Driver Driver
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
        /// The current location of the browser
        /// </summary>
        public Uri Location
        {
            get { return DriverScope.Location; }
        }

        /// <summary>
        /// Disposes the current session, closing any open browser.
        /// </summary>
        public void Dispose()
        {
            if (WasDisposed)
                return;

            Console.Write("Disposing driver...");

            driver.Dispose();

            Console.Write("closed.");
            ActivatorDriverFactory.OpenDrivers--;
            Console.WriteLine(ActivatorDriverFactory.OpenDrivers + " drivers open.");

            WasDisposed = true;
        }

        /// <summary>
        /// Check that a dialog with the specified text appears within the <see cref="Configuration.Timeout"/>
        /// </summary>
        /// <param name="withText">Dialog text</param>
        /// <returns>Whether an element appears</returns>
        public bool HasDialog(string withText)
        {
            return Query(new HasDialogQuery(driver, withText, driverScope));
        }

        /// <summary>
        /// Check that a dialog with the specified is not present. Returns as soon as the dialog is not present, or when the <see cref="Configuration.Timeout"/> is reached.
        /// </summary>
        /// <param name="withText">Dialog text</param>
        /// <returns>Whether an element does not appears</returns>
        public bool HasNoDialog(string withText)
        {
            return Query(new HasNoDialogQuery(driver, withText, driverScope));
        }

        /// <summary>
        /// Accept the first modal dialog to appear within the <see cref="Configuration.Timeout"/>
        /// </summary>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the dialog cannot be found</exception>
        public void AcceptModalDialog()
        {
            driverScope.RetryUntilTimeout(new AcceptModalDialog(driver, DriverScope.Timeout,DriverScope.RetryInterval));
        }

        /// <summary>
        /// Cancel the first modal dialog to appear within the <see cref="Configuration.Timeout"/>
        /// </summary>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the dialog cannot be found</exception>
        public void CancelModalDialog()
        {
            driverScope.RetryUntilTimeout(new CancelModalDialog(driver,DriverScope.Timeout, DriverScope.RetryInterval));
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
            restrictedResourceDownloader.DownloadFile(urlBuilder.GetFullyQualifiedUrl(resource,configuration), saveAs);
        }

        public BrowserSession ClickButton(string locator)
        {
            driverScope.ClickButton(locator);
            return this;
        }

        public BrowserSession ClickLink(string locator)
        {
            driverScope.ClickLink(locator);
            return this;
        }

        public BrowserSession ClickButton(string locator, Query<bool> until)
        {
            driverScope.ClickButton(locator, until);
            return this;
        }

        public BrowserSession ClickLink(string locator, Query<bool> until)
        {
            driverScope.ClickLink(locator, until);
            return this;
        }

        /// <summary>
        /// Visit a url in the browser
        /// </summary>
        /// <param name="virtualPath">Virtual paths will use the Configuration.AppHost,Port,SSL settings. Otherwise supply a fully qualified URL.</param>
        public BrowserSession Visit(string virtualPath)
        {
            driver.Visit(urlBuilder.GetFullyQualifiedUrl(virtualPath,configuration));
            return this;
        }

        public ElementScope FindButton(string locator)
        {
            return driverScope.FindButton(locator);
        }

        public ElementScope FindLink(string locator)
        {
            return driverScope.FindLink(locator);
        }

        public ElementScope FindField(string locator)
        {
            return driverScope.FindField(locator);
        }

        public FillInWith FillIn(string locator)
        {
            return driverScope.FillIn(locator);
        }

        public FillInWith FillIn(Element element)
        {
            return driverScope.FillIn(element);
        }

        public SelectFrom Select(string option)
        {
            return driverScope.Select(option);
        }

        public bool HasContent(string text)
        {
            return driverScope.HasContent(text);
        }

        public bool HasContentMatch(Regex pattern)
        {
            return driverScope.HasContentMatch(pattern);
        }

        public bool HasNoContent(string text)
        {
            return driverScope.HasNoContent(text);
        }

        public bool HasNoContentMatch(Regex pattern)
        {
            return driverScope.HasNoContentMatch(pattern);
        }

        public bool HasCss(string cssSelector)
        {
            return driverScope.HasCss(cssSelector);
        }

        public bool HasNoCss(string cssSelector)
        {
            return driverScope.HasNoCss(cssSelector);
        }

        public bool HasXPath(string xpath)
        {
            return driverScope.HasXPath(xpath);
        }

        public bool HasNoXPath(string xpath)
        {
            return driverScope.HasNoXPath(xpath);
        }

        public ElementScope FindCss(string cssSelector)
        {
            return driverScope.FindCss(cssSelector);
        }

        public ElementScope FindXPath(string xpath)
        {
            return driverScope.FindXPath(xpath);
        }

        public IEnumerable<ElementFound> FindAllCss(string cssSelector)
        {
            return driverScope.FindAllCss(cssSelector);
        }

        public IEnumerable<ElementFound> FindAllXPath(string xpath)
        {
            return driverScope.FindAllXPath(xpath);
        }

        public ElementScope FindSection(string locator)
        {
            return driverScope.FindSection(locator);
        }

        public IFrameElementScope FindIFrame(string locator)
        {
            return driverScope.FindIFrame(locator);
        }

        public ElementScope FindFieldset(string locator)
        {
            return driverScope.FindFieldset(locator);
        }

        public ElementScope FindId(string id)
        {
            return driverScope.FindId(id);
        }

        public BrowserSession Check(string locator)
        {
            driverScope.Check(locator);
            return this;
        }

        public BrowserSession Uncheck(string locator)
        {
            driverScope.Uncheck(locator);
            return this;
        }

        public BrowserSession Choose(string locator)
        {
            driverScope.Choose(locator);
            return this;
        }

        public string ExecuteScript(string javascript)
        {
            return driverScope.ExecuteScript(javascript);
        }

        public BrowserSession Hover(Element element)
        {
            driverScope.Hover(element);
            return this;
        }

        public bool Has(ElementScope findElement)
        {
            return driverScope.Has(findElement);
        }

        public bool HasNo(ElementScope findElement)
        {
            return driverScope.HasNo(findElement);
        }

        public void RetryUntilTimeout(Action action)
        {
            driverScope.RetryUntilTimeout(action);
        }

        public TResult RetryUntilTimeout<TResult>(Func<TResult> function)
        {
            return driverScope.RetryUntilTimeout(function);
        }

        public void RetryUntilTimeout(DriverAction driverAction)
        {
            driverScope.RetryUntilTimeout(driverAction);
        }

        public T Query<T>(Func<T> query, T expecting)
        {
            return driverScope.Query(query, expecting);
        }

        public T Query<T>(Query<T> query)
        {
            return driverScope.Query(query);
        }

        public void TryUntil(Action tryThis, Func<bool> until, TimeSpan waitBeforeRetry)
        {
            driverScope.TryUntil(tryThis, until, waitBeforeRetry);
        }

        public void TryUntil(DriverAction tryThis, Query<bool> until)
        {
            driverScope.TryUntil(tryThis, until);
        }

        public State FindState(params State[] states)
        {
            return driverScope.FindState(states);
        }

        public BrowserSession ConsideringInvisibleElements()
        {
            driverScope.ConsideringInvisibleElements();
            return this;
        }

        public BrowserSession ConsideringOnlyVisibleElements()
        {
            driverScope.ConsideringOnlyVisibleElements();
            return this;
        }

        public BrowserSession WithTimeout(TimeSpan timeout)
        {
            driverScope.WithTimeout(timeout);
            return this;
        }
    }
}