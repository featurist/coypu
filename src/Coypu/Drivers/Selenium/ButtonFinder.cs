using System;
using System.Linq;
using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
    internal class ButtonFinder
    {
        private readonly ElementFinder elementFinder;
        private readonly TextMatcher textMatcher;
        private readonly XPath xPath;

        public ButtonFinder(ElementFinder elementFinder, TextMatcher textMatcher, XPath xPath)
        {
            this.elementFinder = elementFinder;
            this.textMatcher = textMatcher;
            this.xPath = xPath;
        }

        public IWebElement FindButton2(string locator)
        {
            return FindButtonByText(locator) ??
                   FindButtonByIdNameOrValue(locator) ??
                   elementFinder.FindByPartialId(locator).FirstOrDefault(IsButton);
        }

        private IWebElement FindButtonByIdNameOrValue(string locator) 
        {
            var xpathToFind = xPath.Format(".//*[@id = {0} or @name = {0} or @value = {0} or @alt = {0}]", locator);
            return elementFinder.Find(By.XPath(xpathToFind)).FirstOrDefault(IsButton);
        }

        private IWebElement FindButtonByText(string locator) 
        {
            return
                elementFinder.Find(By.TagName("button")).FirstOrDefault(e => textMatcher.TextMatches(e, locator)) ??
                elementFinder.Find(By.ClassName("button")).FirstOrDefault(e => textMatcher.TextMatches(e, locator)) ??
                elementFinder.Find(By.XPath(".//*[@role = 'button']")).FirstOrDefault(e => textMatcher.TextMatches(e, locator));
        }

        private bool IsButton(IWebElement e)
        {
            return e.TagName == "button" || IsInputButton(e) || e.GetAttribute("role") == "button";
        }

        private bool IsInputButton(IWebElement e)
        {
            return e.TagName == "input" && FieldFinder.InputButtonTypes.Contains(e.GetAttribute("type"));
        }
    }
}