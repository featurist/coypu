using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
    internal class ElementFinder
    {
        private readonly Scoping scoping;
        private readonly XPath xPath;

        public ElementFinder(Scoping scoping, XPath xPath)
        {
            this.scoping = scoping;
            this.xPath = xPath;
        }

        public bool ConsiderInvisibleElements { get; set; }

        public IEnumerable<IWebElement> FindByPartialId(string id)
        {
            var xpath = String.Format(".//*[substring(@id, string-length(@id) - {0} + 1, string-length(@id)) = {1}]",
                                      id.Length, xPath.Literal(id));
            return Find(By.XPath(xpath));
        }

        public IEnumerable<IWebElement> Find(By by)
        {
            return scoping.Scope.FindElements(by).Where(IsDisplayed);
        }

        public bool IsDisplayed(IWebElement e)
        {
            return e.IsDisplayed() || ConsiderInvisibleElements;
        }
    }
}