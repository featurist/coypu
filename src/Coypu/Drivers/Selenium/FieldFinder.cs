using System.Linq;
using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
    internal class FieldFinder
    {
        private static readonly string[] FieldInputTypes = new[] { "text", "password", "radio", "checkbox", "file", "email", "tel" };
        public static readonly string[] InputButtonTypes = new[] { "button", "submit", "image" };
        private readonly ElementFinder elementFinder;
        private readonly XPath xPath;

        public FieldFinder(ElementFinder elementFinder, XPath xPath)
        {
            this.elementFinder = elementFinder;
            this.xPath = xPath;
        }

        public IWebElement FindField(string locator, Scope scope)
        {
            return FindFieldById(locator, scope) ??
                   FindFieldFromLabel(locator, scope) ??
                   FindFieldByName(locator, scope) ??
                   FindFieldByPlaceholder(locator, scope) ??
                   FindRadioButtonOrCheckboxFromValue(locator, scope) ??
                   elementFinder.FindByPartialId(locator, scope).FirstOrDefault(e => IsField(e, scope));
        }

        private IWebElement FindRadioButtonOrCheckboxFromValue(string locator, Scope scope) {
            return elementFinder.Find(By.XPath(".//input[@type = 'checkbox' or @type = 'radio']"), scope).FirstOrDefault(e => e.GetAttribute("value") == locator);
        }

        private IWebElement FindFieldFromLabel(string locator,Scope scope)
        {
            var label = FindLabelByText(locator, scope);
            if (label == null)
                return null;

            var id = label.GetAttribute("for");

            var field = id != null
                            ? FindFieldById(id, scope)
                            : label.FindElements(By.XPath("*")).FirstDisplayedOrDefault(e => IsField(e, scope));

            return field;
        }

        private IWebElement FindLabelByText(string locator, Scope scope)
        {
            return
                elementFinder.Find(By.XPath(xPath.Format(".//label[text() = {0}]", locator)), scope).FirstOrDefault() ??
                elementFinder.Find(By.XPath(xPath.Format(".//label[contains(text(),{0})]", locator)), scope).FirstOrDefault();
        }

        private IWebElement FindFieldByPlaceholder(string placeholder,Scope scope)
        {
            return elementFinder.Find(By.XPath(xPath.Format(".//input[@placeholder = {0}]", placeholder)), scope).FirstOrDefault(e => IsField(e, scope));
        }

        private IWebElement FindFieldById(string id, Scope scope)
        {
            return elementFinder.Find(By.Id(id), scope).FirstOrDefault(e => IsField(e, scope));
        }

        private IWebElement FindFieldByName(string name, Scope scope)
        {
            return elementFinder.Find(By.Name(name), scope).FirstOrDefault(e => IsField(e, scope));
        }

        private bool IsField(IWebElement e, Scope scope)
        {
            return IsInputField(e, scope) || e.TagName == "select" || e.TagName == "textarea";
        }

        private bool IsInputField(IWebElement e, Scope scope)
        {
            var inputTypes = FieldInputTypes;
            if (scope.ConsiderInvisibleElements)
            {
                inputTypes = inputTypes.Concat(new[]{"hidden"}).ToArray();
            }
            return e.TagName == "input" && inputTypes.Contains(e.GetAttribute("type"));
        }
    }
}