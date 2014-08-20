using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
    internal class SeleniumFrame : SeleniumElement
    {
        private readonly SeleniumWindowManager seleniumWindowManager;

        public SeleniumFrame(IWebElement seleniumElement, IWebDriver selenium, SeleniumWindowManager seleniumWindowManager)
            : base(seleniumElement, selenium)
        {
            this.seleniumWindowManager = seleniumWindowManager;
        }

        private IWebElement FindBody()
        {
            return ((IWebDriver)Native).FindElement(By.CssSelector("body"));
        }

        public override string Text
        {
            get
            {
                return FindBody().Text;
            }
        }

        public override string OuterHTML
        {
            get
            {
                return FindBody().GetAttribute("outerHTML");
            }
        }

        public override string InnerHTML
        {
            get
            {
                return FindBody().GetAttribute("innerHTML");
            }
        }

        public override object Native
        {
            get
            {
                seleniumWindowManager.SwitchToFrame(native);
                return selenium;
            }
        }
    }
}