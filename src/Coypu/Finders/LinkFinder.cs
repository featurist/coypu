using System.Collections.Generic;

namespace Coypu.Finders
{
    internal class LinkFinder : ElementFinder
    {
        internal LinkFinder(Driver driver, string locator, DriverScope scope, Options options) : base(driver, locator, scope, options) { }


        public override bool SupportsPartialTextMatching
        {
            get { return true; }
        }

        internal override IEnumerable<ElementFound> Find(Options options)
        {
            return Driver.FindLinks(Locator, Scope, options);
        }

        internal override string QueryDescription
        {
            get { return "link: " + Locator; }
        }
    }
}