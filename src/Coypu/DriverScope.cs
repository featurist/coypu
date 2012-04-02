using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Coypu.Actions;
using Coypu.Finders;
using Coypu.Queries;
using Coypu.Robustness;

namespace Coypu
{
    public class DriverScope : Scope
    {
        protected readonly SessionConfiguration SessionConfiguration;
        private readonly ElementFinder elementFinder;
        protected Driver driver;
        protected RobustWrapper robustWrapper;
        protected readonly Waiter waiter;
        internal UrlBuilder urlBuilder;
        internal StateFinder stateFinder;
        private ElementFound element;
        private Options options;

        internal DriverScope(SessionConfiguration SessionConfiguration, ElementFinder elementFinder, Driver driver, RobustWrapper robustWrapper, Waiter waiter, UrlBuilder urlBuilder)
        {
            this.elementFinder = elementFinder ?? new DocumentElementFinder(driver);
            this.SessionConfiguration = SessionConfiguration;
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
            options = outer.SessionConfiguration;
            SessionConfiguration = outer.SessionConfiguration;
        }

        public virtual Uri Location
        {
            get { return driver.Location; }
        }

        public bool ConsiderInvisibleElements
        {
            get { return Default(options).ConsiderInvisibleElements; }
        }

        protected Options SetOptions(Options options)
        {
            return this.options = Default(options);
        }

        private Options Default(Options options)
        {
            return options ?? SessionConfiguration;
        }

        public void ClickButton(string locator, Options options = null)
        {
            RetryUntilTimeout(WaitThenClickButton(locator, SetOptions(options)));
        }

        public void ClickLink(string locator, Options options = null)
        {
            RetryUntilTimeout(WaitThenClickLink(locator, SetOptions(options)));
        }

        private WaitThenClick WaitThenClickLink(string locator, Options options = null)
        {
            return new WaitThenClick(driver, SetOptions(options), waiter, new LinkFinder(driver, locator, this));
        }

        private WaitThenClick WaitThenClickButton(string locator, Options options = null)
        {
            return new WaitThenClick(driver, SetOptions(options), waiter, new ButtonFinder(driver, locator, this));
        }

        public DriverScope ClickButton(string locator, PredicateQuery until, TimeSpan waitBeforeRetry, Options options = null)
        {
            options = SetOptions(options);
            TryUntil(WaitThenClickButton(locator, options), until, waitBeforeRetry, options);
            return this;
        }

        public DriverScope ClickLink(string locator, PredicateQuery until, TimeSpan waitBeforeRetry, Options options = null)
        {
            options = SetOptions(options);
            TryUntil(WaitThenClickLink(locator, options), until, waitBeforeRetry, options);
            return this;
        }

        public ElementScope FindButton(string locator, Options options = null)
        {
            return new RobustElementScope(new ButtonFinder(driver, locator, this), this, SetOptions(options));
        }

        public ElementScope FindLink(string locator, Options options = null)
        {
            return new RobustElementScope(new LinkFinder(driver, locator, this), this, SetOptions(options));
        }

        public ElementScope FindField(string locator, Options options = null)
        {
            return new RobustElementScope(new FieldFinder(driver, locator, this), this, SetOptions(options));
        }

        public FillInWith FillIn(string locator, Options options = null)
        {
            return new FillInWith(locator, driver, robustWrapper, this, SetOptions(options));
        }

        public SelectFrom Select(string option, Options options = null)
        {
            return new SelectFrom(option, driver, robustWrapper, this, SetOptions(options));
        }

        public bool HasContent(string text, Options options = null)
        {
            return Query(new HasContentQuery(driver, this, text, SetOptions(options)));
        }

        public bool HasContentMatch(Regex pattern, Options options = null)
        {
            return Query(new HasContentMatchQuery(driver, this, pattern, SetOptions(options)));
        }

        public bool HasNoContent(string text, Options options = null)
        {
            return Query(new HasNoContentQuery(driver, this, text, SetOptions(options)));
        }

        public bool HasNoContentMatch(Regex pattern, Options options = null)
        {
            return Query(new HasNoContentMatchQuery(driver, this, pattern, SetOptions(options)));
        }

        public bool HasCss(string cssSelector, Options options = null)
        {
            return Query(new HasCssQuery(driver, this, cssSelector, SetOptions(options)));
        }

