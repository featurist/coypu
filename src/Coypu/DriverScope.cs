using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Coypu.Actions;
using Coypu.Finders;
using Coypu.Queries;
using Coypu.Robustness;

namespace Coypu
{
    public class DriverScope : Scope<DriverScope>
    {
        private readonly Configuration configuration;
        private readonly ElementFinder elementFinder;
        protected Driver driver;
        internal RobustWrapper robustWrapper;
        private readonly Waiter waiter;
        internal UrlBuilder urlBuilder;
        internal StateFinder stateFinder;
        private ElementFound element;
        private Options options;

        internal DriverScope(Configuration configuration, ElementFinder elementFinder, Driver driver, RobustWrapper robustWrapper, Waiter waiter, UrlBuilder urlBuilder)
        {
            this.configuration = configuration;
            this.elementFinder = elementFinder;
            this.driver = driver;
            this.robustWrapper = robustWrapper;
            this.waiter = waiter;
            this.urlBuilder = urlBuilder;
            stateFinder = new StateFinder(robustWrapper);
        }

        internal DriverScope(ElementFinder elementFinder, DriverScope outer)
        {
            this.elementFinder = elementFinder;
            driver = outer.driver;
            robustWrapper = outer.robustWrapper;
            urlBuilder = outer.urlBuilder;
            stateFinder = outer.stateFinder;
            waiter = outer.waiter;
            options = outer.configuration;
            configuration = outer.configuration;
        }

        public Uri Location
        {
            get { return driver.Location; }
        }

        public bool ConsiderInvisibleElements
        {
            get { return Default(options).ConsiderInvisibleElements; }
        }

        private Options SetOptions(Options options)
        {
            return this.options = Default(options);
        }

        internal Options Default(Options options)
        {
            return options ?? configuration;
        }

        /// <summary>
        /// Click a button, input of type button|submit|image or div with the css class "button"
        /// </summary>
        /// <param name="locator">The text/value, name or id of the button</param>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        public DriverScope ClickButton(string locator, Options options = null)
        {
            RetryUntilTimeout(WaitThenClickButton(locator, SetOptions(options)));
            return this;
        }

        /// <summary>
        /// Click the first matching link
        /// </summary>
        /// <param name="locator">The text of the link</param>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        public DriverScope ClickLink(string locator, Options options = null)
        {
            RetryUntilTimeout(WaitThenClickLink(locator, SetOptions(options)));
            return this;
        }

        private WaitThenClick WaitThenClickLink(string locator, Options options)
        {
            return new WaitThenClick(driver, SetOptions(options), waiter, new LinkFinder(driver, locator, this));
        }

        private WaitThenClick WaitThenClickButton(string locator, Options options)
        {
            return new WaitThenClick(driver, SetOptions(options), waiter, new ButtonFinder(driver, locator, this));
        }

        /// <summary>
        /// Click a previously found element
        /// </summary>
        /// <param name="element">The element to click</param>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        public DriverScope Click(Options options = null)
        {
            RetryUntilTimeout(new Click(this,driver, SetOptions(options)));
            return this;
        }

        /// <summary>
        /// <para>Click a button, input of type button|submit|image or div with the css class "button".</para>
        /// <para>Wait for a condition to be satisfied for a specified time otherwise click and wait again.</para>
        /// <para>Continues until the expected condition is satisfied or the <see cref="Configuration.Timeout"/> is reached.</para>
        /// </summary>
        /// <param name="locator">The text/value, name or id of the button</param>
        /// <param name="until">The condition to be satisfied</param>
        /// <returns>The first matching button</returns>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        public DriverScope ClickButton(string locator, Query<bool> until, TimeSpan waitBeforeRetry, Options options = null)
        {
            options = SetOptions(options);
            TryUntil(WaitThenClickButton(locator, options), until, waitBeforeRetry, options);
            return this;
        }

        /// <summary>
        /// <para>Click a link and wait for a condition to be satisfied for a specified time otherwise click and wait again.</para> 
        /// <para>Continues until the expected condition is satisfied or the <see cref="Configuration.Timeout"/> is reached.</para>
        /// </summary>
        /// <param name="locator">The text of the link</param>
        /// <param name="until">The condition to be satisfied</param>
        /// <returns>The first matching button</returns>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        public DriverScope ClickLink(string locator, Query<bool> until, TimeSpan waitBeforeRetry, Options options = null)
        {
            options = SetOptions(options);
            TryUntil(WaitThenClickLink(locator, options), until, waitBeforeRetry, options);
            return this;
        }

