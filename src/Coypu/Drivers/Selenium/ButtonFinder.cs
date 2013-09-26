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
            return elementFinder.Find(By.XPath(xPath.ButtonXPath(locator)), scope);
        }
    }
}