using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
    internal class SeleniumFrame : SeleniumElement
    {
        public SeleniumFrame(IWebElement seleniumElement, IWebDriver selenium) : base(seleniumElement, selenium)
        {
        }

        public override string Text
        {
            get
            {
                selenium.SwitchTo().Frame(native);
                return selenium.FindElement(By.CssSelector("body")).Text;
            }
        }

        public override string OuterHTML
        {
            get
            {
                selenium.SwitchTo().Frame(native);
                return selenium.FindElement(By.CssSelector("body")).GetAttribute("outerHTML");
            }
        }

        public override string InnerHTML
        {
            get
            {
                selenium.SwitchTo().Frame(native);
                return selenium.FindElement(By.CssSelector("body")).GetAttribute("innerHTML");
            }
        }

        public override object Native
        {
            get
            {
                selenium.SwitchTo().Frame(native);
                return selenium;
            }
        }
    }
}