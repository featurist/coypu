using System;
using Coypu.Drivers;

namespace Coypu.Finders
{
    internal class SelectFinder : XPathQueryFinder
    {
        internal SelectFinder(IDriver driver, string locator, DriverScope scope, Options options) : base(driver, locator, scope, options) { }

        public override bool SupportsSubstringTextMatching => true;

        protected override Func<string, Options, string> GetQuery(Html html)
        {
            return html.Select;
        }

        internal override string QueryDescription => "select: " + Locator;
    }
}