        /// <summary>
        /// Find the first input of type button|submit|image or div with the css class "button" to appear within the <see cref="Configuration.Timeout"/> .
        /// </summary>
        /// <param name="locator">The text/value, name or id of the button</param>
        /// <returns>A button</returns>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        public ElementScope FindButton(string locator, Options options = null)
        {
            return new RobustElementScope(new ButtonFinder(driver, locator, this), this, robustWrapper, SetOptions(options));
        }

        /// <summary>
        /// Find the first matching link to appear within the <see cref="Configuration.Timeout"/>
        /// </summary>
        /// <param name="locator">The text of the link</param>
        /// <returns>A link</returns>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        public ElementScope FindLink(string locator, Options options = null)
        {
            return new RobustElementScope(new LinkFinder(driver, locator, this), this, robustWrapper, SetOptions(options));
        }

        /// <summary>
        /// Find the first form field of any type to appear within the <see cref="Configuration.Timeout"/>
        /// </summary>
        /// <param name="locator">The text of the associated label element, the id or name, the placeholder text, the value of a radio button, the last part of the id (for asp.net forms testing)</param>
        /// <returns>A form field</returns>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        public ElementScope FindField(string locator, Options options = null)
        {
            return new RobustElementScope(new FieldFinder(driver, locator, this), this, robustWrapper, SetOptions(options));
        }

