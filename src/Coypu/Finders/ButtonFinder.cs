using System;
using Coypu.Drivers;

namespace Coypu.Finders
{
    internal class ButtonFinder : XPathQueryFinder
    {
        internal ButtonFinder(Driver driver, string locator, DriverScope scope, Options options) : base(driver, locator, scope, options) { }

        public override bool SupportsSubstringTextMatching
        {
            get { return true; }
        }

        protected override Func<string, Options, string> GetQuery(Html html)
        {
            return html.Button;
        }

        public override string QueryDescription
        {
            get { return "button: " + Locator; }
        }

        
    }
}