using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Coypu.Actions;
using Coypu.Drivers;
using Coypu.Finders;
using Coypu.Queries;
using Coypu.Timing;

namespace Coypu
{
    public class DriverScope : Scope
    {
        protected readonly SessionConfiguration SessionConfiguration;
        internal readonly ElementFinder elementFinder;

        protected Driver driver;
        protected TimingStrategy timingStrategy;
        protected readonly Waiter waiter;
        internal UrlBuilder urlBuilder;
        internal StateFinder stateFinder;
        private ElementFound element;

        internal DriverScope(SessionConfiguration SessionConfiguration, ElementFinder elementFinder, Driver driver, TimingStrategy timingStrategy, Waiter waiter, UrlBuilder urlBuilder)
        {
            this.elementFinder = elementFinder ?? new DocumentElementFinder(driver);
            this.SessionConfiguration = SessionConfiguration;
            this.driver = driver;
            this.timingStrategy = timingStrategy;
            this.waiter = waiter;
            this.urlBuilder = urlBuilder;
            stateFinder = new StateFinder(timingStrategy);
        }

        internal DriverScope(ElementFinder elementFinder, DriverScope outerScope)
        {
            this.elementFinder = elementFinder;
            driver = outerScope.driver;
            timingStrategy = outerScope.timingStrategy;
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

        public ElementFinder ElementFinder
        {
            get { return elementFinder; }
        }

        internal Options MergeWithSession(Options options)
        {
            return Options.Merge(options, SessionConfiguration);
        }

        internal Options MergeWithFinder(Options options)
        {
            return Options.Merge(options, ElementFinder.Options);
        }

        public void ClickButton(string locator, Options options = null)
        {
            RetryUntilTimeout(WaitThenClickButton(locator, MergeWithSession(options)));
        }

        public void ClickLink(string locator, Options options = null)
        {
            RetryUntilTimeout(WaitThenClickLink(locator, MergeWithSession(options)));
        }

        private WaitThenClick WaitThenClickLink(string locator, Options options = null)
        {
            return new WaitThenClick(driver, MergeWithSession(options), waiter, new LinkFinder(driver, locator, this, MergeWithSession(options)));
        }

        private WaitThenClick WaitThenClickButton(string locator, Options options = null)
        {
            return new WaitThenClick(driver, MergeWithSession(options), waiter, new ButtonFinder(driver, locator, this, MergeWithSession(options)));
        }

        public Scope ClickButton(string locator, PredicateQuery until, TimeSpan waitBeforeRetry, Options options = null)
        {
            TryUntil(WaitThenClickButton(locator, MergeWithSession(options)), until, waitBeforeRetry, MergeWithSession(options));
            return this;
        }

        public Scope ClickLink(string locator, PredicateQuery until, TimeSpan waitBeforeRetry, Options options = null)
        {
            TryUntil(WaitThenClickLink(locator, MergeWithSession(options)), until, waitBeforeRetry, MergeWithSession(options));
            return this;
        }

        public ElementScope FindButton(string locator, Options options = null)
        {
            return new RobustElementScope(new ButtonFinder(driver, locator, this, MergeWithSession(options)), this, MergeWithSession(options));
        }

        public ElementScope FindLink(string locator, Options options = null)
        {
            return new RobustElementScope(new LinkFinder(driver, locator, this, MergeWithSession(options)), this, MergeWithSession(options));
        }

        public ElementScope FindField(string locator, Options options = null)
        {
            return new RobustElementScope(new FieldFinder(driver, locator, this, MergeWithSession(options)), this, MergeWithSession(options));
        }

        public FillInWith FillIn(string locator, Options options = null) 
        {
            return new FillInWith(FindField(locator, MergeWithSession(options)), driver, timingStrategy, MergeWithSession(options));
        }

        public SelectFrom Select(string option, Options options = null)
        {
            return new SelectFrom(option, driver, timingStrategy, this, MergeWithSession(options));
        }

        public bool HasContent(string text, Options options = null)
        {
            return Query(new HasContentQuery(this, text, MergeWithFinder(options)));
        }

        public bool HasContentMatch(Regex pattern, Options options = null)
        {
            return Query(new HasContentMatchQuery(this, pattern, MergeWithFinder(options)));
        }

        public bool HasNoContent(string text, Options options = null)
        {
            return Query(new HasNoContentQuery(this, text, MergeWithFinder(options)));
        }

        public bool HasNoContentMatch(Regex pattern, Options options = null)
        {
            return Query(new HasNoContentMatchQuery(this, pattern, MergeWithFinder(options)));
        }

        public ElementScope FindCss(string cssSelector, Options options = null)
        {
            return new RobustElementScope(new CssFinder(driver, cssSelector, this, MergeWithSession(options)), this, MergeWithSession(options));
        }

        public ElementScope FindCss(string cssSelector, string text, Options options = null)
        {
            return new RobustElementScope(new CssFinder(driver, cssSelector, this, MergeWithSession(options), text), this, MergeWithSession(options));
        }

        public ElementScope FindCss(string cssSelector, Regex text, Options options = null)
        {
            return new RobustElementScope(new CssFinder(driver, cssSelector, this, MergeWithSession(options), text), this, MergeWithSession(options));
        }

        public ElementScope FindXPath(string xpath, Options options = null)
        {
            return new RobustElementScope(new XPathFinder(driver, xpath, this, MergeWithSession(options)), this, MergeWithSession(options));
        }

        public IEnumerable<SnapshotElementScope> FindAllCss(string cssSelector, Func<IEnumerable<SnapshotElementScope>, bool> predicate = null, Options options = null)
        {
            if (predicate != null)
                return Query(new FindAllCssWithPredicateQuery(cssSelector, predicate, this, MergeWithSession(options)));

            return FindAllCssNoPredicate(cssSelector, MergeWithSession(options));
        }

        internal IEnumerable<SnapshotElementScope> FindAllCssNoPredicate(string cssSelector, Options options)
        {
            return driver.FindAllCss(cssSelector, this, options).AsSnapshotElementScopes(this, options);
        }

        public IEnumerable<SnapshotElementScope> FindAllXPath(string xpath, Func<IEnumerable<SnapshotElementScope>, bool> predicate = null, Options options = null)
        {
            if (predicate != null)
                return Query(new FindAllXPathWithPredicateQuery(xpath, predicate, this, MergeWithSession(options)));

            return FindAllXPathNoPredicate(xpath, MergeWithSession(options));
        }

        internal IEnumerable<SnapshotElementScope> FindAllXPathNoPredicate(string xpath, Options options)
        {
            return driver.FindAllXPath(xpath, this, options).AsSnapshotElementScopes(this, options);
        }

        public ElementScope FindSection(string locator, Options options = null)
        {
            return new RobustElementScope(new SectionFinder(driver, locator, this, MergeWithSession(options)), this, MergeWithSession(options));
        }

        public ElementScope FindFieldset(string locator, Options options = null)
        {
            return new RobustElementScope(new FieldsetFinder(driver, locator, this, MergeWithSession(options)), this, MergeWithSession(options));
        }

        public ElementScope FindId(string id, Options options = null)
        {
            return new RobustElementScope(new IdFinder(driver, id, this, MergeWithSession(options)), this, MergeWithSession(options));
        }

        public void Check(string locator, Options options = null)
        {
            var mergedOptions = MergeWithSession(options);
            RetryUntilTimeout(new Check(driver, FindField(locator, MergeWithSession(options)), MergeWithSession(options)));
        }

        public void Uncheck(string locator, Options options = null)
        {
            RetryUntilTimeout(new Uncheck(driver, FindField(locator, MergeWithSession(options)), MergeWithSession(options)));
        }

        public void Uncheck(ElementScope checkbox, Options options = null)
        {
            RetryUntilTimeout(new Uncheck(driver, checkbox, MergeWithSession(options)));
        }

        public void Choose(string locator, Options options = null)
        {
            var mergedOptions = MergeWithSession(options);
            RetryUntilTimeout(new Choose(driver, FindField(locator, MergeWithSession(options)), MergeWithSession(options)));
        }

        public bool HasNo(ElementScope findElement)
        {
            return findElement.Missing();
        }

        public void RetryUntilTimeout(Action action, Options options = null)
        {
            timingStrategy.Synchronise(new LambdaBrowserAction(action, MergeWithSession(options)));
        }

        public TResult RetryUntilTimeout<TResult>(Func<TResult> function, Options options = null)
        {
            return timingStrategy.Synchronise(new LambdaQuery<TResult>(function, MergeWithSession(options)));
        }

        public void RetryUntilTimeout(BrowserAction action)
        {
            Query(action);
        }

        public ElementScope FindFrame(string locator, Options options = null)
        {
            return new RobustElementScope(new FrameFinder(driver, locator, this, MergeWithSession(options)), this, MergeWithSession(options));
        }

        public T Query<T>(Func<T> query, T expecting, Options options = null)
        {
            return timingStrategy.Synchronise(new LambdaQuery<T>(query, expecting, MergeWithSession(options)));
        }

        public T Query<T>(Query<T> query)
        {
            return timingStrategy.Synchronise(query);
        }

        public void TryUntil(Action tryThis, Func<bool> until, TimeSpan waitBeforeRetry, Options options = null)
        {
            var mergedOptions = MergeWithSession(options);
            timingStrategy.TryUntil(new LambdaBrowserAction(tryThis, MergeWithSession(options)), new LambdaPredicateQuery(until, mergedOptions), mergedOptions.Timeout, waitBeforeRetry);
        }

        public void TryUntil(BrowserAction tryThis, PredicateQuery until, TimeSpan waitBeforeRetry, Options options = null)
        {
            timingStrategy.TryUntil(tryThis, until, MergeWithSession(options).Timeout, waitBeforeRetry);
        }

        public State FindState(State[] states, Options options = null)
        {
            return stateFinder.FindState(states, this, MergeWithSession(options));
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
            if (element == null || element.Stale(ElementFinder.Options))
                element = ElementFinder.ResolveQuery();
            return element;
        }
    }
}