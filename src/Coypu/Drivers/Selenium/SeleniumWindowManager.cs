using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
    internal class SeleniumWindowManager
    {
        private readonly IWebDriver webDriver;
        private IWebElement switchedToFrameElement;
        private IWebDriver switchedToFrame;
        private string lastKnownWindowHandle;

        public bool SwitchedToAFrame
        {
            get { return switchedToFrame != null; }
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
            if (Equals(switchedToFrameElement, webElement))
                return switchedToFrame;

            var frame = webDriver.SwitchTo().Frame(webElement);

            switchedToFrameElement = webElement;
            switchedToFrame = frame;

            return frame;
        }

        public void SwitchToWindow(string windowName)
        {
            if (lastKnownWindowHandle != windowName || SwitchedToAFrame)
            {
                webDriver.SwitchTo().Window(windowName);
                lastKnownWindowHandle = windowName;
            }

            switchedToFrame = null;
            switchedToFrameElement = null;
        }
    }
}