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

        public bool ConsiderInvisibleElements { get; set; }

        public IEnumerable<IWebElement> FindByPartialId(string id, DriverScope scope)
        {
            var xpath = String.Format(".//*[substring(@id, string-length(@id) - {0} + 1, string-length(@id)) = {1}]",
                                      id.Length, xPath.Literal(id));
            return Find(By.XPath(xpath),scope);
        }

        public IEnumerable<IWebElement> Find(By by, DriverScope scope)
        {
            return SeleniumScope(scope).FindElements(by).Where(IsDisplayed);
        }

        public static ISearchContext SeleniumScope(DriverScope scope)
        {
            return (ISearchContext) scope.Now();
        }

        public bool IsDisplayed(IWebElement e)
        {
            return e.IsDisplayed() || ConsiderInvisibleElements;
        }
    }
}