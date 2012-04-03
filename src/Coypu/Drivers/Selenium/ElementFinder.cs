using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
    internal class ElementFinder
    {
        private readonly XPath xPath;

        public ElementFinder(XPath xPath)
        {
            this.xPath = xPath;
        }

        public IEnumerable<IWebElement> FindByPartialId(string id, DriverScope scope)
        {
            var xpath = String.Format(".//*[substring(@id, string-length(@id) - {0} + 1, string-length(@id)) = {1}]",
                                      id.Length, xPath.Literal(id));
            return Find(By.XPath(xpath),scope);
        }

        public IEnumerable<IWebElement> Find(By by, DriverScope scope)
        {
            var context = SeleniumScope(scope);
            return context.FindElements(by).Where(e => IsDisplayed(e, scope));
        }

        public ISearchContext SeleniumScope(DriverScope scope)
        {
            return (ISearchContext) scope.Now().Native;
        }

        public bool IsDisplayed(IWebElement e, DriverScope scope)
        {
            return scope.ConsiderInvisibleElements || e.IsDisplayed();
        }
    }
}