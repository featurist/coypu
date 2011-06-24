using System;
using System.Linq;
using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
    public class FieldFinder
    {
        private static readonly string[] FieldInputTypes = new[] { "text", "password", "radio", "checkbox", "file" };
        public static readonly string[] InputButtonTypes = new[] { "button", "submit", "image" };
        private readonly ElementFinder elementFinder;

        public FieldFinder(ElementFinder elementFinder)
        {
            this.elementFinder = elementFinder;
        }

        public IWebElement FindField(string locator)
        {
            return FindFieldFromLabel(locator) ??
                   FindFieldByIdOrName(locator) ??
                   FindFieldByPlaceholder(locator) ??
                   FindRadioButtonFromValue(locator) ??
                   elementFinder.FindByPartialId(locator).FirstOrDefault(IsField);
        }

        private IWebElement FindRadioButtonFromValue(string locator)
        {
            return elementFinder.Find(By.XPath(".//input[@type = 'radio']")).FirstOrDefault(e => e.GetAttribute("value") == locator);
        }

        private IWebElement FindFieldFromLabel(string locator)
        {
            var label = FindLabelByText(locator);
            if (label == null)
                return null;

            var id = label.GetAttribute("for");

            var field = id != null
                            ? FindFieldById(id)
                            : label.FindElements(By.XPath("*")).FirstDisplayedOrDefault(IsField);

            return field;
        }

        private IWebElement FindLabelByText(string locator)
        {
            return
                elementFinder.Find(By.XPath(String.Format(".//label[text() = \"{0}\"]", locator))).FirstOrDefault() ??
                elementFinder.Find(By.XPath(String.Format(".//label[contains(text(),\"{0}\")]", locator))).FirstOrDefault();
        }

        private IWebElement FindFieldByPlaceholder(string placeholder)
        {
            return elementFinder.Find(By.XPath(String.Format(".//input[@placeholder = \"{0}\"]", placeholder))).FirstOrDefault(IsField);
        }

        private IWebElement FindFieldByIdOrName(string locator)
        {
            var xpathToFind = String.Format(".//*[@id = \"{0}\" or @name = \"{0}\"]", locator);
            return elementFinder.Find(By.XPath(xpathToFind)).FirstOrDefault(IsField);
        }

        private IWebElement FindFieldById(string id)
        {
            return elementFinder.Find(By.Id(id)).FirstOrDefault(IsField);
        }

        private bool IsField(IWebElement e)
        {
            return IsInputField(e) || e.TagName == "select" || e.TagName == "textarea";
        }

        private bool IsInputField(IWebElement e)
        {
            return e.TagName == "input" && FieldInputTypes.Contains(e.GetAttribute("type"));
        }
    }
}