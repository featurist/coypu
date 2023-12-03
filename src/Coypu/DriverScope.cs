using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Coypu.Actions;
using Coypu.Drivers;
using Coypu.Finders;
using Coypu.Queries;
using Coypu.Timing;

// ReSharper disable InconsistentNaming
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
namespace Coypu
{
    public abstract class DriverScope : Scope
    {
        protected readonly DisambiguationStrategy DisambiguationStrategy;
        protected readonly SessionConfiguration SessionConfiguration;
        protected TimingStrategy TimingStrategy;
        protected readonly Waiter Waiter;
        internal IDriver _driver;
        internal readonly ElementFinder _elementFinder;
        internal StateFinder StateFinder;
        internal UrlBuilder UrlBuilder;
        private Element _element;

        internal DriverScope(SessionConfiguration sessionConfiguration,
                             ElementFinder elementFinder,
                             IDriver driver,
                             TimingStrategy timingStrategy,
                             Waiter waiter,
                             UrlBuilder urlBuilder,
                             DisambiguationStrategy disambiguationStrategy)
        {
            _driver = driver;
            _elementFinder = elementFinder ?? new DocumentElementFinder(driver, sessionConfiguration);
            DisambiguationStrategy = disambiguationStrategy;
            SessionConfiguration = sessionConfiguration;
            StateFinder = new StateFinder(timingStrategy);
            TimingStrategy = timingStrategy;
            UrlBuilder = urlBuilder;
            Waiter = waiter;
        }

        internal DriverScope(ElementFinder elementFinder,
                             DriverScope outerScope)
        {
            _driver = outerScope._driver;
            _elementFinder = elementFinder;
            DisambiguationStrategy = outerScope.DisambiguationStrategy;
            OuterScope = outerScope;
            SessionConfiguration = outerScope.SessionConfiguration;
            StateFinder = outerScope.StateFinder;
            TimingStrategy = outerScope.TimingStrategy;
            UrlBuilder = outerScope.UrlBuilder;
            Waiter = outerScope.Waiter;
        }

        public ElementFinder ElementFinder => _elementFinder;

        internal abstract bool Stale { get; set; }

        public string Text => Now().Text;

        public DriverScope OuterScope { get; }

        public virtual Uri Location => _driver.Location(this);

        public Browser Browser => SessionConfiguration.Browser;

        public void ClickButton(string locator,
                                Options options = null)
        {
            RetryUntilTimeout(WaitThenClickButton(locator, Merge(options)));
        }

        public void ClickLink(string locator,
                              Options options = null)
        {
            RetryUntilTimeout(WaitThenClickLink(locator, Merge(options)));
        }

        public Scope ClickButton(string locator,
                                 PredicateQuery until,
                                 Options options = null)
        {
            TryUntil(WaitThenClickButton(locator, Merge(options)), until, Merge(options));
            return this;
        }

        public Scope ClickLink(string locator,
                               PredicateQuery until,
                               Options options = null)
        {
            TryUntil(WaitThenClickLink(locator, Merge(options)), until, Merge(options));
            return this;
        }

        public ElementScope FindButton(string locator,
                                       Options options = null)
        {
            return new ButtonFinder(_driver, locator, this, Merge(options)).AsScope();
        }

        public ElementScope FindLink(string locator,
                                     Options options = null)
        {
            return new LinkFinder(_driver, locator, this, Merge(options)).AsScope();
        }

        public ElementScope FindField(string locator,
                                      Options options = null)
        {
            return new FieldFinder(_driver, locator, this, Merge(options)).AsScope();
        }

        public FillInWith FillIn(string locator,
                                 Options options = null)
        {
            return new FillInWith(FindField(locator, options), _driver, TimingStrategy, Merge(options));
        }

        public SelectFrom Select(string option,
                                 Options options = null)
        {
            return new SelectFrom(option, _driver, TimingStrategy, this, Merge(options), DisambiguationStrategy);
        }

