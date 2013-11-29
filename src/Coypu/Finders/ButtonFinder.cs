using System;
using Coypu.Drivers;

namespace Coypu.Finders
{
    internal class ButtonFinder : XPathQueryFinder
    {
        internal ButtonFinder(Driver driver, string locator, DriverScope scope) : base(driver, locator, scope) { }

        protected override Func<string, bool, string> GetQuery(XPath xpath)
        {
            return xpath.Button;
        }

        internal override string QueryDescription
        {
            get { return "button: " + Locator; }
        }

        
    }
}