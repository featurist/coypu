using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
    internal class MouseControl
    {
        private readonly IWebDriver _selenium;

        public MouseControl(IWebDriver selenium)
        {
            _selenium = selenium;
        }

        public void Hover(Element element)
        {
            var sequenceBuilder = new OpenQA.Selenium.Interactions.Actions(_selenium);
            var actionSequenceBuilder = sequenceBuilder.MoveToElement((IWebElement) element.Native);
            var action = actionSequenceBuilder.Build();
            action.Perform();
        }
    }
}