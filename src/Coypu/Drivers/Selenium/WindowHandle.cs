using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Coypu.Drivers.Selenium
{
    internal class WindowHandle : ElementFound
    {
        private readonly RemoteWebDriver selenium;
        private readonly string windowHandle;

        public WindowHandle(RemoteWebDriver selenium, string windowHandle)
        {
            this.selenium = selenium;
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
                var currentWindowHandle = selenium.CurrentWindowHandle;
                try
                {
                    return ((ISearchContext)Native).FindElement(By.CssSelector("body")).Text;
                }
                finally
                {
                    selenium.SwitchTo().Window(currentWindowHandle);
                }
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
                selenium.SwitchTo().Window(windowHandle);
                return selenium;
            }
        }

        public bool Stale
        {
            get
            {
                return !selenium.WindowHandles.Contains(windowHandle);
            }
        }

        public string this[string attributeName]
        {
            get { throw new System.NotImplementedException(); }
        }
    }
}