using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Coypu.Actions;
using Coypu.Finders;
using Coypu.Predicates;
using Coypu.Queries;
using Coypu.Robustness;

namespace Coypu
{
    public class ElementScope : Element, Scope<ElementScope>
    {
        private readonly ElementFinder elementFinder;
        private readonly DriverScope driverScope;
        private readonly RobustWrapper robustWrapper;

        internal ElementScope(ElementFinder elementFinder, DriverScope outerScope, RobustWrapper robustWrapper)
        {
            this.elementFinder = elementFinder;
            this.driverScope = new DriverScope(elementFinder,outerScope);
            this.robustWrapper = robustWrapper;
        }

        public string Id
        {
            get { throw new System.NotImplementedException(); }
        }

        public string Text
        {
            get { throw new System.NotImplementedException(); }
        }

        public string Value
        {
            get { throw new System.NotImplementedException(); }
        }

        public string Name
        {
            get { throw new System.NotImplementedException(); }
        }

        public string SelectedOption
        {
            get { throw new System.NotImplementedException(); }
        }

        public bool Selected
        {
            get { throw new System.NotImplementedException(); }
        }

        public object Native
        {
            get { throw new System.NotImplementedException(); }
        }

        protected internal DriverScope DriverScope
        {
            get { return driverScope; }
        }

        public string this[string attributeName]
        {
            get { throw new System.NotImplementedException(); }
        }

        public virtual Element Now()
        {
            return DriverScope.Now();
        }

        public ElementScope Click()
        {
            robustWrapper.RobustlyDo(new Click(elementFinder, DriverScope));
            return this;
        }

        public ElementScope Hover()
        {
            robustWrapper.RobustlyDo(new Hover(elementFinder,DriverScope));
            return this;
        }

        public ElementScope Hover(Element element)
        {
            DriverScope.Hover(element);
            return this;
        }

        public ElementScope ClickButton(string locator)
        {
            DriverScope.ClickButton(locator);
            return this;
        }

        public ElementScope ClickLink(string locator)
        {
            DriverScope.ClickLink(locator);
            return this;
        }

        public ElementScope ClickButton(string locator, Func<bool> until, TimeSpan waitBetweenRetries)
        {
            DriverScope.ClickButton(locator, until, waitBetweenRetries);
            return this;
        }

        public ElementScope ClickLink(string locator, Func<bool> until, TimeSpan waitBetweenRetries)
        {
            DriverScope.ClickLink(locator, until, waitBetweenRetries);
            return this;
        }

        public ElementScope FindButton(string locator)
        {
            return DriverScope.FindButton(locator);
        }

        public ElementScope FindLink(string locator)
        {
            return DriverScope.FindLink(locator);
        }

        public ElementScope FindField(string locator)
        {
            return DriverScope.FindField(locator);
        }

        public FillInWith FillIn(string locator)
        {
            return DriverScope.FillIn(locator);
        }

        public FillInWith FillIn(Element element)
        {
            return DriverScope.FillIn(element);
        }

        public SelectFrom Select(string option)
        {
            return DriverScope.Select(option);
        }

        public bool HasContent(string text)
        {
            return DriverScope.HasContent(text);
        }

        public bool HasContentMatch(Regex pattern)
        {
            return DriverScope.HasContentMatch(pattern);
        }

        public bool HasNoContent(string text)
        {
            return DriverScope.HasNoContent(text);
        }

        public bool HasNoContentMatch(Regex pattern)
        {
            return DriverScope.HasContentMatch(pattern);
        }

        public bool HasCss(string cssSelector)
        {
            return DriverScope.HasCss(cssSelector);
        }

        public bool HasNoCss(string cssSelector)
        {
            return DriverScope.HasNoCss(cssSelector);
        }

        public bool HasXPath(string xpath)
        {
            return DriverScope.HasXPath(xpath);
        }

        public bool HasNoXPath(string xpath)
        {
            return DriverScope.HasNoXPath(xpath);
        }

        public ElementScope FindCss(string cssSelector)
        {
            return DriverScope.FindCss(cssSelector);
        }

        public ElementScope FindXPath(string xpath)
        {
            return DriverScope.FindXPath(xpath);
        }

        public IEnumerable<Element> FindAllCss(string cssSelector)
        {
            return DriverScope.FindAllCss(cssSelector);
        }

        public IEnumerable<Element> FindAllXPath(string xpath)
        {
            return DriverScope.FindAllXPath(xpath);
        }

        public ElementScope FindSection(string locator)
        {
            return DriverScope.FindSection(locator);
        }

        public ElementScope FindFieldset(string locator)
        {
            return DriverScope.FindFieldset(locator);
        }

        public ElementScope FindId(string id)
        {
            return DriverScope.FindId(id);
        }

        public ElementScope Check(string locator)
        {
            DriverScope.Check(locator);
            return this;
        }

        public ElementScope Uncheck(string locator)
        {
            DriverScope.Uncheck(locator);
            return this;
        }

        public ElementScope Choose(string locator)
        {
            DriverScope.Choose(locator);
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

        public void RetryUntilTimeout(Action action)
        {
            DriverScope.RetryUntilTimeout(action);
        }

        public TResult RetryUntilTimeout<TResult>(Func<TResult> function)
        {
            return DriverScope.RetryUntilTimeout(function);
        }

        public T Query<T>(Func<T> query, T expecting)
        {
            return DriverScope.Query(query, expecting);
        }

        public void TryUntil(Action tryThis, Func<bool> until, TimeSpan waitBeforeRetry)
        {
            DriverScope.TryUntil(tryThis, until, waitBeforeRetry);
        }

        public void TryUntil(DriverAction tryThis, BrowserSessionPredicate until, TimeSpan waitBeforeRetry)
        {
            DriverScope.TryUntil(tryThis, until, waitBeforeRetry);
        }

        public State FindState(params State[] states)
        {
            return DriverScope.FindState(states);
        }

        public ElementScope ConsideringInvisibleElements()
        {
            DriverScope.ConsideringInvisibleElements();
            return this;
        }

        public ElementScope ConsideringOnlyVisibleElements()
        {
            DriverScope.ConsideringOnlyVisibleElements();
            return this;
        }

        public ElementScope WithIndividualTimeout(TimeSpan timeout)
        {
            DriverScope.WithIndividualTimeout(timeout);
            return this;
        }

        public bool Exists()
        {
            return robustWrapper.Query(new ElementExistsQuery(driverScope));
        }

        public bool Missing()
        {
            return robustWrapper.Query(new ElementMissingQuery(driverScope));
        }
    }
}