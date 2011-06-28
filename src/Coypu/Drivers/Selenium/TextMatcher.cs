using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
    internal class TextMatcher
    {
        public bool TextMatches(IWebElement e, string locator) 
        {
            return e.Text.Trim() == locator.Trim();
        }
    }
}