using System.Collections.Generic;
using System.ComponentModel;

namespace Coypu.Finders
{
    internal class IdFinder : ElementFinder
    {
        internal IdFinder(Driver driver, string locator, DriverScope scope) : base(driver, locator, scope) { }

        public override bool SupportsPartialTextMatching
        {
            get { return false; }
        }

        internal override IEnumerable<ElementFound> Find(Options options)
        {
            return new[] { Driver.FindId(Locator, Scope) };
        }

        internal override string QueryDescription
        {
            get { return "id: " + Locator; }
        }
    }
}