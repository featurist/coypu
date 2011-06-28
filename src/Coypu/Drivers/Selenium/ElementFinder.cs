using System;
using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
    internal class ElementFinder
    {
        private readonly Scoping scoping;

        public ElementFinder(Scoping scoping)
        {
            this.scoping = scoping;
        }

        public IEnumerable<IWebElement> FindByPartialId(string id)
        {
            var xpath = String.Format(".//*[substring(@id, string-length(@id) - {0} + 1, string-length(@id)) = '{1}']",
                                      id.Length, id);
            return Find(By.XPath(xpath));
        }

        public IEnumerable<IWebElement> Find(By by)
        {
            return scoping.Scope.FindElements(by).Where(IsDisplayed);
        }

        public bool IsDisplayed(IWebElement e)
        {
            return e.IsDisplayed();
        }
    }
}