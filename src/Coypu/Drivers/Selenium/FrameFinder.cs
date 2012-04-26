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

        public IWebElement FindFrame(string locator, DriverScope scope)
        {
            var frame = FindFrameByTag(locator, scope, "iframe") ??
                        FindFrameByTag(locator, scope, "frame");
            return frame;
        }

        private IWebElement FindFrameByTag(string locator, DriverScope scope, string tagNameToFind)
        {
            return WebElement(locator, elementFinder.Find(By.TagName(tagNameToFind), scope));
        }

        private IWebElement WebElement(string locator, IEnumerable<IWebElement> webElements)
        {
            var frame = webElements.FirstOrDefault(e => e.GetAttribute("id") == locator ||
                                                        e.GetAttribute("name") == locator ||
                                                        e.GetAttribute("title") == locator ||
                                                        FrameContentsMatch(e, locator));
            return frame;
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