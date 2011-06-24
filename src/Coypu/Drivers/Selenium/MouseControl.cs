using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;

namespace Coypu.Drivers.Selenium
{
    public class MouseControl
    {
        private readonly IWebDriver selenium;

        public MouseControl(IWebDriver selenium)
        {
            this.selenium = selenium;
        }

        public void Hover2(Element element)
        {
            var sequenceBuilder = new DefaultActionSequenceBuilder(selenium);
            var actionSequenceBuilder = sequenceBuilder.MoveToElement((IWebElement) element.Native);
            var action = actionSequenceBuilder.Build();
            action.Perform();
        }
    }
}