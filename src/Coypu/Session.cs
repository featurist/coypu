using System;
using System.Collections.Generic;
using Coypu.Robustness;
using System.Text.RegularExpressions;

namespace Coypu
{
    public class Session : IDisposable
    {
        private readonly Driver driver;
        private readonly RobustWrapper robustWrapper;
        private readonly Clicker clicker;
        private readonly UrlBuilder urlBuilder;
        public bool WasDisposed { get; private set; }

        public Driver Driver
        {
            get { return driver; }
        }

        public object Native
        {
            get { return driver.Native; }
        }

        public RobustWrapper RobustWrapper
        {
            get { return robustWrapper; }
        }

        public Session(Driver driver, RobustWrapper robustWrapper)
        {
            this.robustWrapper = robustWrapper;
            this.driver = driver;
            clicker = new Clicker(driver);
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
            robustWrapper.Robustly(() => clicker.FindAndClickButton(locator));
        }

        public void ClickLink(string locator)
        {
            robustWrapper.Robustly(() => clicker.FindAndClickLink(locator));
        }

        public void Visit(string virtualPath)
        {
            driver.Visit(urlBuilder.GetFullyQualifiedUrl(virtualPath));
        }

        public void Click(Element element)
        {
            robustWrapper.Robustly(() => driver.Click(element));
        }

        public Element FindButton(string locator)
        {
            return robustWrapper.Robustly(() => driver.FindButton(locator));
        }

        public Element FindLink(string locator)
        {
            return robustWrapper.Robustly(() => driver.FindLink(locator));
        }

        public Element FindField(string locator)
        {
            return robustWrapper.Robustly(() => driver.FindField(locator));
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
            return robustWrapper.Query(() => driver.HasContent(text), true);
        }
        
        public bool HasContentMatch(Regex pattern)
        {
            return robustWrapper.Query(() => driver.HasContentMatch(pattern), true);
        }

        public bool HasNoContent(string text)
        {
            return robustWrapper.Query(() => driver.HasContent(text), false);
        }

        public bool HasNoContentMatch(Regex pattern)
        {
            return robustWrapper.Query(() => driver.HasContentMatch(pattern), false);
        }

        public bool HasCss(string cssSelector)
        {
            return robustWrapper.Query(() => driver.HasCss(cssSelector), true);
        }

        public bool HasNoCss(string cssSelector)
        {
            return robustWrapper.Query(() => driver.HasCss(cssSelector), false);
        }

        public bool HasXPath(string xpath)
        {
            return robustWrapper.Query(() => driver.HasXPath(xpath), true);
        }

        public bool HasNoXPath(string xpath)
        {
            return robustWrapper.Query(() => driver.HasXPath(xpath), false);
        }

        public Element FindCss(string cssSelector)
        {
            return robustWrapper.Robustly(() => driver.FindCss(cssSelector));
        }

        public Element FindXPath(string xpath)
        {
            return robustWrapper.Robustly(() => driver.FindXPath(xpath));
        }

        public IEnumerable<Element> FindAllCss(string cssSelector)
        {
            return driver.FindAllCss(cssSelector);
        }

        public IEnumerable<Element> FindAllXPath(string xpath)
        {
            return driver.FindAllXPath(xpath);
        }

        public void Check(string locator)
        {
            robustWrapper.Robustly(() => driver.Check(driver.FindField(locator)));
        }

        public void Uncheck(string locator)
        {
            robustWrapper.Robustly(() => driver.Uncheck(driver.FindField(locator)));
        }

        public void Choose(string locator)
        {
            robustWrapper.Robustly(() => driver.Choose(driver.FindField(locator)));
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
            robustWrapper.Robustly(() => driver.AcceptModalDialog());
        }

        public void CancelModalDialog()
        {
            robustWrapper.Robustly(() => driver.CancelModalDialog());
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
            robustWrapper.TryUntil(() => ClickButton(locator), until, waitBetweenRetries);
        }

        public void ClickLink(string locator, Func<bool> until, TimeSpan waitBetweenRetries)
        {
            robustWrapper.TryUntil(() => ClickLink(locator), until, waitBetweenRetries);
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
            robustWrapper.Robustly(() => driver.Click(findElement()));
        }

        public void Hover(Func<Element> findElement)
        {
            robustWrapper.Robustly(() => driver.Hover(findElement()));
            
        }
    }
}