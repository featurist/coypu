using System;
using System.Linq;
using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
    internal class SeleniumElement : ElementFound
    {
        private readonly IWebElement native;

        protected IWebElement NativeSeleniumElement
        {
            get { return native; }
        }

        public SeleniumElement(IWebElement seleniumElement)
        {
            native = seleniumElement;
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

        public virtual object Native
        {
            get { return native; }
        }

        public bool Stale
        {
            get
            {
                try
                {
                    NativeSeleniumElement.FindElement(By.XPath("."));
                    return !NativeSeleniumElement.Displayed;
                }
                catch(InvalidOperationException)
                {
                    return true;
                }
                catch (StaleElementReferenceException)
                {
                    return true;
                }
            }
        }

        public string this[string attributeName]
        {
            get { return NativeSeleniumElement.GetAttribute(attributeName); }
        }
    }
}