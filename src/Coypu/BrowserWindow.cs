using System;
using Coypu.Actions;
using Coypu.Finders;
using Coypu.Queries;
using Coypu.Robustness;

namespace Coypu
{
    /// <summary>
    /// A browser window belonging to a particular browser session
    /// </summary>
    public class BrowserWindow : DriverScope
    {
        internal BrowserWindow(SessionConfiguration SessionConfiguration, ElementFinder elementFinder, Driver driver, RobustWrapper robustWrapper, Waiter waiter, UrlBuilder urlBuilder) 
            : base(SessionConfiguration, elementFinder, driver, robustWrapper, waiter, urlBuilder)
        {
        }

        /// <summary>
        /// Check that a dialog with the specified text appears within the <see cref="SessionConfiguration.Timeout"/>
        /// </summary>
        /// <param name="withText">Dialog text</param>
        /// <returns>Whether an element appears</returns>
        public bool HasDialog(string withText, Options options = null)
        {
            return Query(new HasDialogQuery(driver, withText, this, SetOptions(options)));
        }

        /// <summary>
        /// Check that a dialog with the specified is not present. Returns as soon as the dialog is not present, or when the <see cref="SessionConfiguration.Timeout"/> is reached.
        /// </summary>
        /// <param name="withText">Dialog text</param>
        /// <returns>Whether an element does not appears</returns>
        public bool HasNoDialog(string withText, Options options = null)
        {
            return Query(new HasNoDialogQuery(driver, withText, this, SetOptions(options)));
        }

        /// <summary>
        /// Accept the first modal dialog to appear within the <see cref="SessionConfiguration.Timeout"/>
        /// </summary>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the dialog cannot be found</exception>
        public void AcceptModalDialog(Options options = null)
        {
            RetryUntilTimeout(new AcceptModalDialog(this, driver, SetOptions(options)));
        }

        /// <summary>
        /// Cancel the first modal dialog to appear within the <see cref="SessionConfiguration.Timeout"/>
        /// </summary>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the dialog cannot be found</exception>
        public void CancelModalDialog(Options options = null)
        {
            RetryUntilTimeout(new CancelModalDialog(this, driver, SetOptions(options)));
        }

        /// <summary>
        /// Visit a url in the browser
        /// </summary>
        /// <param name="virtualPath">Virtual paths will use the SessionConfiguration.AppHost,Port,SSL settings. Otherwise supply a fully qualified URL.</param>
        public void Visit(string virtualPath)
        {
            driver.Visit(urlBuilder.GetFullyQualifiedUrl(virtualPath,SessionConfiguration));
        }

        /// <summary>
        /// Fill in a previously found text field
        /// </summary>
        /// <param name="element">The text field</param>
        /// <returns>With</returns>
        public FillInWith FillIn(Element element, Options options = null)
        {
            return new FillInWith(element, driver, robustWrapper, this, SetOptions(options));
        }

    }
}