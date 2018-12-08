using System;
using Coypu.Drivers;

namespace Coypu.Finders
{
    internal class ButtonFinder : XPathQueryFinder
    {
        internal ButtonFinder(IDriver driver, string locator, DriverScope scope, Options options) : base(driver, locator, scope, options) { }

        public override bool SupportsSubstringTextMatching => true;

        protected override Func<string, Options, string> GetQuery(Html html)
        {
            return html.Button;
        }

        internal override string QueryDescription => "button: " + Locator;
    }
}