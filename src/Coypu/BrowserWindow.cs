using System.Drawing;
using Coypu.Actions;
using Coypu.Finders;
using Coypu.Queries;
using Coypu.Timing;
#pragma warning disable 1591

namespace Coypu
{
    /// <summary>
    ///     A browser window belonging to a particular browser session
    /// </summary>
    public class BrowserWindow : DriverScope
    {
        internal BrowserWindow(SessionConfiguration sessionConfiguration,
                               ElementFinder elementFinder,
                               IDriver driver,
                               TimingStrategy timingStrategy,
                               Waiter waiter,
                               UrlBuilder urlBuilder,
                               DisambiguationStrategy disambiguationStrategy)
            : base(sessionConfiguration, elementFinder, driver, timingStrategy, waiter, urlBuilder,
                   disambiguationStrategy) { }

        internal BrowserWindow(ElementFinder elementFinder,
                               DriverScope outerScope) : base(elementFinder, outerScope) { }

        /// <summary>
        ///     Returns the page's title displayed in the browser
        /// </summary>
        public string Title => driver.Title(this);

        internal override bool Stale { get; set; }

        /// <summary>
        ///     Check that a dialog with the specified text appears within the <see>
        ///         <cref>SessionConfiguration.Timeout</cref>
        ///     </see>
        /// </summary>
        /// <param name="withText">Dialog text</param>
        /// <param name="options"></param>
        /// <returns>Whether an element appears</returns>
        public bool HasDialog(string withText,
                              Options options = null)
        {
            return Query(new HasDialogQuery(driver, withText, this, Merge(options)));
        }

        /// <summary>
        ///     Check that a dialog with the specified is not present. Returns as soon as the dialog is not present, or when the
        ///     <see>
        ///         <cref>SessionConfiguration.Timeout</cref>
        ///     </see>
        ///     is reached.
        /// </summary>
        /// <param name="withText">Dialog text</param>
        /// <param name="options"></param>
        /// <returns>Whether an element does not appears</returns>
        public bool HasNoDialog(string withText,
                                Options options = null)
        {
            return Query(new HasNoDialogQuery(driver, withText, this, Merge(options)));
        }

        /// <summary>
        ///     Accept the first modal dialog to appear within the <see>
        ///         <cref>SessionConfiguration.Timeout</cref>
        ///     </see>
        /// </summary>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the dialog cannot be found</exception>
        public void AcceptModalDialog(Options options = null)
        {
            RetryUntilTimeout(new AcceptModalDialog(this, driver, Merge(options)));
        }

        /// <summary>
        ///     Cancel the first modal dialog to appear within the <see>
        ///         <cref>SessionConfiguration.Timeout</cref>
        ///     </see>
        /// </summary>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the dialog cannot be found</exception>
        public void CancelModalDialog(Options options = null)
        {
            RetryUntilTimeout(new CancelModalDialog(this, driver, Merge(options)));
        }

        /// <summary>
        ///     Visit a url in the browser
        /// </summary>
        /// <param name="virtualPath">
        ///     Virtual paths will use the SessionConfiguration.AppHost,Port,SSL settings. Otherwise supply a
        ///     fully qualified URL.
        /// </param>
        public void Visit(string virtualPath)
        {
            driver.Visit(urlBuilder.GetFullyQualifiedUrl(virtualPath, SessionConfiguration), this);
        }

        /// <summary>
        ///     Navigate back to the previous location in the browser's history
        /// </summary>
        public void GoBack()
        {
            driver.GoBack(this);
        }

        /// <summary>
        ///     Navigate forward to the next location in the browser's history
        /// </summary>
        public void GoForward()
        {
            driver.GoForward(this);
        }
        
        /// <summary>
        ///     Executes custom javascript in the browser
        /// </summary>
        /// <param name="javascript">JavaScript to execute</param>
        /// <param name="args">Script arguments to be passed down to the browser</param>
        /// <returns>Anything returned from the script</returns>
        public object ExecuteScript(string javascript,
                                    params object[] args)
        {
            return driver.ExecuteScript(javascript, this, args);
        }

        /// <summary>
        ///     Maximises this browser window
        /// </summary>
        public void MaximiseWindow()
        {
            driver.MaximiseWindow(this);
        }

        /// <summary>
        ///     Resizes this browser window to the supplied dimensions
        /// </summary>
        /// <param name="width">The required width</param>
        /// <param name="height">The required height</param>
        public void ResizeTo(int width,
                             int height)
        {
            driver.ResizeTo(new Size(width, height), this);
        }

        /// <summary>
        ///     Refreshes the current browser window page
        /// </summary>
        public void Refresh()
        {
            driver.Refresh(this);
        }

        public void SaveScreenshot(string saveAs)
        {
            driver.SaveScreenshot(saveAs, this);
        }

        /// <summary>
        ///     <para>Check if this window exists within the <see>
        ///             <cref>SessionConfiguration.Timeout</cref>
        ///         </see>
        ///     </para>
        /// </summary>
        /// <param name="options">
        ///     <para>Override the way Coypu is configured to find elements for this call only.</para>
        ///     <para>E.g. A longer wait:</para>
        ///     <code>new Options{Timeout = TimeSpan.FromSeconds(60)}</code>
        /// </param>
        public bool Exists(Options options = null)
        {
            return Query(new WindowExistsQuery(this, Merge(options)));
        }

        /// <summary>
        ///     <para>Check if this window becomes missing within the <see>
        ///             <cref>SessionConfiguration.Timeout</cref>
        ///         </see>
        ///     </para>
        /// </summary>
        /// <param name="options">
        ///     <para>Override the way Coypu is configured to find elements for this call only.</para>
        ///     <para>E.g. A longer wait:</para>
        ///     <code>new Options{Timeout = TimeSpan.FromSeconds(60)}</code>
        /// </param>
        public bool Missing(Options options = null)
        {
            return Query(new WindowMissingQuery(this, Merge(options)));
        }
    }
}