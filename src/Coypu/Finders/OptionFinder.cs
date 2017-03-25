using System;
using Coypu.Drivers;

namespace Coypu.Finders
{
    internal class OptionFinder : XPathQueryFinder
    {
        internal OptionFinder(Driver driver, string locator, DriverScope scope, Options options) : base(driver, locator, scope, options) { }

        public override bool SupportsSubstringTextMatching
        {
            get { return true; }
        }

        protected override Func<string, Options, string> GetQuery(Html html)
        {
            return html.Option;
        }

        public override string QueryDescription
        {
            get { return "option: " + Locator; }
        }
    }
}