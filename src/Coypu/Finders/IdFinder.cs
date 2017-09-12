using System;
using System.Collections.Generic;
using System.ComponentModel;
using Coypu.Drivers;

namespace Coypu.Finders
{
    internal class IdFinder : XPathQueryFinder
    {
        internal IdFinder(Driver driver, string locator, DriverScope scope, Options options) : base(driver, locator, scope, options) { }

        public override bool SupportsSubstringTextMatching
        {
            get { return false; }
        }

        protected override Func<string, Options, string> GetQuery(Html html)
        {
            return html.Id;
        }

        public override string QueryDescription
        {
            get { return "id: " + Locator; }
        }
    }
}