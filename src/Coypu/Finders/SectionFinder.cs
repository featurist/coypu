using System;
using Coypu.Drivers;

namespace Coypu.Finders
{
    internal class SectionFinder : XPathQueryFinder
    {
        internal SectionFinder(Driver driver, string locator, DriverScope scope) : base(driver, locator, scope) { }

        public override bool SupportsPartialTextMatching
        {
            get { return true; }
        }

        internal override string QueryDescription
        {
            get { return "section: " + Locator; }
        }

        protected override Func<string, Options, string> GetQuery(XPath xpath)
        {
            return xpath.Section;
        }
    }
}