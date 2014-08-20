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
        private readonly SeleniumWindowManager seleniumWindowManager;

        public FrameFinder(IWebDriver selenium, ElementFinder elementFinder, XPath xPath, SeleniumWindowManager seleniumWindowManager)
        {
            this.selenium = selenium;
            this.elementFinder = elementFinder;
            this.xPath = xPath;
            this.seleniumWindowManager = seleniumWindowManager;
        }

        public IEnumerable<IWebElement> FindFrame(string locator, Scope scope, Options options)
        {
            var frames = AllElementsByTag(scope, "iframe", options)
                        .Union(AllElementsByTag(scope, "frame", options));

            return WebElement(locator, frames, options);
        }

        private IEnumerable<IWebElement> AllElementsByTag(Scope scope, string tagNameToFind, Options options)
        {
            return elementFinder.FindAll(By.TagName(tagNameToFind), scope, options);
        }

        private IEnumerable<IWebElement> WebElement(string locator, IEnumerable<IWebElement> webElements, Options options)
        {
            return webElements.Where(e => e.GetAttribute("id") == locator ||
                                            e.GetAttribute("name") == locator ||
                                            (options.TextPrecisionExact ? e.GetAttribute("title") == locator : e.GetAttribute("title").Contains(locator)) ||
                                            FrameContentsMatch(e, locator, options));
        }

        private bool FrameContentsMatch(IWebElement e, string locator, Options options)
        {
            var currentHandle = selenium.CurrentWindowHandle;
            try
            {

                var frame = seleniumWindowManager.SwitchToFrame(e);
                return
                   frame.Title == locator ||
                   frame.FindElements(By.XPath(".//h1[" + xPath.IsText(locator, options) + "]")).Any();
            }
            finally
            {
                selenium.SwitchTo().Window(currentHandle);
            }
        }
    }
}