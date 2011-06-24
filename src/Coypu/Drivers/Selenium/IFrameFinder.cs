using System;
using System.Linq;
using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
    public class IFrameFinder
    {
        private readonly IWebDriver selenium;
        private readonly ElementFinder elementFinder;

        public IFrameFinder(IWebDriver selenium, ElementFinder elementFinder)
        {
            this.selenium = selenium;
            this.elementFinder = elementFinder;
        }

        public IWebElement FindIFrame(string locator)
        {
            var frame = elementFinder.Find(By.TagName("iframe")).FirstOrDefault(e => e.GetAttribute("id") == locator ||
                                                                                     e.GetAttribute("title") == locator ||
                                                                                     FrameContentsMatch(e, locator));
            return frame;
        }

        private bool FrameContentsMatch(IWebElement e, string locator)
        {
            try
            {
                var frame = selenium.SwitchTo().Frame(e);
                return
                    frame.Title == locator ||
                    frame.FindElements(By.XPath(String.Format(".//h1[text() = \"{0}\"]", locator))).Any();
            }
            finally
            {
                selenium.SwitchTo().DefaultContent();
            }
        }
    }
}