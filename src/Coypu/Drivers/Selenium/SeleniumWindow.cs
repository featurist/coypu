using System;
using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
    internal class SeleniumWindow : ElementFound
    {
        private readonly IWebDriver webDriver;
        private readonly string windowHandle;

        public SeleniumWindow(IWebDriver webDriver, string windowHandle)
        {
            this.webDriver = webDriver;
            this.windowHandle = windowHandle;
        }

        public string Id
        {
            get { throw new System.NotSupportedException(); }
        }

        public string Text
        {
            get
            {
                return RetainingCurrentWindow(() => ((ISearchContext)Native).FindElement(By.CssSelector("body")).Text);
            }
        }

        public string InnerHTML
        {
            get
            {
                return RetainingCurrentWindow(() => ((ISearchContext)Native).FindElement(By.XPath("./*")).GetAttribute("innerHTML"));
            }
        }

        public string Title
        {
            get { return RetainingCurrentWindow(() => webDriver.Title); }
        }

        public string OuterHTML
        {
            get
            {
                return RetainingCurrentWindow(() => ((ISearchContext)Native).FindElement(By.XPath("./*")).GetAttribute("outerHTML"));
            }
        }

        private T RetainingCurrentWindow<T>(Func<T> function)
        {
            var currentWindowHandle = webDriver.CurrentWindowHandle;
            try
            {
                return function();
            }
            finally
            {
                SwitchTo(currentWindowHandle);
            }
        }

        public string Value
        {
            get { throw new System.NotSupportedException(); }
        }

        public string Name
        {
            get { throw new System.NotSupportedException(); }
        }

        public string SelectedOption
        {
            get { throw new System.NotSupportedException(); }
        }

        public bool Selected
        {
            get { throw new System.NotSupportedException(); }
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
            webDriver.SwitchTo().Window(windowName);
        }

        public bool Stale(Options options)
        {
            return !webDriver.WindowHandles.Contains(windowHandle);
        }

        public string this[string attributeName]
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}