using System;
using Coypu.Drivers;

namespace Coypu.Finders
{
    internal class OptionFinder : XPathQueryFinder
    {
        internal OptionFinder(IDriver driver, string locator, DriverScope scope, Options options) : base(driver, locator, scope, options) { }

        public override bool SupportsSubstringTextMatching => true;

        protected override Func<string, Options, string> GetQuery(Html html)
        {
            return html.Option;
        }

        internal override string QueryDescription => "option: " + Locator;
    }
}