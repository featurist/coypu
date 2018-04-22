using System;
using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
    internal class SeleniumWindow : Element
    {
        private readonly SeleniumWindowManager _seleniumWindowManager;
        private readonly IWebDriver _webDriver;
        private readonly string _windowHandle;

        public SeleniumWindow(IWebDriver webDriver,
                              string windowHandle,
                              SeleniumWindowManager seleniumWindowManager)
        {
            _webDriver = webDriver;
            _windowHandle = windowHandle;
            _seleniumWindowManager = seleniumWindowManager;
        }

        public string Id => throw new NotSupportedException();

        public string Text => ((ISearchContext) Native).FindElement(By.CssSelector("body"))
                                                       .Text;

        public string InnerHTML => ((ISearchContext) Native).FindElement(By.XPath("./*"))
                                                            .GetAttribute("innerHTML");

        public string Title => _webDriver.Title;

        public bool Disabled => throw new NotSupportedException();

        public string OuterHTML => ((ISearchContext) Native).FindElement(By.XPath("./*"))
                                                            .GetAttribute("outerHTML");

        public string Value => throw new NotSupportedException();

        public string Name => throw new NotSupportedException();

        public string SelectedOption => throw new NotSupportedException();

        public bool Selected => throw new NotSupportedException();

        public object Native
        {
            get
            {
                SwitchTo(_windowHandle);
                return _webDriver;
            }
        }

        public string this[string attributeName] => throw new NotSupportedException();

        private void SwitchTo(string windowName)
        {
            _seleniumWindowManager.SwitchToWindow(windowName);
        }
    }
}