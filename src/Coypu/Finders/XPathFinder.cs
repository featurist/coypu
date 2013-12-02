using System.Collections.Generic;

namespace Coypu.Finders
{
    internal class XPathFinder : ElementFinder
    {
        internal XPathFinder(Driver driver, string locator, DriverScope scope) : base(driver, locator, scope) { }

        public override bool SupportsPartialTextMatching
        {
            get { return false; }
        }

        internal override IEnumerable<ElementFound> Find(Options options)
        {
            return Driver.FindAllXPath(Locator, Scope);
        }

        internal override string QueryDescription
        {
            get { return "xpath: " + Locator; }
        }
    }
}