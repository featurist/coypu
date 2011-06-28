using System;
using System.Collections.Generic;
using Coypu.Drivers;
using Coypu.Robustness;
using System.Text.RegularExpressions;

namespace Coypu
{
    public class Session : IDisposable, RobustWrapper
    {
        private readonly Driver driver;
        private readonly RobustWrapper robustWrapper;
        private readonly Clicker clicker;
        private readonly UrlBuilder urlBuilder;
        public bool WasDisposed { get; private set; }

        internal Driver Driver
        {
            get { return driver; }
        }

        public object Native
        {
            get { return driver.Native; }
        }

        public Session(Driver driver, RobustWrapper robustWrapper, Waiter waiter)
        {
            this.robustWrapper = robustWrapper;
            this.driver = driver;
            clicker = new Clicker(driver, waiter);
            urlBuilder = new UrlBuilder();
        }

        public void Dispose()
        {
            if (WasDisposed) 
                return;

            driver.Dispose();
            WasDisposed = true;
        }

        /// <summary>
        /// Click a button, input of type button|submit|image or div with the css class "button"
        /// </summary>
        /// <param name="locator">The text/value, name or id of the button</param>
        /// <returns>The first matching button</returns>
        public void ClickButton(string locator)
        {
            Robustly(() => clicker.FindAndClickButton(locator));
        }

        /// <summary>
        /// Click the first matching link
        /// </summary>
        /// <param name="locator">The text of the link</param>
        public void ClickLink(string locator)
        {
            Robustly(() => clicker.FindAndClickLink(locator));
        }

        /// <summary>
        /// Click a previously found element
        /// </summary>
        /// <param name="locator">The element to click</param>
        public void Click(Element element)
        {
            Robustly(() => driver.Click(element));
        }

        /// <summary>
        /// Find and click an element robustly
        /// </summary>
        /// <param name="locator">How to find the element</param>
        public void Click(Func<Element> findElement)
        {
            Robustly(() => driver.Click(findElement()));
        }

        /// <summary>
        /// Click a button, input of type button|submit|image or div with the css class "button". 
        /// Wait for a condition to be satisfied for a specified time otherwise click and wait again. 
        /// Continues until the expected condition is satisfied or the Configuration.Timeout is reached
        /// </summary>
        /// <param name="locator">The text/value, name or id of the button</param>
        /// <param name="until">The condition to be satisfied</param>
        /// <param name="waitBetweenRetries">How long to wait for the condition to be satisfied before clicking again</param>
        /// <returns>The first matching button</returns>
        public void ClickButton(string locator, Func<bool> until, TimeSpan waitBetweenRetries)
        {
            TryUntil(() => ClickButton(locator), until, waitBetweenRetries);
        }

        /// <summary>
        /// Click a link and wait for a condition to be satisfied for a specified time otherwise click and wait again. 
        /// Continues until the expected condition is satisfied or the Configuration.Timeout is reached
        /// </summary>
        /// <param name="locator">The text of the link</param>
        /// <param name="until">The condition to be satisfied</param>
        /// <param name="waitBetweenRetries">How long to wait for the condition to be satisfied before clicking again</param>
        /// <returns>The first matching button</returns>
        public void ClickLink(string locator, Func<bool> until, TimeSpan waitBetweenRetries)
        {
            TryUntil(() => ClickLink(locator), until, waitBetweenRetries);
        }

        /// <summary>
        /// Visit a url in the browser.
        /// </summary>
        /// <param name="virtualPath">Virtual path (will be joined to the Configuration.AppHost+Port) or a fully qualified URL</param>
        public void Visit(string virtualPath)
        {
            driver.Visit(urlBuilder.GetFullyQualifiedUrl(virtualPath));
        }

        /// <summary>
        /// Find a button, input of type button|submit|image or div with the css class "button".
        /// </summary>
        /// <param name="locator">The text/value, name or id of the button</param>
        /// <returns>The first matching button</returns>
        public Element FindButton(string locator)
        {
            return Robustly(() => driver.FindButton(locator));
        }

        /// <summary>
        /// Find a link
        /// </summary>
        /// <param name="locator">The text of the link</param>
        /// <returns>The first matching link</returns>
        public Element FindLink(string locator)
        {
            return Robustly(() => driver.FindLink(locator));
        }

