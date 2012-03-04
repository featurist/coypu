using System.Linq;
using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
    internal class IFrameFinder
    {
        private readonly IWebDriver selenium;
        private readonly ElementFinder elementFinder;
        private readonly XPath xPath;

        public IFrameFinder(IWebDriver selenium, ElementFinder elementFinder, XPath xPath)
        {
            this.selenium = selenium;
            this.elementFinder = elementFinder;
            this.xPath = xPath;
        }

        public IWebElement FindIFrame(string locator, DriverScope scope)
        {
            var frame = elementFinder.Find(By.TagName("iframe"), scope).FirstOrDefault(e => e.GetAttribute("id") == locator ||
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