        public bool HasNoCss(string cssSelector, Options options = null)
        {
            return Query(new HasNoCssQuery(driver, this, cssSelector, SetOptions(options)));
        }

        public bool HasXPath(string xpath, Options options = null)
        {
            return Query(new HasXPathQuery(driver, this, xpath, SetOptions(options)));
        }

        public bool HasNoXPath(string xpath, Options options = null)
        {
            return Query(new HasNoXPathQuery(driver, this, xpath, SetOptions(options)));
        }

        public ElementScope FindCss(string cssSelector, Options options = null)
        {
            return new RobustElementScope(new CssFinder(driver, cssSelector, this), this, SetOptions(options));
        }

        public ElementScope FindXPath(string xpath, Options options = null)
        {
            return new RobustElementScope(new XPathFinder(driver, xpath, this), this, SetOptions(options));
        }

        public IEnumerable<ElementFound> FindAllCss(string cssSelector, Options options = null)
        {
            SetOptions(options);
            return driver.FindAllCss(cssSelector, this);
        }

        public IEnumerable<ElementFound> FindAllXPath(string xpath, Options options = null)
        {
            SetOptions(options);
            return driver.FindAllXPath(xpath, this);
        }

        public ElementScope FindSection(string locator, Options options = null)
        {
            return new RobustElementScope(new SectionFinder(driver, locator, this), this, SetOptions(options));
        }

        public ElementScope FindFieldset(string locator, Options options = null)
        {
            return new RobustElementScope(new FieldsetFinder(driver, locator, this), this, SetOptions(options));
        }

        public ElementScope FindId(string id, Options options = null)
        {
            return new RobustElementScope(new IdFinder(driver, id, this), this, SetOptions(options));
        }

        public void Check(string locator, Options options = null)
        {
            RetryUntilTimeout(new Check(driver, this, locator, SetOptions(options)));
        }

        public void Uncheck(string locator, Options options = null)
        {
            RetryUntilTimeout(new Uncheck(driver, this, locator, SetOptions(options)));
        }

        public void Choose(string locator, Options options = null)
        {
            RetryUntilTimeout(new Choose(driver, this, locator, SetOptions(options)));
        }

        public string ExecuteScript(string javascript)
        {
            return driver.ExecuteScript(javascript,this);
        }

        public DriverScope Hover(Options options = null)
        {
            RetryUntilTimeout(new Hover(this, driver, SetOptions(options)));
            return this;
        }

        public bool Has(ElementScope findElement)
        {
            return findElement.Exists();
        }

        public bool HasNo(ElementScope findElement)
        {
            return findElement.Missing();
        }

        public void RetryUntilTimeout(Action action, Options options = null)
        {
            robustWrapper.Robustly(new LambdaBrowserAction(action,SetOptions(options)));
        }

        public TResult RetryUntilTimeout<TResult>(Func<TResult> function, Options options = null)
        {
            return robustWrapper.Robustly(new LambdaQuery<TResult>(function,SetOptions(options)));
        }

        public void RetryUntilTimeout(BrowserAction action)
        {
            Query(action);
        }

        public IFrameElementScope FindIFrame(string locator, Options options = null)
        {
            return new IFrameElementScope(new IFrameFinder(driver, locator, this), this, SetOptions(options));
        }

        public T Query<T>(Func<T> query, T expecting, Options options = null)
        {
            return robustWrapper.Robustly(new LambdaQuery<T>(query, expecting, SetOptions(options)));
        }

        public T Query<T>(Query<T> query)
        {
            return robustWrapper.Robustly(query);
        }

        public void TryUntil(Action tryThis, Func<bool> until, TimeSpan waitBeforeRetry, Options options = null)
        {
            options = SetOptions(options);
            robustWrapper.TryUntil(new LambdaBrowserAction(tryThis, options), new LambdaPredicateQuery(until,options), options.Timeout, waitBeforeRetry);
        }

        public void TryUntil(BrowserAction tryThis, PredicateQuery until, TimeSpan waitBeforeRetry, Options options = null)
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

        /// <summary>
        /// Try and find this scope now
        /// </summary>
        /// <returns></returns>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        public virtual ElementFound Now()
        {
            return FindElement();
        }

        protected internal ElementFound FindElement()
        {
            if (element == null || element.Stale)
                element = elementFinder.Find();
            return element;
        }
    }
}