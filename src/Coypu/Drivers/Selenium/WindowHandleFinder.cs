using System;
using System.Collections.Generic;
using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
    internal class WindowHandleFinder
    {
        private readonly IWebDriver webDriver;
        private readonly SeleniumWindowManager seleniumWindowManager;

        public WindowHandleFinder(IWebDriver webDriver, SeleniumWindowManager seleniumWindowManager)
        {
            this.webDriver = webDriver;
            this.seleniumWindowManager = seleniumWindowManager;
        }

        public IEnumerable<string> FindWindowHandles(string titleOrName, Options options)
        {   
            var currentHandle = GetCurrentWindowHandle();
            IList<string> matchingWindowHandles = new List<string>();

            try
            {
                seleniumWindowManager.SwitchToWindow(titleOrName);
                matchingWindowHandles.Add(webDriver.CurrentWindowHandle);
            }
            catch (NoSuchWindowException)
            {
                foreach (var windowHandle in webDriver.WindowHandles)
                {
                    seleniumWindowManager.SwitchToWindow(windowHandle);
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
                seleniumWindowManager.SwitchToWindow(currentHandle);
            }
            catch (NoSuchWindowException ex)
            {
                throw new MissingWindowException("The active window was closed. Coypu should prevent this by ensuring fresh scope higher up.", ex);
            }
            return matchingWindowHandles;
        }

        private bool SubstringMatch(string titleOrName)
        {
            return webDriver.Title.Contains(titleOrName);
        }

        private bool ExactMatch(string titleOrName, string windowHandle)
        {
            return (windowHandle == titleOrName || webDriver.Title == titleOrName);
        }

        public string GetCurrentWindowHandle()
        {
            try
            {
                return webDriver.CurrentWindowHandle;
            }
            catch (NoSuchWindowException) {}
            catch (InvalidOperationException) {}
            return null;
        }
    }
}