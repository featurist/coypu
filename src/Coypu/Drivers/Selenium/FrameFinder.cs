using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
    internal class FrameFinder
    {
        private readonly IWebDriver selenium;
        private readonly ElementFinder elementFinder;
        private readonly XPath xPath;

        public FrameFinder(IWebDriver selenium, ElementFinder elementFinder, XPath xPath)
        {
            this.selenium = selenium;
            this.elementFinder = elementFinder;
            this.xPath = xPath;
        }

        public IWebElement FindFrame(string locator, Scope scope)
        {
            var frames = AllElementsByTag(scope, "iframe").Union(AllElementsByTag(scope, "frame"));
            
            return scope.Options.FilterWithMatchStrategy(WebElement(locator, frames), "frame: " + locator);
        }

        private IEnumerable<IWebElement> AllElementsByTag(Scope scope, string tagNameToFind)
        {
            return elementFinder.FindAll(By.TagName(tagNameToFind), scope);
        }

        private IEnumerable<IWebElement> WebElement(string locator, IEnumerable<IWebElement> webElements)
        {
            return webElements.Where(e => e.GetAttribute("id") == locator ||
                                                        e.GetAttribute("name") == locator ||
                                                        e.GetAttribute("title") == locator ||
                                                        FrameContentsMatch(e, locator));
        }

        private bool FrameContentsMatch(IWebElement e, string locator)
        {
            var currentHandle = selenium.CurrentWindowHandle;
            try
            {
                var frame = selenium.SwitchTo().Frame(e);
                return
                    frame.Title == locator ||
                    frame.FindElements(By.XPath(xPath.Format(".//h1[text() = {0}]", locator))).Any();
            }
            finally
            {
                selenium.SwitchTo().Window(currentHandle);
            }
        }
    }
}