using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;

namespace Coypu.Drivers.Selenium
{
    internal class SeleniumWindowManager
    {
        private readonly IWebDriver _webDriver;
        private IWebDriver _switchedToFrame;
        private IWebElement _switchedToFrameElement;

        public SeleniumWindowManager(IWebDriver webDriver)
        {
            _webDriver = webDriver;
        }

        public bool SwitchedToAFrame => _switchedToFrame != null;

        public string LastKnownWindowHandle { get; private set; }

        public IWebDriver SwitchToFrame(IWebElement webElement)
        {
            if (Equals(_switchedToFrameElement, webElement))
                return _switchedToFrame;

            var frame = _webDriver.SwitchTo()
                                  .Frame(webElement);

            // Fix for https://bugzilla.mozilla.org/show_bug.cgi?id=1305822 
            if (_webDriver is FirefoxDriver)
            {
                _webDriver.SwitchTo()
                    .DefaultContent();
            }

            _switchedToFrameElement = webElement;
            _switchedToFrame = frame;

            return frame;
        }

        public void SwitchToWindow(string windowName)
        {
            if (LastKnownWindowHandle != windowName || SwitchedToAFrame)
            {
                _webDriver.SwitchTo()
                          .Window(windowName);
                LastKnownWindowHandle = windowName;
            }

            _switchedToFrame = null;
            _switchedToFrameElement = null;
        }
    }
}