using System;
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

        public override string Id
        {
            get { return NativeSeleniumElement.GetAttribute("id"); }
        }

        public override string Text
        {
            get { return NativeSeleniumElement.Text; }
        }

        public override string Value
        {
            get { return NativeSeleniumElement.GetAttribute("value"); }
        }

        public override string Name
        {
            get { return NativeSeleniumElement.GetAttribute("name"); }
        }

        public override string SelectedOption
        {
            get
            {
                return NativeSeleniumElement.FindElements(By.TagName("option"))
                    .Where(e => e.Selected)
                    .Select(e => e.Text)
                    .FirstOrDefault();
            }
        }

        public override bool Selected
        {
            get { return NativeSeleniumElement.Selected; }
        }

        public override string this[string attributeName]
        {
            get { return NativeSeleniumElement.GetAttribute(attributeName); }
        }
    }
}