using System;
using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
    internal class SeleniumWindow : Element
    {
        private readonly IWebDriver webDriver;
        private readonly string windowHandle;
        private readonly SeleniumWindowManager seleniumWindowManager;

        public SeleniumWindow(IWebDriver webDriver, string windowHandle, SeleniumWindowManager seleniumWindowManager)
        {
            this.webDriver = webDriver;
            this.windowHandle = windowHandle;
            this.seleniumWindowManager = seleniumWindowManager;
        }

        public string Id
        {
            get { throw new NotSupportedException(); }
        }

        public string Text
        {
            get
            {
                return ((ISearchContext)Native).FindElement(By.CssSelector("body")).Text;
            }
        }

        public string InnerHTML
        {
            get
            {
                return ((ISearchContext)Native).FindElement(By.XPath("./*")).GetAttribute("innerHTML");
            }
        }

        public string Title
        {
            get { return webDriver.Title; }
        }

        public bool Disabled
        {
            get { throw new NotSupportedException(); }
        }

        public string OuterHTML
        {
            get
            {
                return ((ISearchContext)Native).FindElement(By.XPath("./*")).GetAttribute("outerHTML");
            }
        }

        public string Value
        {
            get { throw new NotSupportedException(); }
        }

        public string Name
        {
            get { throw new NotSupportedException(); }
        }

        public string SelectedOption
        {
            get { throw new NotSupportedException(); }
        }

        public bool Selected
        {
            get { throw new NotSupportedException(); }
        }

        public object Native
        {
            get
            {
                SwitchTo(windowHandle);
                return webDriver;
            }
        }

        private void SwitchTo(string windowName)
        {
            seleniumWindowManager.SwitchToWindow(windowName);
        }

        public string this[string attributeName]
        {
            get { throw new NotSupportedException(); }
        }
    }
}