using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Coypu.Actions;
using Coypu.Finders;
using Coypu.Queries;
using Coypu.Robustness;

namespace Coypu
{
    public class ElementScope : Element, Scope<ElementScope>
    {
        private readonly DriverScope driverScope;
        private readonly RobustWrapper robustWrapper;

        internal ElementScope(ElementFinder elementFinder, DriverScope outerScope, RobustWrapper robustWrapper)
        {
            driverScope = new DriverScope(elementFinder, outerScope);
            this.robustWrapper = robustWrapper;
        }

        public string Id
        {
            get { return Now().Id; }
        }

        public string Text
        {
            get { return Now().Text; }
        }

        public string Value
        {
            get { return Now().Value; }
        }

        public string Name
        {
            get { return Now().Name; }
        }

        public string SelectedOption
        {
            get { return Now().SelectedOption; }
        }

        public bool Selected
        {
            get { return Now().Selected; }
        }

        public object Native
        {
            get { return Now().Native; }
        }

        protected internal DriverScope DriverScope
        {
            get { return driverScope; }
        }

        public string this[string attributeName]
        {
            get { return Now()[attributeName]; }
        }

        public virtual ElementFound Now()
        {
            return driverScope.Now();
        }

        public ElementScope Click(Options options = null)
        {
            driverScope.Click(options);
            return this;
        }

        public ElementScope Hover(Options options = null)
        {
            driverScope.Hover(options);
            return this;
        }

        public ElementScope ClickButton(string locator, Options options = null)
        {
            DriverScope.ClickButton(locator, options);
            return this;
        }

        public ElementScope ClickLink(string locator, Options options = null)
        {
            DriverScope.ClickLink(locator, options);
            return this;
        }

        public ElementScope FindButton(string locator, Options options = null)
        {
            return DriverScope.FindButton(locator, options);
        }

        public ElementScope FindLink(string locator, Options options = null)
        {
            return DriverScope.FindLink(locator, options);
        }

        public ElementScope FindField(string locator, Options options = null)
        {
            return DriverScope.FindField(locator, options);
        }

        public FillInWith FillIn(string locator, Options options = null)
        {
            return DriverScope.FillIn(locator, options);
        }

        public FillInWith FillIn(Element element, Options options = null)
        {
            return DriverScope.FillIn(element, options);
        }

        public SelectFrom Select(string option, Options options = null)
        {
            return DriverScope.Select(option, options);
        }

        public bool HasContent(string text, Options options = null)
        {
            return DriverScope.HasContent(text, options);
        }

        public bool HasContentMatch(Regex pattern, Options options = null)
        {
            return DriverScope.HasContentMatch(pattern, options);
        }

        public bool HasNoContent(string text, Options options = null)
        {
            return DriverScope.HasNoContent(text, options);
        }

        public bool HasNoContentMatch(Regex pattern, Options options = null)
        {
            return DriverScope.HasContentMatch(pattern, options);
        }

        public bool HasCss(string cssSelector, Options options = null)
        {
            return DriverScope.HasCss(cssSelector, options);
        }

        public bool HasNoCss(string cssSelector, Options options = null)
        {
            return DriverScope.HasNoCss(cssSelector, options);
        }

        public bool HasXPath(string xpath, Options options = null)
        {
            return DriverScope.HasXPath(xpath, options);
        }

        public bool HasNoXPath(string xpath, Options options = null)
        {
            return DriverScope.HasNoXPath(xpath, options);
        }

        public ElementScope FindCss(string cssSelector, Options options = null)
        {
            return DriverScope.FindCss(cssSelector, options);
        }

        public ElementScope FindXPath(string xpath, Options options = null)
        {
            return DriverScope.FindXPath(xpath, options);
        }

        public IEnumerable<ElementFound> FindAllCss(string cssSelector, Options options = null)
        {
            return DriverScope.FindAllCss(cssSelector, options);
        }

        public IEnumerable<ElementFound> FindAllXPath(string xpath, Options options = null)
        {
            return DriverScope.FindAllXPath(xpath, options);
        }

        public ElementScope FindSection(string locator, Options options = null)
        {
            return DriverScope.FindSection(locator, options);
        }

        public ElementScope FindFieldset(string locator, Options options = null)
        {
            return DriverScope.FindFieldset(locator, options);
        }

        public ElementScope FindId(string id, Options options = null)
        {
            return DriverScope.FindId(id, options);
        }

        public IFrameElementScope FindIFrame(string locator, Options options = null)
        {
            return DriverScope.FindIFrame(locator, options);
        }

        public ElementScope Check(string locator, Options options = null)
        {
            DriverScope.Check(locator, options);
            return this;
        }

        public ElementScope Uncheck(string locator, Options options = null)
        {
            DriverScope.Uncheck(locator, options);
            return this;
        }

        public ElementScope Choose(string locator, Options options = null)
        {
            DriverScope.Choose(locator, options);
            return this;
        }

        public string ExecuteScript(string javascript)
        {
            return DriverScope.ExecuteScript(javascript);
        }

        public bool Has(ElementScope findElement)
        {
            return DriverScope.Has(findElement);
        }

        public bool HasNo(ElementScope findElement)
        {
            return DriverScope.HasNo(findElement);
        }

        public void RetryUntilTimeout(Action action, Options options = null)
        {
            DriverScope.RetryUntilTimeout(action,options);
        }

        public TResult RetryUntilTimeout<TResult>(Func<TResult> function, Options options = null)
        {
            return DriverScope.RetryUntilTimeout(function,options);
        }

        public void RetryUntilTimeout(DriverAction driverAction)
        {
            DriverScope.RetryUntilTimeout(driverAction);
        }

        public T Query<T>(Func<T> query, T expecting, Options options = null)
        {
            return DriverScope.Query(query, expecting,options);
        }

        public void TryUntil(Action tryThis, Func<bool> until, TimeSpan waitBeforeRetry, Options options = null)
        {
            DriverScope.TryUntil(tryThis, until,waitBeforeRetry, options);
        }

        public void TryUntil(DriverAction tryThis, Query<bool> until, TimeSpan waitBeforeRetry, Options options = null)
        {
            DriverScope.TryUntil(tryThis, until, waitBeforeRetry, options);
        }

        public State FindState(params State[] states)
        {
            return DriverScope.FindState(states);
        }

        public State FindState(State[] states, Options options = null)
        {
            return DriverScope.FindState(states,options);
        }

        public bool Exists(Options options = null)
        {
            return robustWrapper.Robustly(new ElementExistsQuery(driverScope, DriverScope.Default(options)));
        }

        public bool Missing(Options options = null)
        {
            return robustWrapper.Robustly(new ElementMissingQuery(driverScope, DriverScope.Default(options)));
        }
    }
}