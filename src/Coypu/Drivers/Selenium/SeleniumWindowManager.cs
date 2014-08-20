using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
    internal class SeleniumWindowManager
    {
        private readonly IWebDriver webDriver;
        private bool switchedToFrame;

        public bool SwitchedToFrame
        {
            get { return switchedToFrame; }
        }

        public SeleniumWindowManager(IWebDriver webDriver)
        {
            this.webDriver = webDriver;
        }

        public IWebDriver SwitchToFrame(IWebElement webElement)
        {
            var frame = webDriver.SwitchTo().Frame(webElement);
            switchedToFrame = true;
            return frame;
        }

        public void SwitchToWindow(string windowName)
        {
            if (GetCurrentWindowHandle() != windowName || switchedToFrame)
                webDriver.SwitchTo().Window(windowName);
            
            switchedToFrame = false;
        }

        private string GetCurrentWindowHandle()
        {
            try
            {
                return webDriver.CurrentWindowHandle;
            }
            catch (NoSuchWindowException)
            {
                return string.Empty;
            }
        }

    }
}