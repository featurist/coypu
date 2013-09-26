using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
    internal class ElementFinder
    {
        public IWebElement Find(By by, Scope scope, Func<IWebElement, bool> predicate = null)
        {
            var context = SeleniumScope(scope);
            IWebElement firstMatch = null;
            try
            {
                firstMatch = context.FindElement(@by);
            }
            catch (WebDriverException) {}

            if (scope.ConsiderInvisibleElements && matches(predicate, firstMatch))
                return firstMatch;

            return firstMatch != null && firstMatch.IsDisplayed() && matches(predicate, firstMatch)
                       ? firstMatch
                       : context.FindElements(@by).FirstOrDefault(e => IsDisplayed(e, scope) && matches(predicate, e));
        }

        private static bool matches(Func<IWebElement, bool> predicate, IWebElement firstMatch)
        {
            return (predicate == null || predicate(firstMatch));
        }

        public IEnumerable<IWebElement> FindAll(By by, Scope scope)
        {
            var context = SeleniumScope(scope);
            return scope.ConsiderInvisibleElements 
                ? context.FindElements(@by) 
                : context.FindElements(@by).Where(e => IsDisplayed(e, scope));
        }

        public ISearchContext SeleniumScope(Scope scope)
        {
            return (ISearchContext) scope.Now().Native;
        }

        public bool IsDisplayed(IWebElement e, Scope scope)
        {
            return scope.ConsiderInvisibleElements || e.IsDisplayed();
        }
    }
}