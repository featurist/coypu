using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
    internal class SeleniumFrame : SeleniumElement
    {
        private readonly SeleniumWindowManager _seleniumWindowManager;

        public SeleniumFrame(IWebElement seleniumElement,
                             IWebDriver selenium,
                             SeleniumWindowManager seleniumWindowManager)
            : base(seleniumElement, selenium)
        {
            _seleniumWindowManager = seleniumWindowManager;
        }

        public override string Text => FindBody()
            .Text;

        public override string OuterHTML => FindBody()
            .GetAttribute("outerHTML");

        public override string InnerHTML => FindBody()
            .GetAttribute("innerHTML");

        public override object Native
        {
            get
            {
                _seleniumWindowManager.SwitchToFrame(_native);
                return Selenium;
            }
        }

        private IWebElement FindBody()
        {
            return ((IWebDriver) Native).FindElement(By.CssSelector("body"));
        }
    }
}
