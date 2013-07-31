using System;
using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
    internal class WindowHandleFinder
    {
        private readonly IWebDriver webDriver;

        public WindowHandleFinder(IWebDriver webDriver)
        {
            this.webDriver = webDriver;
        }


        public string FindWindowHandle(string titleOrName)
        {
            var currentHandle = GetCurrentWindowHandle();
            string matchingWindowHandle = null;
            string partiallyMatchingWindowHandle = null;

            try
            {
                webDriver.SwitchTo().Window(titleOrName);
                matchingWindowHandle = webDriver.CurrentWindowHandle;
            }
            catch (NoSuchWindowException)
            {
                foreach (var windowHandle in webDriver.WindowHandles)
                {
                    webDriver.SwitchTo().Window(windowHandle);
                    if (windowHandle == titleOrName || webDriver.Title == titleOrName)
                    {
                        matchingWindowHandle = windowHandle;
                        break;
                    }
                    if (webDriver.Title.Contains(titleOrName))
                        partiallyMatchingWindowHandle = windowHandle;
                }
            }
            matchingWindowHandle = matchingWindowHandle ?? partiallyMatchingWindowHandle;
            if (matchingWindowHandle == null)
                throw new MissingWindowException("No such window found: " + titleOrName);

            webDriver.SwitchTo().Window(currentHandle);
            return matchingWindowHandle;
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