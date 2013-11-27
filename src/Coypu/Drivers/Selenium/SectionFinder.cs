using OpenQA.Selenium;

namespace Coypu.Drivers.Selenium
{
    internal class SectionFinder
    {
        private readonly ElementFinder elementFinder;

        readonly string[] sectionTags = { "section", "div"};
        readonly string[] headerTags = { "h1", "h2", "h3", "h4", "h5", "h6" };
        private readonly XPath xPath;

        public SectionFinder(ElementFinder elementFinder, XPath xPath)
        {
            this.elementFinder = elementFinder;
            this.xPath = xPath;
        }

        public IWebElement FindSection(string locator, Scope scope)
        {
            var xpath = xPath.Format(".//*[" + xPath.TagNamedOneOf(sectionTags) + " and (" +
                                        "./*[" + xPath.TagNamedOneOf(headerTags) + " and normalize-space() = {0} ] or " +
                                        "@id = {0}" + 
                                     ")]",locator.Trim());
           
            return elementFinder.Find(By.XPath(xpath),scope, "section: " + locator);
        }
    }
}