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
        private readonly DriverScope outerScope;
        protected readonly DisambiguationStrategy DisambiguationStrategy = new FinderOptionsDisambiguationStrategy();

        internal DriverScope(SessionConfiguration sessionConfiguration, ElementFinder elementFinder, Driver driver, TimingStrategy timingStrategy, Waiter waiter, UrlBuilder urlBuilder, DisambiguationStrategy disambiguationStrategy)
        {
            this.elementFinder = elementFinder ?? new DocumentElementFinder(driver, sessionConfiguration);
            this.SessionConfiguration = sessionConfiguration;
            this.driver = driver;
            this.timingStrategy = timingStrategy;
            this.waiter = waiter;
            this.urlBuilder = urlBuilder;
            this.DisambiguationStrategy = disambiguationStrategy;
            stateFinder = new StateFinder(timingStrategy);
        }

        internal DriverScope(ElementFinder elementFinder, DriverScope outerScope)
        {
            this.elementFinder = elementFinder;
            this.outerScope = outerScope;
            driver = outerScope.driver;
            timingStrategy = outerScope.timingStrategy;
            urlBuilder = outerScope.urlBuilder;
            DisambiguationStrategy = outerScope.DisambiguationStrategy;
            stateFinder = outerScope.stateFinder;
            waiter = outerScope.waiter;
            SessionConfiguration = outerScope.SessionConfiguration;
        }

        internal DriverScope OuterScope
        {
            get { return outerScope; }
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

        internal Options Merge(Options options)
        {
            var mergeWith = ElementFinder != null ? ElementFinder.Options : SessionConfiguration;
            return Options.Merge(options, mergeWith);
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
            return new WaitThenClick(driver, Merge(options), waiter, new LinkFinder(driver, locator, this, Merge(options)), DisambiguationStrategy);
        }

        private WaitThenClick WaitThenClickButton(string locator, Options options = null)
        {
            return new WaitThenClick(driver, Merge(options), waiter, new ButtonFinder(driver, locator, this, Merge(options)), DisambiguationStrategy);
        }

        public Scope ClickButton(string locator, PredicateQuery until, Options options = null)
        {
            TryUntil(WaitThenClickButton(locator, Merge(options)), until, Merge(options));
            return this;
        }

        public Scope ClickLink(string locator, PredicateQuery until, Options options = null)
        {
            TryUntil(WaitThenClickLink(locator, Merge(options)), until, Merge(options));
            return this;
        }

        public ElementScope FindButton(string locator, Options options = null)
        {
            return new ButtonFinder(driver, locator, this, Merge(options)).AsScope();
        }

        public ElementScope FindLink(string locator, Options options = null)
        {
            return new LinkFinder(driver, locator, this, Merge(options)).AsScope();
        }

        public ElementScope FindField(string locator, Options options = null)
        {
            return new FieldFinder(driver, locator, this, Merge(options)).AsScope();
        }

        public FillInWith FillIn(string locator, Options options = null) 
        {
            return new FillInWith(FindField(locator, options), driver, timingStrategy, Merge(options));
        }

        public SelectFrom Select(string option, Options options = null)
        {
            return new SelectFrom(option, driver, timingStrategy, this, Merge(options), DisambiguationStrategy);
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

        public ElementScope FindCss(string cssSelector, Options options = null)
        {
            return new CssFinder(driver, cssSelector, this, Merge(options)).AsScope();
        }

        public ElementScope FindCss(string cssSelector, string text, Options options = null)
        {
            return new CssFinder(driver, cssSelector, this, Merge(options), text).AsScope();
        }

        public ElementScope FindCss(string cssSelector, Regex text, Options options = null)
        {
            return new CssFinder(driver, cssSelector, this, Merge(options), text).AsScope();
        }

        public ElementScope FindXPath(string xpath, Options options = null)
        {
            return new XPathFinder(driver, xpath, this, Merge(options)).AsScope();
        }

        [Obsolete("For assertions please use Assert.That(scope, Shows.Css(\".your-selector\",etc); instead for decent feedback. For the old behaviour of HasCss you can use FindCss(...).Exists();")]
        public bool HasCss(string cssSelector, string text, Options options = null)
        {
            return FindCss(cssSelector, text, options).Exists();
        }

        [Obsolete("For assertions please use Assert.That(scope, Shows.Css(\".your-selector\",etc); instead for decent feedback. For the old behaviour of HasCss you can use FindCss(...).Exists();")]
        public bool HasCss(string cssSelector, Regex text, Options options = null)
        {
            return FindCss(cssSelector, text, options).Exists();
        }

        [Obsolete("For assertions please use Assert.That(scope, Shows.Css(\".your-selector\",etc); instead for decent feedback. For the old behaviour of HasXPath you can use FindXPath(...).Exists();")]
        public bool HasXPath(string xpath, Options options = null)
        {
            return FindXPath(xpath,options).Exists();
        }

        [Obsolete("For assertions please use Assert.That(scope, Shows.No.Css(\".your-selector\",etc); instead for decent feedback. For the old behaviour of HasNoCss you can use FindCss(...).Missing();")]
        public bool HasNoCss(string cssSelector, string text, Options options = null)
        {
            return FindCss(cssSelector, text, options).Missing();
        }

        [Obsolete("For assertions please use Assert.That(scope, Shows.No.Css(\".your-selector\",etc); instead for decent feedback. For the old behaviour of HasNOCss you can use FindCss(...).Missing();")]
        public bool HasNoCss(string cssSelector, Regex text, Options options = null)
        {
            return FindCss(cssSelector, text, options).Missing();
        }

        [Obsolete("For assertions please use Assert.That(scope, Shows.No.XPath(\"/your/xpath\",etc); instead for decent feedback. For the old behaviour of HasNoXPath you can use FindXPath(...).Missing();")]
        public bool HasNoXPath(string xpath, Options options = null)
        {
            return FindXPath(xpath, options).Missing();
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
            return new SectionFinder(driver, locator, this, Merge(options)).AsScope();
        }

        public ElementScope FindFieldset(string locator, Options options = null)
        {
            return new FieldsetFinder(driver, locator, this, Merge(options)).AsScope();
        }

        public ElementScope FindId(string id, Options options = null)
        {
            return new IdFinder(driver, id, this, Merge(options)).AsScope();
        }

        public ElementScope FindIdEndingWith(string endsWith, Options options = null)
        {
            return FindCss(string.Format(@"*[id$=""{0}""]", endsWith), options);
        }

        public void Check(string locator, Options options = null)
        {
            RetryUntilTimeout(new CheckAction(driver, FindField(locator, options), Merge(options)));
        }

        public void Uncheck(string locator, Options options = null)
        {
            RetryUntilTimeout(new Uncheck(driver, FindField(locator, options), Merge(options)));
        }

        public void Choose(string locator, Options options = null)
        {
            RetryUntilTimeout(new Choose(driver, FindField(locator, options), Merge(options)));
        }

        public void RetryUntilTimeout(Action action, Options options = null)
        {
            timingStrategy.Synchronise(new LambdaBrowserAction(action, Merge(options)));
        }

        public TResult RetryUntilTimeout<TResult>(Func<TResult> function, Options options = null)
        {
            return timingStrategy.Synchronise(new LambdaQuery<TResult>(function, Merge(options)));
        }

        public void RetryUntilTimeout(BrowserAction action)
        {
            Query(action);
        }

        public ElementScope FindFrame(string locator, Options options = null)
        {
            return new FrameFinder(driver, locator, this, Merge(options)).AsScope();
        }

        public T Query<T>(Func<T> query, T expecting, Options options = null)
        {
            return timingStrategy.Synchronise(new LambdaQuery<T>(query, expecting, Merge(options)));
        }

        public T Query<T>(Query<T> query)
        {
            return timingStrategy.Synchronise(query);
        }

        public void TryUntil(Action tryThis, Func<bool> until, TimeSpan waitBeforeRetry, Options options = null)
        {
            var mergedOptions = Merge(options);
            var predicateOptions = Options.Merge(new Options {Timeout = waitBeforeRetry}, mergedOptions);

            timingStrategy.TryUntil(new LambdaBrowserAction(tryThis, mergedOptions),
                                    new LambdaPredicateQuery(WithZeroTimeout(until), predicateOptions), mergedOptions);
        }

        private Func<bool> WithZeroTimeout(Func<bool> query)
        {
            var zeroTimeoutUntil = new Func<bool>(() =>
                {
                    var was = timingStrategy.ZeroTimeout;
                    timingStrategy.ZeroTimeout = true;
                    try
                    {
                        return query();
                    }
                    finally
                    {
                        timingStrategy.ZeroTimeout = was;
                    }
                });
            return zeroTimeoutUntil;
        }

        public void TryUntil(BrowserAction tryThis, PredicateQuery until, Options options = null)
        {
            timingStrategy.TryUntil(tryThis, until, Merge(options));
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

        protected internal virtual ElementFound FindElement()
        {
            if (element == null || element.Stale(ElementFinder.Options))
                element = DisambiguationStrategy.ResolveQuery(ElementFinder);
            return element;
        }

    }
}