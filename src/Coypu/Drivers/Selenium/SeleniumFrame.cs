using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
    internal class SeleniumFrame : SeleniumElement
    {
        public SeleniumFrame(IWebElement seleniumElement, IWebDriver webDriver) : base(seleniumElement, webDriver)
        {
        }

        public override string Text
        {
            get
            {
                return NativeSeleniumElement.FindElement(By.XPath(".")).Text;
            }
        }
    }
}