using System.Collections.Generic;

namespace Coypu.Finders
{
    internal class XPathFinder : ElementFinder
    {
        internal XPathFinder(Driver driver, string locator, DriverScope scope, Options options) : base(driver, locator, scope, options) { }

        public override bool SupportsSubstringTextMatching
        {
            get { return false; }
        }

        internal override IEnumerable<Element> Find(Options options)
        {
            return Driver.FindAllXPath(Locator, Scope, options);
        }

        internal override string QueryDescription
        {
            get { return "xpath: " + Locator; }
        }
    }
}