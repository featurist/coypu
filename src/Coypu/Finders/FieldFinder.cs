using System;
using Coypu.Drivers;

namespace Coypu.Finders
{
    internal class FieldFinder : XPathQueryFinder
    {
        internal FieldFinder(Driver driver, string locator, DriverScope scope, Options options) : base(driver, locator, scope, options) { }

        public override bool SupportsPartialTextMatching
        {
            get { return true; }
        }

        protected override Func<string, Options, string> GetQuery(Html html)
        {
            return html.Field;
        }

        internal override string QueryDescription
        {
            get { return "field: " + Locator; }
        }
    }

    internal class OptionFinder : XPathQueryFinder
    {
        internal OptionFinder(Driver driver, string locator, DriverScope scope, Options options) : base(driver, locator, scope, options) { }

        public override bool SupportsPartialTextMatching
        {
            get { return true; }
        }

        protected override Func<string, Options, string> GetQuery(Html html)
        {
            return html.Option;
        }

        internal override string QueryDescription
        {
            get { return "Option: " + Locator; }
        }
    }
}