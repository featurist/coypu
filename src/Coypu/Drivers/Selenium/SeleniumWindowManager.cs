using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
    internal class SeleniumWindowManager
    {
        private readonly IWebDriver webDriver;
        private bool switchedToFrame;
        private string lastKnownWindowHandle;

        public bool SwitchedToFrame
        {
            get { return switchedToFrame; }
        }

        public string LastKnownWindowHandle
        {
           get { return lastKnownWindowHandle; } 
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
            if (lastKnownWindowHandle != windowName || switchedToFrame)
            {
                webDriver.SwitchTo().Window(windowName);
                lastKnownWindowHandle = windowName;
            }
            
            switchedToFrame = false;
        }
    }
}