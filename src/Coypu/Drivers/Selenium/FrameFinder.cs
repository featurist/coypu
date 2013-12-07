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

        public IEnumerable<IWebElement> FindFrame(string locator, Scope scope, bool exact)
        {
            var frames = AllElementsByTag(scope, "iframe")
                        .Union(AllElementsByTag(scope, "frame"));

            return WebElement(locator, frames, exact);
        }

        private IEnumerable<IWebElement> AllElementsByTag(Scope scope, string tagNameToFind)
        {
            return elementFinder.FindAll(By.TagName(tagNameToFind), scope);
        }

        private IEnumerable<IWebElement> WebElement(string locator, IEnumerable<IWebElement> webElements, bool exact)
        {
            return webElements.Where(e => e.GetAttribute("id") == locator ||
                                            e.GetAttribute("name") == locator ||
                                            (exact ? e.GetAttribute("title") == locator : e.GetAttribute("title").Contains(locator)) ||
                                            FrameContentsMatch(e, locator, exact));
        }

        private bool FrameContentsMatch(IWebElement e, string locator, bool exact)
        {
            var currentHandle = selenium.CurrentWindowHandle;
            try
            {
                var frame = selenium.SwitchTo().Frame(e);
                return
                   frame.Title == locator ||
                   frame.FindElements(By.XPath(".//h1[" + xPath.IsText(locator,exact) + "]")).Any();
            }
            finally
            {
                selenium.SwitchTo().Window(currentHandle);
            }
        }
    }
}