        public bool HasContent(string text,
                               Options options = null)
        {
            return Query(new HasContentQuery(this, text, Merge(options)));
        }

        public bool HasContentMatch(Regex pattern,
                                    Options options = null)
        {
            return Query(new HasContentMatchQuery(this, pattern, Merge(options)));
        }

        public bool HasNoContent(string text,
                                 Options options = null)
        {
            return Query(new HasNoContentQuery(this, text, Merge(options)));
        }

        public bool HasNoContentMatch(Regex pattern,
                                      Options options = null)
        {
            return Query(new HasNoContentMatchQuery(this, pattern, Merge(options)));
        }

        public ElementScope FindCss(string cssSelector,
                                    Options options = null)
        {
            return new CssFinder(_driver, cssSelector, this, Merge(options)).AsScope();
        }

        public ElementScope FindCss(string cssSelector,
                                    string text,
                                    Options options = null)
        {
            return new CssFinder(_driver, cssSelector, this, Merge(options), text).AsScope();
        }

        public ElementScope FindCss(string cssSelector,
                                    Regex text,
                                    Options options = null)
        {
            return new CssFinder(_driver, cssSelector, this, Merge(options), text).AsScope();
        }

        public ElementScope FindXPath(string xpath,
                                      Options options = null)
        {
            return new XPathFinder(_driver, xpath, this, Merge(options)).AsScope();
        }

        public ElementScope FindXPath(string xpath,
                                      string text,
                                      Options options = null)
        {
            return new XPathFinder(_driver, xpath, this, Merge(options), text).AsScope();
        }

        public ElementScope FindXPath(string xpath,
                                      Regex text,
                                      Options options = null)
        {
            return new XPathFinder(_driver, xpath, this, Merge(options), text).AsScope();
        }

        public IEnumerable<SnapshotElementScope> FindAllCss(string cssSelector,
                                                            Func<IEnumerable<SnapshotElementScope>, bool> predicate = null,
                                                            Options options = null)
        {
            return Query(new FindAllCssWithPredicateQuery(cssSelector, predicate, this, Merge(options)));
        }

        public IEnumerable<SnapshotElementScope> FindAllXPath(string xpath,
                                                              Func<IEnumerable<SnapshotElementScope>, bool> predicate = null,
                                                              Options options = null)
        {
            return Query(new FindAllXPathWithPredicateQuery(xpath, predicate, this, Merge(options)));
        }

        public ElementScope FindSection(string locator,
                                        Options options = null)
        {
            return new SectionFinder(_driver, locator, this, Merge(options)).AsScope();
        }

        public ElementScope FindFieldset(string locator,
                                         Options options = null)
        {
            return new FieldsetFinder(_driver, locator, this, Merge(options)).AsScope();
        }

        public ElementScope FindId(string id,
                                   Options options = null)
        {
            return new IdFinder(_driver, id, this, Merge(options)).AsScope();
        }

        public ElementScope FindIdEndingWith(string endsWith,
                                             Options options = null)
        {
            return FindCss(string.Format(@"*[id$=""{0}""]", endsWith), options);
        }

        public void Check(string locator,
                          Options options = null)
        {
            RetryUntilTimeout(new CheckAction(_driver, FindField(locator, options), Merge(options)));
        }

        public void Uncheck(string locator,
                            Options options = null)
        {
            RetryUntilTimeout(new Uncheck(_driver, FindField(locator, options), Merge(options)));
        }

        public void Choose(string locator,
                           Options options = null)
        {
            RetryUntilTimeout(new Choose(_driver, FindField(locator, options), Merge(options)));
        }

        public void RetryUntilTimeout(Action action,
                                      Options options = null)
        {
            TimingStrategy.Synchronise(new LambdaBrowserAction(action, Merge(options)));
        }

        public TResult RetryUntilTimeout<TResult>(Func<TResult> function,
                                                  Options options = null)
        {
            return TimingStrategy.Synchronise(new LambdaQuery<TResult>(function, Merge(options)));
        }

