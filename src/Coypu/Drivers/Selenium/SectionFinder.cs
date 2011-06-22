using System;
using System.Linq;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Remote;

namespace Coypu.Drivers.Selenium
{
    public class SectionFinder
    {
        private readonly RemoteWebDriver selenium;
        private readonly SeleniumElementFinder elementFinder;
        private readonly TextMatcher textMatcher;

        public SectionFinder(RemoteWebDriver selenium, SeleniumElementFinder elementFinder, TextMatcher textMatcher)
        {
            this.selenium = selenium;
            this.elementFinder = elementFinder;
            this.textMatcher = textMatcher;
        }

        public IWebElement FindSection(string locator)
        {
            return FindSectionByHeaderText(locator) ??
                   elementFinder.Find(By.Id(locator)).FirstDisplayedOrDefault(IsSection);
        }

        private IWebElement FindSectionByHeaderText(string locator) 
        {
            return FindSectionByHeaderText(locator, "section") ??
                   FindSectionByHeaderText(locator, "div");
        }

        private IWebElement FindSectionByHeaderText(string locator, string sectionTag) 
        {
            string[] headerTags = { "h1", "h2", "h3", "h4", "h5", "h6" };
            var headersXPath = String.Join(" or ", headerTags);
            var withAHeader = elementFinder.Find(By.XPath(String.Format(".//{0}[{1}]", sectionTag, headersXPath)));

            var childHeaderSelector = GetChildHeaderSelector(headerTags);
            return
                withAHeader.FirstDisplayedOrDefault(e => Enumerable.Any<IWebElement>(e.FindElements(childHeaderSelector), h => textMatcher.TextMatches(h, locator)));
        }

        private By GetChildHeaderSelector(string[] headerTags) 
        {
            if (selenium is ChromeDriver) {
                var namePredicate = String.Join(" or ",
                                                headerTags.Select(t => String.Format("name() = '{0}'", t)).ToArray());
                return By.XPath(String.Format("./*[{0}]", namePredicate));
            }
            return By.CssSelector(String.Join(",", headerTags));
        }

        private static bool IsSection(IWebElement e) 
        {
            return e.TagName == "section" || e.TagName == "div";
        }
    }
}