        /// <summary>
        /// Find a form field of any type
        /// </summary>
        /// <param name="locator">The text of the associated label element, the id or name, the placeholder text, the value of a radio button, the last part of the id (for asp.net forms testing)</param>
        /// <returns>The first matching field</returns>
        public Element FindField(string locator)
        {
            return Robustly(() => driver.FindField(locator));
        }

        /// <summary>
        /// Find a text field to fill in
        /// </summary>
        /// <param name="locator">The text of the associated label element, the id or name, the placeholder text, the last part of the id (for asp.net forms testing)</param>
        /// <returns>With</returns>
        public FillInWith FillIn(string locator)
        {
            return new FillInWith(locator, driver, robustWrapper);
        }

        /// <summary>
        /// Fill in a previously found text field
        /// </summary>
        /// <param name="element">The text field</param>
        /// <returns>With</returns>
        public FillInWith FillIn(Element element)
        {
            return new FillInWith(element, driver, robustWrapper);
        }

        /// <summary>
        /// Select an option from a select element
        /// </summary>
        /// <param name="option">The text or value of the option to select</param>
        /// <returns>From</returns>
        public SelectFrom Select(string option)
        {
            return new SelectFrom(option, driver, robustWrapper);
        }

        /// <summary>
        /// Check that text appears on the page. 
        /// 
        /// Returns as soon as the text appears, or when the Configuration.Timeout is reached.
        /// </summary>
        /// <param name="text">The exact text to find</param>
        /// <returns>Whether the text appears</returns>
        public bool HasContent(string text)
        {
            return Query(() => driver.HasContent(text), true);
        }

        /// <summary>
        /// Check that text appears on the page using a regular expression
        /// 
        /// Returns as soon as the text appears, or when the Configuration.Timeout is reached.
        /// </summary>
        /// <param name="pattern">The regular expression to match</param>
        /// <returns>Whether the page text matches</returns>
        public bool HasContentMatch(Regex pattern)
        {
            return Query(() => driver.HasContentMatch(pattern), true);
        }

        /// <summary>
        /// Check that text does not appear on the page
        /// 
        /// Returns as soon as the text does not appear, or when the Configuration.Timeout is reached.
        /// </summary>
        /// <param name="text">The exact text expected not to be found</param>
        /// <returns>Whether the text does not appear</returns>
        public bool HasNoContent(string text)
        {
            return Query(() => driver.HasContent(text), false);
        }

        /// <summary>
        /// Check that text does not appear on the page using a regular expression
        /// 
        /// Returns as soon as the text does not appear, or when the Configuration.Timeout is reached.
        /// </summary>
        /// <param name="pattern">The regular expression expected not to match</param>
        /// <returns>Whether the text does not appear</returns>
        public bool HasNoContentMatch(Regex pattern)
        {
            return Query(() => driver.HasContentMatch(pattern), false);
        }

        /// <summary>
        /// Check that an element matching a CSS selector appears on the page. 
        /// 
        /// Returns as soon as the element appears, or when the Configuration.Timeout is reached.
        /// </summary>
        /// <param name="cssSelector">CSS selector</param>
        /// <returns>Whether an element appears</returns>
        public bool HasCss(string cssSelector)
        {
            return Query(() => driver.HasCss(cssSelector), true);
        }

        /// <summary>
        /// Check that an element matching a CSS selector does not appear on the page. 
        /// 
        /// Returns as soon as the element does not appear, or when the Configuration.Timeout is reached.
        /// </summary>
        /// <param name="cssSelector">CSS selector</param>
        /// <returns>Whether an element does not appear</returns>
        public bool HasNoCss(string cssSelector)
        {
            return Query(() => driver.HasCss(cssSelector), false);
        }

        /// <summary>
        /// Check that an element matching an XPath query appears on the page. 
        /// 
        /// Returns as soon as the element appears, or when the Configuration.Timeout is reached.
        /// </summary>
        /// <param name="xpath">XPath query</param>
        /// <returns>Whether an element appears</returns>
        public bool HasXPath(string xpath)
        {
            return Query(() => driver.HasXPath(xpath), true);
        }

