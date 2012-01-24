using System;

namespace Coypu
{
    public class LinkFinder : ElementFinder
    {
        private readonly Driver driver;
        private readonly string locator;

        public LinkFinder(Driver driver, string locator)
        {
            this.driver = driver;
            this.locator = locator;
        }

        public Element Find()
        {           
                return driver.FindLink(locator);
            
        }

        public TimeSpan Timeout
        {
            get; set;
        }
    }
}