using System;
using Coypu.Drivers;

namespace Coypu.Finders
{
    internal class SelectFinder : XPathQueryFinder
    {
        internal SelectFinder(Driver driver, string locator, DriverScope scope, Options options) : base(driver, locator, scope, options) { }

        public override bool SupportsSubstringTextMatching
        {
            get { return true; }
        }

        protected override Func<string, Options, string> GetQuery(Html html)
        {
            return html.Select;
        }

        public override string QueryDescription
        {
            get { return "select: " + Locator; }
        }
    }
}