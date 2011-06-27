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

        public void ClickButton(string locator)
        {
            Robustly(() => clicker.FindAndClickButton(locator));
        }

        public void ClickLink(string locator)
        {
            Robustly(() => clicker.FindAndClickLink(locator));
        }

        public void Visit(string virtualPath)
        {
            driver.Visit(urlBuilder.GetFullyQualifiedUrl(virtualPath));
        }

        public void Click(Element element)
        {
            Robustly(() => driver.Click(element));
        }

        public Element FindButton(string locator)
        {
            return Robustly(() => driver.FindButton(locator));
        }

        public Element FindLink(string locator)
        {
            return Robustly(() => driver.FindLink(locator));
        }

        public Element FindField(string locator)
        {
            return Robustly(() => driver.FindField(locator));
        }

        public FillInWith FillIn(string locator)
        {
            return new FillInWith(locator, driver, robustWrapper);
        }

        public FillInWith FillIn(Element element)
        {
            return new FillInWith(element, driver, robustWrapper);
        }

        public SelectFrom Select(string option)
        {
            return new SelectFrom(option, driver, robustWrapper);
        }

        public bool HasContent(string text)
        {
            return Query(() => driver.HasContent(text), true);
        }
        
        public bool HasContentMatch(Regex pattern)
        {
            return Query(() => driver.HasContentMatch(pattern), true);
        }

        public bool HasNoContent(string text)
        {
            return Query(() => driver.HasContent(text), false);
        }

        public bool HasNoContentMatch(Regex pattern)
        {
            return Query(() => driver.HasContentMatch(pattern), false);
        }

        public bool HasCss(string cssSelector)
        {
            return Query(() => driver.HasCss(cssSelector), true);
        }

        public bool HasNoCss(string cssSelector)
        {
            return Query(() => driver.HasCss(cssSelector), false);
        }

        public bool HasXPath(string xpath)
        {
            return Query(() => driver.HasXPath(xpath), true);
        }

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

        public void ClickButton(string locator, Func<bool> until, TimeSpan waitBetweenRetries)
        {
            TryUntil(() => ClickButton(locator), until, waitBetweenRetries);
        }

        public void ClickLink(string locator, Func<bool> until, TimeSpan waitBetweenRetries)
        {
            TryUntil(() => ClickLink(locator), until, waitBetweenRetries);
        }

        public string ExecuteScript(string javascript)
        {
            return driver.ExecuteScript(javascript);
        }

        public void WithinIFrame(string locator, Action action)
        {
            Within(() => driver.FindIFrame(locator), action);
        }

        public void Click(Func<Element> findElement)
        {
            Robustly(() => driver.Click(findElement()));
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