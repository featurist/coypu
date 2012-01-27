using System.Linq;
using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
    internal class FieldFinder
    {
        private static readonly string[] FieldInputTypes = new[] { "text", "password", "radio", "checkbox", "file" };
        public static readonly string[] InputButtonTypes = new[] { "button", "submit", "image" };
        private readonly ElementFinder elementFinder;
        private readonly XPath xPath;

        public FieldFinder(ElementFinder elementFinder, XPath xPath)
        {
            this.elementFinder = elementFinder;
            this.xPath = xPath;
        }

        public IWebElement FindField(string locator, DriverScope scope)
        {
            return FindFieldFromLabel(locator, scope) ??
                   FindFieldByIdOrName(locator, scope) ??
                   FindFieldByPlaceholder(locator, scope) ??
                   FindRadioButtonFromValue(locator, scope) ??
                   elementFinder.FindByPartialId(locator, scope).FirstOrDefault(IsField);
        }

        private IWebElement FindRadioButtonFromValue(string locator,DriverScope scope)
        {
            return elementFinder.Find(By.XPath(".//input[@type = 'radio']"), scope).FirstOrDefault(e => e.GetAttribute("value") == locator);
        }

        private IWebElement FindFieldFromLabel(string locator,DriverScope scope)
        {
            var label = FindLabelByText(locator, scope);
            if (label == null)
                return null;

            var id = label.GetAttribute("for");

            var field = id != null
                            ? FindFieldById(id, scope)
                            : label.FindElements(By.XPath("*")).FirstDisplayedOrDefault(IsField);

            return field;
        }

        private IWebElement FindLabelByText(string locator, DriverScope scope)
        {
            return
                elementFinder.Find(By.XPath(xPath.Format(".//label[text() = {0}]", locator)), scope).FirstOrDefault() ??
                elementFinder.Find(By.XPath(xPath.Format(".//label[contains(text(),{0})]", locator)), scope).FirstOrDefault();
        }

        private IWebElement FindFieldByPlaceholder(string placeholder,DriverScope scope)
        {
            return elementFinder.Find(By.XPath(xPath.Format(".//input[@placeholder = {0}]", placeholder)), scope).FirstOrDefault(IsField);
        }

        private IWebElement FindFieldByIdOrName(string locator, DriverScope scope)
        {
            var xpathToFind = xPath.Format(".//*[@id = {0} or @name = {0}]", locator);
            return elementFinder.Find(By.XPath(xpathToFind), scope).FirstOrDefault(IsField);
        }

        private IWebElement FindFieldById(string id, DriverScope scope)
        {
            return elementFinder.Find(By.Id(id), scope).FirstOrDefault(IsField);
        }

        private bool IsField(IWebElement e)
        {
            return IsInputField(e) || e.TagName == "select" || e.TagName == "textarea";
        }

        private bool IsInputField(IWebElement e)
        {
            var inputTypes = FieldInputTypes;
            if (elementFinder.ConsiderInvisibleElements)
            {
                inputTypes = inputTypes.Concat(new[]{"hidden"}).ToArray();
            }
            return e.TagName == "input" && inputTypes.Contains(e.GetAttribute("type"));
        }
    }
}