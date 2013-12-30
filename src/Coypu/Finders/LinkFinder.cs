using System;
using System.Collections.Generic;
using Coypu.Drivers;

namespace Coypu.Finders
{
    internal class LinkFinder : XPathQueryFinder
    {
        internal LinkFinder(Driver driver, string locator, DriverScope scope, Options options) : base(driver, locator, scope, options) { }


        public override bool SupportsPartialTextMatching
        {
            get { return true; }
        }

        protected override Func<string, Options, string> GetQuery(Html html)
        {
            return html.Link;
        }

        internal override string QueryDescription
        {
            get { return "link: " + Locator; }
        }
    }
}