        public void RetryUntilTimeout(BrowserAction action)
        {
            Query(action);
        }

        public ElementScope FindFrame(string locator,
                                      Options options = null)
        {
            return new FrameFinder(_driver, locator, this, Merge(options)).AsScope();
        }

        public T Query<T>(Func<T> query,
                          T expecting,
                          Options options = null)
        {
            return TimingStrategy.Synchronise(new LambdaQuery<T>(query, expecting, Merge(options)));
        }

        public T Query<T>(Query<T> query)
        {
            return TimingStrategy.Synchronise(query);
        }

        public void TryUntil(Action tryThis,
                             Func<bool> until,
                             TimeSpan waitBeforeRetry,
                             Options options = null)
        {
            var mergedOptions = Merge(options);
            var predicateOptions = Options.Merge(new Options {Timeout = waitBeforeRetry}, mergedOptions);

            TimingStrategy.TryUntil(new LambdaBrowserAction(tryThis, mergedOptions),
                                    new LambdaPredicateQuery(WithZeroTimeout(until), predicateOptions),
                                    mergedOptions);
        }

        public void TryUntil(BrowserAction tryThis,
                             PredicateQuery until,
                             Options options = null)
        {
            TimingStrategy.TryUntil(tryThis, until, Merge(options));
        }

        public State FindState(State[] states,
                               Options options)
        {
            return StateFinder.FindState(states, this, Merge(options));
        }

        public State FindState(params State[] states)
        {
            return FindState(states, null);
        }

        /// <summary>
        ///     Try and find this scope now
        /// </summary>
        /// <returns></returns>
        /// <exception cref="T:Coypu.MissingHtmlException">Thrown if the element cannot be found</exception>
        /// <exception cref="T:Coypu.AmbiguousHtmlException">
        ///     Thrown if the there is more than one matching element and the
        ///     Match.Single option is set
        /// </exception>
        public virtual Element Now()
        {
            return FindElement();
        }

        protected internal virtual Element FindElement()
        {
            if (_element == null || Stale)
                _element = DisambiguationStrategy.ResolveQuery(ElementFinder);
            return _element;
        }

        internal Options Merge(Options options)
        {
            var mergeWith = ElementFinder != null
                                ? ElementFinder.Options
                                : SessionConfiguration;
            return Options.Merge(options, mergeWith);
        }

        internal IEnumerable<SnapshotElementScope> FindAllCssNoPredicate(string cssSelector,
                                                                         Options options)
        {
            return _driver.FindAllCss(cssSelector, this, options)
                          .AsSnapshotElementScopes(this, options);
        }

        internal IEnumerable<SnapshotElementScope> FindAllXPathNoPredicate(string xpath,
                                                                           Options options)
        {
            return _driver.FindAllXPath(xpath, this, options)
                          .AsSnapshotElementScopes(this, options);
        }

        private WaitThenClick WaitThenClickLink(string locator,
                                                Options options = null)
        {
            return new WaitThenClick(_driver, this, Merge(options), Waiter, new LinkFinder(_driver, locator, this, Merge(options)), DisambiguationStrategy);
        }

        private WaitThenClick WaitThenClickButton(string locator,
                                                  Options options = null)
        {
            return new WaitThenClick(_driver, this, Merge(options), Waiter, new ButtonFinder(_driver, locator, this, Merge(options)), DisambiguationStrategy);
        }

        private Func<bool> WithZeroTimeout(Func<bool> query)
        {
            var zeroTimeoutUntil = new Func<bool>(() =>
            {
                var was = TimingStrategy.ZeroTimeout;
                TimingStrategy.ZeroTimeout = true;
                try
                {
                    return query();
                }
                finally
                {
                    TimingStrategy.ZeroTimeout = was;
                }
            });
            return zeroTimeoutUntil;
        }
    }
}
