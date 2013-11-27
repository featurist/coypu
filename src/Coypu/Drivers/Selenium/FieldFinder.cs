using System;
using System.Linq;
using System.Reflection;
using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
    internal class FieldFinder
    {

        private readonly ElementFinder elementFinder;
        private readonly XPath xPath;

        public FieldFinder(ElementFinder elementFinder, XPath xPath)
        {
            this.elementFinder = elementFinder;
            this.xPath = xPath;
        }

        public IWebElement FindField(string locator, Scope scope)
        {
            var by = xPath.FieldXPathsByPrecedence(locator, scope).Select(By.XPath);
            return elementFinder.Find(by, scope, "field: " + locator);
        }
    }
}