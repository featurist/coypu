using System.Linq;
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
            return xPath.FieldXPaths(locator, scope)
                        .Select(xpath => elementFinder.Find(By.XPath(xpath), scope).FirstOrDefault())
                        .FirstOrDefault(element => element != null);
        }

       
    }
}