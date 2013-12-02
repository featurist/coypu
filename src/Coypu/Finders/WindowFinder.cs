using System.Collections.Generic;

namespace Coypu.Finders
{
    internal class WindowFinder : ElementFinder
    {
        internal WindowFinder(Driver driver, string locator, DriverScope scope) : base(driver, locator, scope) { }

        public override bool SupportsPartialTextMatching
        {
            get { return true; }
        }

        internal override IEnumerable<ElementFound> Find(Options options)
        {
            return Driver.FindWindows(Locator, Scope, options.Exact);
        }

        internal override string QueryDescription
        {
            get { return "window: " + Locator; }
        }
    }
}