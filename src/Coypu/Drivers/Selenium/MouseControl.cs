using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
    internal class MouseControl
    {
        private readonly IWebDriver selenium;

        public MouseControl(IWebDriver selenium)
        {
            this.selenium = selenium;
        }

        public void Hover(Element element)
        {
            var sequenceBuilder = new OpenQA.Selenium.Interactions.Actions(selenium);
            var actionSequenceBuilder = sequenceBuilder.MoveToElement((IWebElement) element.Native);
            var action = actionSequenceBuilder.Build();
            action.Perform();
        }
    }
}