        /// <summary>
        /// Check that an element matching an XPath query appears on the page. 
        /// 
        /// Returns as soon as the element appears, or when the Configuration.Timeout is reached.
        /// </summary>
        /// <param name="xpath">XPath query</param>
        /// <returns>Whether an element appears</returns>
        public bool HasNoXPath(string xpath)
        {
            return Query(() => driver.HasXPath(xpath), false);
        }

        public Element FindCss(string cssSelector)
        {
            return Robustly(() => driver.FindCss(cssSelector));
        }

        public Element FindXPath(string xpath)
        {
            return Robustly(() => driver.FindXPath(xpath));
        }

        public IEnumerable<Element> FindAllCss(string cssSelector)
        {
            return driver.FindAllCss(cssSelector);
        }

        public Element FindSection(string locator)
        {
            return Robustly(() => driver.FindSection(locator));
        }

        public Element FindFieldset(string locator)
        {
            return Robustly(() => driver.FindFieldset(locator));
        }

        public Element FindId(string id)
        {
            return Robustly(() => driver.FindId(id));
        }

        public IEnumerable<Element> FindAllXPath(string xpath)
        {
            return driver.FindAllXPath(xpath);
        }

        public void Check(string locator)
        {
            Robustly(() => driver.Check(driver.FindField(locator)));
        }

        public void Uncheck(string locator)
        {
            Robustly(() => driver.Uncheck(driver.FindField(locator)));
        }

        public void Choose(string locator)
        {
            Robustly(() => driver.Choose(driver.FindField(locator)));
        }

        public bool HasDialog(string withText)
        {
            return robustWrapper.Query(() => driver.HasDialog(withText), true);
        }

        public bool HasNoDialog(string withText)
        {
            return robustWrapper.Query(() => driver.HasDialog(withText), false);
        }

        public void AcceptModalDialog()
        {
            Robustly(() => driver.AcceptModalDialog());
        }

        public void CancelModalDialog()
        {
            Robustly(() => driver.CancelModalDialog());
        }

        public void WithIndividualTimeout(TimeSpan individualTimeout, Action action)
        {
            var defaultTimeout = Configuration.Timeout;
            Configuration.Timeout = individualTimeout;
            try
            {
                action();
            }
            finally
            {
                Configuration.Timeout = defaultTimeout;
            }
        }

        public void Within(Func<Element> findScope, Action doThis)
        {
            try
            {
                driver.SetScope(findScope);
                doThis();
            }
            finally
            {
                driver.ClearScope();
            }
        }

        public void WithinFieldset(string locator, Action action)
        {
            Within(() => driver.FindFieldset(locator), action);
        }

        public void WithinSection(string locator, Action action)
        {
            Within(() => driver.FindSection(locator), action);
        }

        public string ExecuteScript(string javascript)
        {
            return driver.ExecuteScript(javascript);
        }

        public void WithinIFrame(string locator, Action action)
        {
            Within(() => driver.FindIFrame(locator), action);
        }

        public void Hover(Func<Element> findElement)
        {
            Robustly(() => driver.Hover(findElement()));
            
        }

        public bool Has(Func<Element> findElement) 
        {
            return Query(BuildZeroTimeoutHasHtmlQuery(findElement), true);
        }

        public bool HasNo(Func<Element> findElement)
        {
            return Query(BuildZeroTimeoutHasHtmlQuery(findElement), false);
        }

        private static Func<bool> BuildZeroTimeoutHasHtmlQuery(Func<Element> findElement)
        {
            Func<bool> query =
                () =>
                    {
                        var outerTimeout = Configuration.Timeout;
                        Configuration.Timeout = TimeSpan.Zero;
                        try
                        {
                            findElement();
                            return true;
                        }
                        catch (MissingHtmlException)
                        {
                            return false;
                        }
                        finally
                        {
                            Configuration.Timeout = outerTimeout;
                        }
                    };
            return query;
        }

        public void Robustly(Action action)
        {
            robustWrapper.Robustly(action);
        }

        public TResult Robustly<TResult>(Func<TResult> function)
        {
            return robustWrapper.Robustly(function);
        }

        public T Query<T>(Func<T> query, T expecting)
        {
            return robustWrapper.Query(query, expecting);
        }

        public void TryUntil(Action tryThis, Func<bool> until, TimeSpan waitBeforeRetry)
        {
            robustWrapper.TryUntil(tryThis, until, waitBeforeRetry);
        }
    }
}