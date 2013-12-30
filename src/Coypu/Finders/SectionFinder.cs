using System;
using Coypu.Drivers;

namespace Coypu.Finders
{
    internal class SectionFinder : XPathQueryFinder
    {
        internal SectionFinder(Driver driver, string locator, DriverScope scope, Options options) : base(driver, locator, scope, options) { }

        public override bool SupportsPartialTextMatching
        {
            get { return true; }
        }

        internal override string QueryDescription
        {
            get { return "section: " + Locator; }
        }

        protected override Func<string, Options, string> GetQuery(Html html)
        {
            return html.Section;
        }
    }
}