using System;
using System.Collections.Generic;
using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
    internal class WindowHandleFinder
    {
        private readonly SeleniumWindowManager _seleniumWindowManager;
        private readonly IWebDriver _webDriver;

        public WindowHandleFinder(IWebDriver webDriver,
                                  SeleniumWindowManager seleniumWindowManager)
        {
            _webDriver = webDriver;
            _seleniumWindowManager = seleniumWindowManager;
        }

        public IEnumerable<string> FindWindowHandles(string titleOrName,
                                                     Options options)
        {
            var currentHandle = GetCurrentWindowHandle();
            IList<string> matchingWindowHandles = new List<string>();

            try
            {
                _seleniumWindowManager.SwitchToWindow(titleOrName);
                matchingWindowHandles.Add(_webDriver.CurrentWindowHandle);
            }
            catch (NoSuchWindowException)
            {
                foreach (var windowHandle in _webDriver.WindowHandles)
                {
                    _seleniumWindowManager.SwitchToWindow(windowHandle);
                    if (options.TextPrecisionExact)
                    {
                        if (ExactMatch(titleOrName, windowHandle))
                            matchingWindowHandles.Add(windowHandle);
                    }
                    else
                    {
                        if (SubstringMatch(titleOrName))
                            matchingWindowHandles.Add(windowHandle);
                    }
                }
            }

            try
            {
                _seleniumWindowManager.SwitchToWindow(currentHandle);
            }
            catch (NoSuchWindowException ex)
            {
                throw new MissingWindowException("The active window was closed.", ex);
            }

            return matchingWindowHandles;
        }

        private bool SubstringMatch(string titleOrName)
        {
            return _webDriver.Title.Contains(titleOrName);
        }

        private bool ExactMatch(string titleOrName,
                                string windowHandle)
        {
            return windowHandle == titleOrName || _webDriver.Title == titleOrName;
        }

        public string GetCurrentWindowHandle()
        {
            try
            {
                return _webDriver.CurrentWindowHandle;
            }
            catch (NoSuchWindowException) { }
            catch (InvalidOperationException) { }

            return null;
        }
    }
}
