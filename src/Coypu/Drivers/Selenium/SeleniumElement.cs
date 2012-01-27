using System.Linq;
using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
    internal class SeleniumElement : Element
    {
        private IWebElement NativeSeleniumElement
        {
            get { return (IWebElement) Native; }
        }

        public SeleniumElement(IWebElement seleniumElement)
        {
            Native = seleniumElement;
        }

        public string Id
        {
            get { return NativeSeleniumElement.GetAttribute("id"); }
        }

        public string Text
        {
            get { return NativeSeleniumElement.Text; }
        }

        public string Value
        {
            get { return NativeSeleniumElement.GetAttribute("value"); }
        }

        public string Name
        {
            get { return NativeSeleniumElement.GetAttribute("name"); }
        }

        public string SelectedOption
        {
            get
            {
                return NativeSeleniumElement.FindElements(By.TagName("option"))
                    .Where(e => e.Selected)
                    .Select(e => e.Text)
                    .FirstOrDefault();
            }
        }

        public bool Selected
        {
            get { return NativeSeleniumElement.Selected; }
        }

        public object Native { get; set; }

        public string this[string attributeName]
        {
            get { return NativeSeleniumElement.GetAttribute(attributeName); }
        }
    }
}