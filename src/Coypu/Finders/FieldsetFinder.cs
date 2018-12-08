using System;
using System.Collections.Generic;
using Coypu.Drivers;

namespace Coypu.Finders
{
    internal class FieldsetFinder : XPathQueryFinder
    {
        internal FieldsetFinder(IDriver driver, string locator, DriverScope scope, Options options) : base(driver, locator, scope, options) { }

        public override bool SupportsSubstringTextMatching => true;

        internal override string QueryDescription => "fieldset: " + Locator;

        protected override Func<string, Options, string> GetQuery(Html html)
        {
            return html.Fieldset;
        }
    }
}