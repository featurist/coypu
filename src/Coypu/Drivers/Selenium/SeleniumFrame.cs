using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Coypu.Drivers.Selenium
{
    internal class SeleniumFrame : SeleniumElement
    {
        private readonly IWebDriver selenium;

        public SeleniumFrame(IWebElement seleniumElement, IWebDriver selenium)
            : base(seleniumElement)
        {
            this.selenium = selenium;
        }

        public override object Native
        {
            get
            {
                selenium.SwitchTo().Frame(NativeSeleniumElement);
                return selenium;
            }
        }
    }
}