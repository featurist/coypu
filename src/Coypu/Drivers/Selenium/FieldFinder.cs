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
            try
            {
                return xPath.FieldXPathsByPrecedence(locator, scope)
                            .Select(xpath => elementFinder.FindAll(By.XPath(xpath), scope).SingleOrDefault())
                            .SingleOrDefault(element => element != null);
            }
            catch (InvalidOperationException e)
            {
                throw new AmbiguousHtmlException(string.Format("More than one field found matching locator '{0}')",locator), e);
            }
        }

       
    }
}