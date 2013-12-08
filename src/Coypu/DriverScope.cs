using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Coypu.Actions;
using Coypu.Drivers;
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

        internal DriverScope(ElementFinder elementFinder, DriverScope outerScope)
        {
            this.elementFinder = elementFinder;
            driver = outerScope.driver;
            robustWrapper = outerScope.robustWrapper;
            urlBuilder = outerScope.urlBuilder;
            stateFinder = outerScope.stateFinder;
            waiter = outerScope.waiter;
            SessionConfiguration = outerScope.SessionConfiguration;
        }

        public virtual Uri Location
        {
            get { return driver.Location(this); }
        }

        public string Text
        {
            get { return Now().Text; }
        }

        public Browser Browser
        {
            get { return SessionConfiguration.Browser; }
        }

        public Options Merge(Options options)
        {
            return Options.Merge(options, SessionConfiguration);
        }

        public void ClickButton(string locator, Options options = null)
        {
            RetryUntilTimeout(WaitThenClickButton(locator, Merge(options)));
        }

        public void ClickLink(string locator, Options options = null)
        {
            RetryUntilTimeout(WaitThenClickLink(locator, Merge(options)));
        }

        private WaitThenClick WaitThenClickLink(string locator, Options options = null)
        {
            return new WaitThenClick(driver, Merge(options), waiter, new LinkFinder(driver, locator, this, Merge(options)));
        }

        private WaitThenClick WaitThenClickButton(string locator, Options options = null)
        {
            return new WaitThenClick(driver, Merge(options), waiter, new ButtonFinder(driver, locator, this, Merge(options)));
        }

        public Scope ClickButton(string locator, PredicateQuery until, TimeSpan waitBeforeRetry, Options options = null)
        {
            TryUntil(WaitThenClickButton(locator, Merge(options)), until, waitBeforeRetry, Merge(options));
            return this;
        }

        public Scope ClickLink(string locator, PredicateQuery until, TimeSpan waitBeforeRetry, Options options = null)
        {
            TryUntil(WaitThenClickLink(locator, Merge(options)), until, waitBeforeRetry, Merge(options));
            return this;
        }

        public ElementScope FindButton(string locator, Options options = null)
        {
            return new RobustElementScope(new ButtonFinder(driver, locator, this, Merge(options)), this, Merge(options));
        }

        public ElementScope FindLink(string locator, Options options = null)
        {
            return new RobustElementScope(new LinkFinder(driver, locator, this, Merge(options)), this, Merge(options));
        }

        public ElementScope FindField(string locator, Options options = null)
        {
            return new RobustElementScope(new FieldFinder(driver, locator, this, Merge(options)), this, Merge(options));
        }

        public FillInWith FillIn(string locator, Options options = null) 
        {
            return new FillInWith(FindField(locator, Merge(options)), driver, robustWrapper, Merge(options));
        }

        public SelectFrom Select(string option, Options options = null)
        {
            return new SelectFrom(option, driver, robustWrapper, this, Merge(options));
        }

        public bool HasContent(string text, Options options = null)
        {
            return Query(new HasContentQuery(this, text, Merge(options)));
        }

        public bool HasContentMatch(Regex pattern, Options options = null)
        {
            return Query(new HasContentMatchQuery(this, pattern, Merge(options)));
        }

        public bool HasNoContent(string text, Options options = null)
        {
            return Query(new HasNoContentQuery(this, text, Merge(options)));
        }

        public bool HasNoContentMatch(Regex pattern, Options options = null)
        {
            return Query(new HasNoContentMatchQuery(this, pattern, Merge(options)));
        }

        public bool HasCss(string cssSelector, Options options = null)
        {
            return FindCss(cssSelector, Merge(options)).Exists();
        }

        public bool HasCss(string cssSelector, string text, Options options = null)
        {
            return FindCss(cssSelector, text, Merge(options)).Exists();
        }

        public bool HasCss(string cssSelector, Regex text, Options options = null)
        {
            return FindCss(cssSelector, text, Merge(options)).Exists();
        }

        public bool HasNoCss(string cssSelector, Options options = null)
        {
            return FindCss(cssSelector, Merge(options)).Missing();
        }

        public bool HasNoCss(string cssSelector, string text, Options options = null)
        {
            return FindCss(cssSelector, text, Merge(options)).Missing();
        }

        public bool HasNoCss(string cssSelector, Regex text, Options options = null)
        {
            return FindCss(cssSelector, text, Merge(options)).Missing();
        }

        public bool HasXPath(string xpath, Options options = null)
        {
            return FindXPath(xpath, Merge(options)).Exists();
        }

        public bool HasNoXPath(string xpath, Options options = null)
        {
            return FindXPath(xpath, Merge(options)).Missing();
        }

        public ElementScope FindCss(string cssSelector, Options options = null)
        {
            return new RobustElementScope(new CssFinder(driver, cssSelector, this, Merge(options)), this, Merge(options));
        }

        public ElementScope FindCss(string cssSelector, string text, Options options = null)
        {
            return new RobustElementScope(new CssFinder(driver, cssSelector, this, Merge(options), text), this, Merge(options));
        }

        public ElementScope FindCss(string cssSelector, Regex text, Options options = null)
        {
            return new RobustElementScope(new CssFinder(driver, cssSelector, this, Merge(options), text), this, Merge(options));
        }

        public ElementScope FindXPath(string xpath, Options options = null)
        {
            return new RobustElementScope(new XPathFinder(driver, xpath, this, Merge(options)), this, Merge(options));
        }

        public IEnumerable<SnapshotElementScope> FindAllCss(string cssSelector, Func<IEnumerable<SnapshotElementScope>, bool> predicate = null, Options options = null)
        {
            if (predicate != null)
                return Query(new FindAllCssWithPredicateQuery(cssSelector, predicate, this, Merge(options)));

            return FindAllCssNoPredicate(cssSelector, Merge(options));
        }

        internal IEnumerable<SnapshotElementScope> FindAllCssNoPredicate(string cssSelector, Options options)
        {
            return driver.FindAllCss(cssSelector, this, options).AsSnapshotElementScopes(this, options);
        }

        public IEnumerable<SnapshotElementScope> FindAllXPath(string xpath, Func<IEnumerable<SnapshotElementScope>, bool> predicate = null, Options options = null)
        {
            if (predicate != null)
                return Query(new FindAllXPathWithPredicateQuery(xpath, predicate, this, Merge(options)));

            return FindAllXPathNoPredicate(xpath, Merge(options));
        }

        internal IEnumerable<SnapshotElementScope> FindAllXPathNoPredicate(string xpath, Options options)
        {
            return driver.FindAllXPath(xpath, this, options).AsSnapshotElementScopes(this, options);
        }

        public ElementScope FindSection(string locator, Options options = null)
        {
            return new RobustElementScope(new SectionFinder(driver, locator, this, Merge(options)), this, Merge(options));
        }

        public ElementScope FindFieldset(string locator, Options options = null)
        {
            return new RobustElementScope(new FieldsetFinder(driver, locator, this, Merge(options)), this, Merge(options));
        }

        public ElementScope FindId(string id, Options options = null)
        {
            return new RobustElementScope(new IdFinder(driver, id, this, Merge(options)), this, Merge(options));
        }

        public void Check(string locator, Options options = null)
        {
            var mergedOptions = Merge(options);
            RetryUntilTimeout(new Check(driver, FindField(locator, Merge(options)), Merge(options)));
        }

        public void Uncheck(string locator, Options options = null)
        {
            RetryUntilTimeout(new Uncheck(driver, FindField(locator, Merge(options)), Merge(options)));
        }

        public void Uncheck(ElementScope checkbox, Options options = null)
        {
            RetryUntilTimeout(new Uncheck(driver, checkbox, Merge(options)));
        }

        public void Choose(string locator, Options options = null)
        {
            var mergedOptions = Merge(options);
            RetryUntilTimeout(new Choose(driver, FindField(locator, Merge(options)), Merge(options)));
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
            robustWrapper.Robustly(new LambdaBrowserAction(action, Merge(options)));
        }

        public TResult RetryUntilTimeout<TResult>(Func<TResult> function, Options options = null)
        {
            return robustWrapper.Robustly(new LambdaQuery<TResult>(function, Merge(options)));
        }

        public void RetryUntilTimeout(BrowserAction action)
        {
            Query(action);
        }

        public ElementScope FindFrame(string locator, Options options = null)
        {
            return new RobustElementScope(new FrameFinder(driver, locator, this, Merge(options)), this, Merge(options));
        }

        public T Query<T>(Func<T> query, T expecting, Options options = null)
        {
            return robustWrapper.Robustly(new LambdaQuery<T>(query, expecting, Merge(options)));
        }

        public T Query<T>(Query<T> query)
        {
            return robustWrapper.Robustly(query);
        }

        public void TryUntil(Action tryThis, Func<bool> until, TimeSpan waitBeforeRetry, Options options = null)
        {
            var mergedOptions = Merge(options);
            robustWrapper.TryUntil(new LambdaBrowserAction(tryThis, Merge(options)), new LambdaPredicateQuery(until, mergedOptions), mergedOptions.Timeout, waitBeforeRetry);
        }

        public void TryUntil(BrowserAction tryThis, PredicateQuery until, TimeSpan waitBeforeRetry, Options options = null)
        {
            robustWrapper.TryUntil(tryThis, until, Merge(options).Timeout, waitBeforeRetry);
        }

        public State FindState(State[] states, Options options = null)
        {
            return stateFinder.FindState(states, this, Merge(options));
        }

        public State FindState(params State[] states)
        {
            return FindState(states, null);
        }

        /// <summary>
        /// Try and find this scope now
        /// </summary>
        /// <returns></returns>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        /// <exception cref="T:Coypu.AmbiguousHtmlException">Thrown if the there is more than one matching element and the Match.Single option is set</exception>
        public virtual ElementFound Now()
        {
            return FindElement();
        }

        protected internal ElementFound FindElement()
        {
            if (element == null || element.Stale(elementFinder.Options))
                element = elementFinder.ResolveQuery();
            return element;
        }
    }
}