        /// <summary>
        /// Find the first matching text field to appear within the <see cref="Configuration.Timeout"/> to fill in.
        /// </summary>
        /// <param name="locator">The text of the associated label element, the id or name, the placeholder text, the last part of the id (for asp.net forms testing)</param>
        /// <returns>With</returns>
        public FillInWith FillIn(string locator, Options options = null)
        {
            return new FillInWith(locator, driver, robustWrapper, this, SetOptions(options));
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

        /// <summary>
        /// Select an option from a select element
        /// </summary>
        /// <param name="option">The text or value of the option to select</param>
        /// <returns>From</returns>
        public SelectFrom Select(string option, Options options = null)
        {
            return new SelectFrom(option, driver, robustWrapper, this, SetOptions(options));
        }

        /// <summary>
        /// Query whether text appears on the page within the <see cref="Configuration.Timeout"/>
        /// </summary>
        /// <param name="text">The exact text to find</param>
        /// <returns>Whether the text appears</returns>
        public bool HasContent(string text, Options options = null)
        {
            return Query(new HasContentQuery(driver, this, text, SetOptions(options)));
        }

        /// <summary>
        /// Query whether text appears on the page using a regular expression within the <see cref="Configuration.Timeout"/>
        /// </summary>
        /// <param name="pattern">The regular expression to match</param>
        /// <returns>Whether the page text matches</returns>
        public bool HasContentMatch(Regex pattern, Options options = null)
        {
            return Query(new HasContentMatchQuery(driver, this, pattern, SetOptions(options)));
        }

        /// <summary>
        /// Query whether text does not appear on the page. Returns as soon as the text does not appear, or when the <see cref="Configuration.Timeout"/> is reached.
        /// </summary>
        /// <param name="text">The exact text expected not to be found</param>
        /// <returns>Whether the text does not appear</returns>
        public bool HasNoContent(string text, Options options = null)
        {
            return Query(new HasNoContentQuery(driver, this, text, SetOptions(options)));
        }

        /// <summary>
        /// Query whether text does not appear on the page using a regular expression. Returns as soon as the text does not appear, or when the <see cref="Configuration.Timeout"/> is reached.
        /// </summary>
        /// <param name="pattern">The regular expression expected not to match</param>
        /// <returns>Whether the text does not appear</returns>
        public bool HasNoContentMatch(Regex pattern, Options options = null)
        {
            return Query(new HasNoContentMatchQuery(driver, this, pattern, SetOptions(options)));
        }

        /// <summary>
        /// Query whether an element matching a CSS selector appears on the page within the <see cref="Configuration.Timeout"/>
        /// </summary>
        /// <param name="cssSelector">CSS selector</param>
        /// <returns>Whether an element appears</returns>
        public bool HasCss(string cssSelector, Options options = null)
        {
            return Query(new HasCssQuery(driver, this, cssSelector, SetOptions(options)));
        }

        /// <summary>
        /// Query whether an element matching a CSS selector does not appear on the page. Returns as soon as the element does not appear, or when the <see cref="Configuration.Timeout"/> is reached.
        /// </summary>
        /// <param name="cssSelector">CSS selector</param>
        /// <returns>Whether an element does not appear</returns>
        public bool HasNoCss(string cssSelector, Options options = null)
        {
            return Query(new HasNoCssQuery(driver, this, cssSelector, SetOptions(options)));
        }

        /// <summary>
        /// Query whether an element matching an XPath query appears on the page within the <see cref="Configuration.Timeout"/>
        /// </summary>
        /// <param name="xpath">XPath query</param>
        /// <returns>Whether an element appears</returns>
        public bool HasXPath(string xpath, Options options = null)
        {
            return Query(new HasXPathQuery(driver, this, xpath, SetOptions(options)));
        }

        /// <summary>
        /// Query whether an element matching an XPath query appears on the page. Returns as soon as the element appears, or when the <see cref="Configuration.Timeout"/> is reached.
        /// </summary>
        /// <param name="xpath">XPath query</param>
        /// <returns>Whether an element appears</returns>
        public bool HasNoXPath(string xpath, Options options = null)
        {
            return Query(new HasNoXPathQuery(driver, this, xpath, SetOptions(options)));
        }

        /// <summary>
        /// Find an element matching a CSS selector
        /// </summary>
        /// <param name="cssSelector">CSS selector</param>
        /// <returns>The first matchin element</returns>
        public ElementScope FindCss(string cssSelector, Options options = null)
        {
            return new RobustElementScope(new CssFinder(driver, cssSelector, this), this, robustWrapper, SetOptions(options));
        }

        /// <summary>
        /// Find an element matching an XPath query
        /// </summary>
        /// <param name="xpath">XPath query</param>
        /// <returns>The first matchin element</returns>
        public ElementScope FindXPath(string xpath, Options options = null)
        {
            return new RobustElementScope(new XPathFinder(driver, xpath, this), this, robustWrapper, SetOptions(options));
        }

        /// <summary>
        /// Find all elements matching a CSS selector at the current moment. Does not wait until the <see cref="Configuration.Timeout"/> but returns as soon as the driver does.
        /// </summary>
        /// <param name="cssSelector">CSS selector</param>
        /// <returns>All matching elements</returns>
        public IEnumerable<ElementFound> FindAllCss(string cssSelector, Options options = null)
        {
            SetOptions(options);
            return driver.FindAllCss(cssSelector, this);
        }

        /// <summary>
        /// Find all elements matching an XPath query at the current moment. Does not wait until the <see cref="Configuration.Timeout"/> but returns as soon as the driver does.
        /// </summary>
        /// <param name="xpath">XPath query</param>
        /// <returns>All matching elements</returns>
        public IEnumerable<ElementFound> FindAllXPath(string xpath, Options options = null)
        {
            SetOptions(options);
            return driver.FindAllXPath(xpath, this);
        }

        /// <summary>
        /// <para>Find the first matching section to appear within the <see cref="Configuration.Timeout"/></para>
        /// <para>Sections are identified by the text of their child heading element, or by id.</para>
        /// <para>E.g. to find this:
        /// 
        /// <code>    
        ///     &lt;div&gt;	
        ///         &lt;h2&gt;Search results&lt;/h2&gt;
        ///         ...
        ///     &lt;/div&gt;</code>
        ///
        /// or this:
        ///
        /// <code>
        ///     &lt;section&gt;
        ///         &lt;h3&gt;Search results&lt;/h3&gt;
        ///         ...
        ///     &lt;/section&gt;</code>
        /// </para>
        /// <para>use this:</para>
        /// <para>
        /// <code>    FindSection("Search results")</code>
        /// </para>
        /// </summary>
        /// <param name="locator">The text of a child heading element or section id</param>
        /// <returns>An element</returns>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        public ElementScope FindSection(string locator, Options options = null)
        {
            return new RobustElementScope(new SectionFinder(driver, locator, this), this, robustWrapper, SetOptions(options));
        }

        /// <summary>
        /// <para>Find the first matching fieldset to appear within the <see cref="Configuration.Timeout"/></para>
        /// <para>Fieldsets are identified by the text of their child legend element, or by id.</para>
        /// <para>E.g. to find this:
        /// 
        /// <code>    
        ///     &lt;fieldset&gt;	
        ///         &lt;legend&gt;Advanced search&lt;/legend&gt;
        ///         ...
        ///     &lt;/fieldset&gt;</code>
        /// </para>
        /// <para>use this:</para>
        /// <para>
        /// <code>    FindFieldset("Advanced search")</code>
        /// </para>
        /// </summary>
        /// <param name="locator">The text of a child legend element or fieldset id</param>
        /// <returns>An element</returns>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        public ElementScope FindFieldset(string locator, Options options = null)
        {
            return new RobustElementScope(new FieldsetFinder(driver, locator, this), this, robustWrapper, SetOptions(options));
        }

        /// <summary>
        /// Find the first matching element with specified id to appear within the <see cref="Configuration.Timeout"/>
        /// </summary>
        /// <param name="id">Element id</param>
        /// <returns>An elemenet</returns>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        public ElementScope FindId(string id, Options options = null)
        {
            return new RobustElementScope(new IdFinder(driver, id, this), this, robustWrapper, SetOptions(options));
        }

        /// <summary>
        /// Check the first checkbox to appear within the <see cref="Configuration.Timeout"/> matching the text of the associated label element, the id, name or the last part of the id (for asp.net forms testing).
        /// </summary>
        /// <param name="locator">The text of the associated label element, the id or name, the last part of the id (for asp.net forms testing)</param>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        public DriverScope Check(string locator, Options options = null)
        {
            RetryUntilTimeout(new Check(driver, this, locator, SetOptions(options)));
            return this;
        }

        /// <summary>
        /// Uncheck the first checkbox to appear within the <see cref="Configuration.Timeout"/> matching the text of the associated label element, the id, name or the last part of the id (for asp.net forms testing).
        /// </summary>
        /// <param name="locator">The text of the associated label element, the id or name, the last part of the id (for asp.net forms testing)</param>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        public DriverScope Uncheck(string locator, Options options = null)
        {
            RetryUntilTimeout(new Uncheck(driver, this, locator, SetOptions(options)));
            return this;
        }

        /// <summary>
        /// Choose the first radio button to appear within the <see cref="Configuration.Timeout"/> matching the text of the associated label element, the id, the name, the value or the last part of the id (for asp.net forms testing).
        /// </summary>
        /// <param name="locator">The text of the associated label element, the id or name, the last part of the id (for asp.net forms testing)</param>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        public DriverScope Choose(string locator, Options options = null)
        {
            RetryUntilTimeout(new Choose(driver, this, locator, SetOptions(options)));
            return this;
        }

        /// <summary>
        /// Executes custom javascript in the browser
        /// </summary>
        /// <param name="javascript">JavaScript to execute</param>
        /// <returns>Anything returned from the script</returns>
        public string ExecuteScript(string javascript)
        {
            return driver.ExecuteScript(javascript,this);
        }

        /// <summary>
        /// Hover the mouse over an element
        /// </summary>
        /// <param name="element"> </param>
        /// <param name="findElement">The element to hover over</param>
        public DriverScope Hover(Options options = null)
        {
            RetryUntilTimeout(new Hover(this, driver, SetOptions(options)));
            return this;
        }

        /// <summary>
        /// Query whether an element appears within the <see cref="Configuration.Timeout"/>
        /// </summary>
        /// <param name="findElement">A function to find an element</param>
        public bool Has(ElementScope findElement)
        {
            return findElement.Exists();
        }

        /// <summary>
        /// Query whether an element does not appear. Returns as soon as the element does not appear or after the <see cref="Configuration.Timeout"/>
        /// </summary>
        /// <param name="findElement">A function to find an element</param>
        public bool HasNo(ElementScope findElement)
        {
            return findElement.Missing();
        }

        /// <summary>
        /// <para>Retry an action on any exception until it succeeds. Once the <see cref="Configuration.Timeout"/> is passed any exception will be rethrown.</para>
        /// <para>Waits for the <see cref="Configuration.RetryInterval"/> between retries</para>
        /// </summary>
        /// <param name="action">An action</param>
        public void RetryUntilTimeout(Action action, Options options = null)
        {
            robustWrapper.Robustly(new LambdaDriverAction(action,SetOptions(options)));
        }

        /// <summary>
        /// <para>Retry a function on any exception until it succeeds. Once the <see cref="Configuration.Timeout"/> is passed any exception will be rethrown.</para>
        /// <para>Waits for the <see cref="Configuration.RetryInterval"/> between retries</para>
        /// </summary>
        /// <param name="function">A function</param>
        public TResult RetryUntilTimeout<TResult>(Func<TResult> function, Options options = null)
        {
            return robustWrapper.Robustly(new LambdaQuery<TResult>(function,SetOptions(options)));
        }

        /// <summary>
        /// <para>Retry an action on any exception until it succeeds. Once the <see cref="Configuration.Timeout"/> is passed any exception will be rethrown.</para>
        /// <para>Waits for the <see cref="Configuration.RetryInterval"/> between retries</para>
        /// </summary>
        /// <param name="function">A function</param>
        public void RetryUntilTimeout(DriverAction driverAction)
        {
            Query(driverAction);
        }

        public IFrameElementScope FindIFrame(string locator, Options options = null)
        {
            return new IFrameElementScope(new IFrameFinder(driver, locator, this), this, robustWrapper, SetOptions(options));
        }

        /// <summary>
        /// <para>Execute a query repeatedly until either the expected result is returned or the <see cref="Configuration.Timeout"/> is passed.</para>
        /// <para>Once the <see cref="Configuration.Timeout"/> is passed any result will be returned or any exception will be rethrown.</para>
        /// <para>Waits for the <see cref="Configuration.RetryInterval"/> between retries.</para>
        /// </summary>
        /// <param name="query">A query</param>
        /// <param name="expecting">Expected result</param>
        public T Query<T>(Func<T> query, T expecting, Options options = null)
        {
            return robustWrapper.Robustly(new LambdaQuery<T>(query, expecting, SetOptions(options)));
        }

        /// <summary>
        /// <para>Execute a query repeatedly until either the expected result is returned or the <see cref="Configuration.Timeout"/> is passed.</para>
        /// <para>Once the <see cref="Configuration.Timeout"/> is passed any result will be returned or any exception will be rethrown.</para>
        /// <para>Waits for the <see cref="Configuration.RetryInterval"/> between retries.</para>
        /// </summary>
        /// <param name="query">A query</param>
        public T Query<T>(Query<T> query)
        {
            return robustWrapper.Robustly(query);
        }

        /// <summary>
        /// <para>Execute an action repeatedly until a condition is met.</para>
        /// <para>Allows the time specified in <paramref name="waitBeforeRetry"/> for the <paramref name="until"/> condition to be met before each retry.</para>
        /// <para>Once the <see cref="Configuration.Timeout"/> is passed a Coypu.MissingHtmlException will be thrown.</para>
        /// </summary>
        /// <param name="tryThis">The action to try</param>
        /// <param name="until">The condition to be met</param>
        /// <param name="waitBeforeRetry">How long to wait for the condition</param>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the until condition is never met</exception>
        public void TryUntil(Action tryThis, Func<bool> until, TimeSpan waitBeforeRetry, Options options = null)
        {
            options = SetOptions(options);
            robustWrapper.TryUntil(new LambdaDriverAction(tryThis, options), new LambdaQuery<bool>(until), options.Timeout, waitBeforeRetry);
        }

        public void TryUntil(DriverAction tryThis, Query<bool> until, TimeSpan waitBeforeRetry, Options options = null)
        {
            robustWrapper.TryUntil(tryThis, until, SetOptions(options).Timeout, waitBeforeRetry);
        }

        public State FindState(State[] states, Options options = null)
        {
            return stateFinder.FindState(SetOptions(options), states);
        }

        public State FindState(params State[] states)
        {
            return stateFinder.FindState(SetOptions(options), states);
        }

        public ElementFound Now()
        {
            if (element == null || element.Stale)
                element = elementFinder.Find();

            return element;
        }
    }
}