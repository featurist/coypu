using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
    internal class ElementFinder
    {
        public IEnumerable<IWebElement> FindAll(By @by, Scope scope, Options options, Func<IWebElement, bool> predicate = null)
        {
            try
            {
                return SeleniumScope(scope).FindElements(@by).Where(e => matches(predicate, e) && IsDisplayed(e, options));
            }
            catch (StaleElementReferenceException e)
            {
                throw new StaleElementException(e);
            }
        }

        public ISearchContext SeleniumScope(Scope scope)
        {
            return (ISearchContext) scope.Now().Native;
        }

        private static bool matches(Func<IWebElement, bool> predicate, IWebElement element)
        {
            return (predicate == null || predicate(element));
        }

        public bool IsDisplayed(IWebElement e, Options options)
        {
            return options.ConsiderInvisibleElements || e.IsDisplayed();
        }
    }
}