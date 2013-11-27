using System.Linq;
using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
    internal class ButtonFinder
    {
        
        private readonly ElementFinder elementFinder;
        private readonly XPath xPath;

        public ButtonFinder(ElementFinder elementFinder, XPath xPath)
        {
            this.elementFinder = elementFinder;
            this.xPath = xPath;
        }

        public IWebElement FindButton(string locator, Scope scope)
        {
            var by = xPath.ButtonXPathsByPrecedence(locator, scope.Options).Select(By.XPath);
            return elementFinder.Find(by, scope, "button: " + locator);
        }
    }
}