using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Coypu
{
    public class ElementScope : Element, Scope<ElementScope>
    {
        private readonly ElementFinder elementFinder;
        private readonly DriverScope driverScope;

        public ElementScope(ElementFinder elementFinder, DriverScope driverScope)
        {
            this.elementFinder = elementFinder;
            this.driverScope = driverScope;
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

        public string this[string attributeName]
        {
            get { throw new System.NotImplementedException(); }
        }

        public Element Now()
        {
            return elementFinder.Find();
        }

        public ElementScope Click()
        {
            Click(Now());
            return this;
        }

        public ElementScope Click(Element element)
        {
            driverScope.Click(element);
            return this;
        }

        public ElementScope Hover()
        {
            Hover(Now());
            return this;
        }

        public ElementScope Hover(Element element)
        {
            driverScope.Hover(element);
            return this;
        }

        public ElementScope ClickButton(string locator)
        {
            driverScope.ClickButton(locator);
            return this;
        }

        public ElementScope ClickLink(string locator)
        {
            driverScope.ClickLink(locator);
            return this;
        }

        public ElementScope ClickButton(string locator, Func<bool> until, TimeSpan waitBetweenRetries)
        {
            driverScope.ClickButton(locator, until, waitBetweenRetries);
            return this;
        }

        public ElementScope ClickLink(string locator, Func<bool> until, TimeSpan waitBetweenRetries)
        {
            driverScope.ClickLink(locator, until, waitBetweenRetries);
            return this;
        }

        public ElementScope FindButton(string locator)
        {
            return driverScope.FindButton(locator);
        }

        public ElementScope FindLink(string locator)
        {
            return driverScope.FindLink(locator);
        }

        public ElementScope FindField(string locator)
        {
            return driverScope.FindField(locator);
        }

        public FillInWith FillIn(string locator)
        {
            return driverScope.FillIn(locator);
        }

        public FillInWith FillIn(Element element)
        {
            return driverScope.FillIn(element);
        }

        public SelectFrom Select(string option)
        {
            return driverScope.Select(option);
        }

        public bool HasContent(string text)
        {
            return driverScope.HasContent(text);
        }

        public bool HasContentMatch(Regex pattern)
        {
            return driverScope.HasContentMatch(pattern);
        }

        public bool HasNoContent(string text)
        {
            return driverScope.HasNoContent(text);
        }

        public bool HasNoContentMatch(Regex pattern)
        {
            return driverScope.HasContentMatch(pattern);
        }

        public bool HasCss(string cssSelector)
        {
            return driverScope.HasCss(cssSelector);
        }

        public bool HasNoCss(string cssSelector)
        {
            return driverScope.HasNoCss(cssSelector);
        }

        public bool HasXPath(string xpath)
        {
            return driverScope.HasXPath(xpath);
        }

        public bool HasNoXPath(string xpath)
        {
            return driverScope.HasNoXPath(xpath);
        }

        public ElementScope FindCss(string cssSelector)
        {
            return driverScope.FindCss(cssSelector);
        }

        public ElementScope FindXPath(string xpath)
        {
            return driverScope.FindXPath(xpath);
        }

        public IEnumerable<Element> FindAllCss(string cssSelector)
        {
            return driverScope.FindAllCss(cssSelector);
        }

        public IEnumerable<Element> FindAllXPath(string xpath)
        {
            return driverScope.FindAllXPath(xpath);
        }

        public ElementScope FindSection(string locator)
        {
            return driverScope.FindSection(locator);
        }

        public ElementScope FindFieldset(string locator)
        {
            return driverScope.FindFieldset(locator);
        }

        public ElementScope FindId(string id)
        {
            return driverScope.FindId(id);
        }

        public ElementScope Check(string locator)
        {
            driverScope.Check(locator);
            return this;
        }

        public ElementScope Uncheck(string locator)
        {
            driverScope.Uncheck(locator);
            return this;
        }

        public ElementScope Choose(string locator)
        {
            driverScope.Choose(locator);
            return this;
        }

        public string ExecuteScript(string javascript)
        {
            return driverScope.ExecuteScript(javascript);
        }

        public bool Has(ElementScope findElement)
        {
            return driverScope.Has(findElement);
        }

        public bool HasNo(ElementScope findElement)
        {
            return driverScope.HasNo(findElement);
        }

        public void RetryUntilTimeout(Action action)
        {
            driverScope.RetryUntilTimeout(action);
        }

        public TResult RetryUntilTimeout<TResult>(Func<TResult> function)
        {
            return driverScope.RetryUntilTimeout(function);
        }

        public T Query<T>(Func<T> query, T expecting)
        {
            return driverScope.Query(query, expecting);
        }

        public void TryUntil(Action tryThis, Func<bool> until, TimeSpan waitBeforeRetry)
        {
            driverScope.TryUntil(tryThis, until, waitBeforeRetry);
        }

        public State FindState(params State[] states)
        {
            return driverScope.FindState(states);
        }

        public ElementScope ConsideringInvisibleElements()
        {
            driverScope.ConsideringInvisibleElements();
            return this;
        }

        public ElementScope WithIndividualTimeout(TimeSpan timeout)
        {
            driverScope.WithIndividualTimeout(timeout);
            return this;
        }
    }
}