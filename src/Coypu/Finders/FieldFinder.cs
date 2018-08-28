using System;
using Coypu.Drivers;

namespace Coypu.Finders
{
    internal class FieldFinder : XPathQueryFinder
    {
        internal FieldFinder(Driver driver, string locator, DriverScope scope, Options options) : base(driver, locator, scope, options) { }

        public override bool SupportsSubstringTextMatching
        {
            get { return true; }
        }

        protected override Func<string, Options, string> GetQuery(Html html)
        {
            if (options.UseExtendedTextLocators)
                return html.FieldExtended;
            else
                return html.Field;
        }

        internal override string QueryDescription
        {
            get { return "field: " + Locator; }
        }
    }
}