using System;
using Coypu.Drivers;

namespace Coypu.Finders
{
    internal class SectionFinder : XPathQueryFinder
    {
        internal SectionFinder(IDriver driver, string locator, DriverScope scope, Options options) : base(driver, locator, scope, options) { }

        public override bool SupportsSubstringTextMatching => true;

        internal override string QueryDescription => "section: " + Locator;

        protected override Func<string, Options, string> GetQuery(Html html)
        {
            return html.Section;
        }
    }
}