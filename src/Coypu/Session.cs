using System;
using Coypu.Robustness;
using Coypu.WebRequests;

namespace Coypu
{
    /// <summary>
    /// A browser session
    /// </summary>
    public class Session : DriverScope<Session>, IDisposable
    {
        private readonly RestrictedResourceDownloader restrictedResourceDownloader;

        internal bool WasDisposed { get; private set; }

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
            get { return driver.Location; }
        }

        internal Session(Driver driver, RobustWrapper robustWrapper, Waiter waiter, RestrictedResourceDownloader restrictedResourceDownloader, UrlBuilder urlBuilder)
            : base(driver,robustWrapper,waiter,urlBuilder)
        {
            this.restrictedResourceDownloader = restrictedResourceDownloader;
        }

        /// <summary>
        /// Disposes the current session, closing any open browser.
        /// </summary>
        public void Dispose()
        {
            if (WasDisposed)
                return;

            driver.Dispose();
            WasDisposed = true;
        }

        /// <summary>
        /// Check that a dialog with the specified text appears within the <see cref="Configuration.Timeout"/>
        /// </summary>
        /// <param name="withText">Dialog text</param>
        /// <returns>Whether an element appears</returns>
        public bool HasDialog(string withText)
        {
            return robustWrapper.Query(() => driver.HasDialog(withText), true);
        }

        /// <summary>
        /// Check that a dialog with the specified is not present. Returns as soon as the dialog is not present, or when the <see cref="Configuration.Timeout"/> is reached.
        /// </summary>
        /// <param name="withText">Dialog text</param>
        /// <returns>Whether an element does not appears</returns>
        public bool HasNoDialog(string withText)
        {
            return !robustWrapper.Query(() => driver.HasDialog(withText), false);
        }

        /// <summary>
        /// Accept the first modal dialog to appear within the <see cref="Configuration.Timeout"/>
        /// </summary>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the dialog cannot be found</exception>
        public void AcceptModalDialog()
        {
            RetryUntilTimeout(() => driver.AcceptModalDialog());
        }

        /// <summary>
        /// Cancel the first modal dialog to appear within the <see cref="Configuration.Timeout"/>
        /// </summary>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the dialog cannot be found</exception>
        public void CancelModalDialog()
        {
            RetryUntilTimeout(() => driver.CancelModalDialog());
        }

        /// <summary>
        /// <para>Use an <param name="individualTimeout" /> for everything you do within an <param name="action" /> - temporarilly overriding the <see cref="Configuration.Timeout"/></para>
        /// <para>For when you need an unusually long (or short) timeout for a particular interaction.</para>
        /// <para>E.g.:
        /// <code>
        ///   session.FillIn("Attachment").With(@"c:\coypu\bigfile.mp4");
        ///   session.Click("Upload");
        ///   session.WithIndividualTimeout(Timespan.FromSeconds(60), () => session.ClickButton("Delete bigfile.mp4"));
        ///      </code>
        /// </para>
        /// </summary>
        public void WithIndividualTimeout(TimeSpan individualTimeout, Action action)
        {
            temporaryTimeouts.WithIndividualTimeout<object>(individualTimeout, () =>
                                                                                   {
                                                                                       action();
                                                                                       return null;
                                                                                   });
        }

        /// <summary>
        /// <para>Use an <param name="individualTimeout" /> for everything you do within a <param name="function" /> - temporarilly overriding the <see cref="Configuration.Timeout"/></para>
        /// <para>For when you need an unusually long (or short) timeout for a particular interaction.</para>
        /// <para>E.g.:
        /// <code>
        ///   session.FillIn("Attachment").With(@"c:\coypu\bigfile.mp4");
        ///   session.Click("Upload");
        ///   bool uploaded = session.WithIndividualTimeout(Timespan.FromSeconds(60), () => session.HasContent("File bigfile.mp4 (10.5mb) uploaded successfully"));
        ///      </code>
        /// </para>
        /// </summary>
        public T WithIndividualTimeout<T>(TimeSpan individualTimeout, Func<T> function)
        {
            return temporaryTimeouts.WithIndividualTimeout(individualTimeout, function);
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
            restrictedResourceDownloader.DownloadFile(urlBuilder.GetFullyQualifiedUrl(resource), saveAs);
        }
    }
}