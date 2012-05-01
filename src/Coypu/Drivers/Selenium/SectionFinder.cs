using System;
using System.Linq;
using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
    internal class SectionFinder
    {
        private readonly ElementFinder elementFinder;
        private readonly TextMatcher textMatcher;

        readonly string[] headerTags = { "h1", "h2", "h3", "h4", "h5", "h6" };

        public SectionFinder(ElementFinder elementFinder, TextMatcher textMatcher)
        {
            this.elementFinder = elementFinder;
            this.textMatcher = textMatcher;
        }

        public IWebElement FindSection(string locator, Scope scope)
        {
            return FindSectionByHeaderText(locator,scope) ??
                   elementFinder.Find(By.Id(locator),scope).FirstDisplayedOrDefault(IsSection);
        }

        private IWebElement FindSectionByHeaderText(string locator, Scope scope) 
        {
            return FindSectionByHeaderText(locator, "section",scope) ??
                   FindSectionByHeaderText(locator, "div",scope);
        }

        private IWebElement FindSectionByHeaderText(string locator, string sectionTag, Scope scope) 
        {
            var headersXPath = String.Join(" or ", headerTags);
            var withAHeader = elementFinder.Find(By.XPath(String.Format(".//{0}[{1}]", sectionTag, headersXPath)),scope);

            return withAHeader.FirstDisplayedOrDefault(e => WhereAHeaderMatches(e, locator));
        }

        private bool WhereAHeaderMatches(ISearchContext e, string locator)
        {
            return e.FindElements(By.XPath("./*")).Any(h => headerTags.Contains(h.TagName) && textMatcher.TextMatches(h, locator));
        }

        private static bool IsSection(IWebElement e) 
        {
            return e.TagName == "section" || e.TagName == "div";
        }
    }
}