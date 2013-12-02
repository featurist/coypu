using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Coypu.Drivers;

namespace Coypu.Finders
{
    internal class FrameFinder : ElementFinder
    {
        internal FrameFinder(Driver driver, string locator, DriverScope scope) : base(driver, locator, scope) { }

        public override bool SupportsPartialTextMatching
        {
            get { return true; }
        }

        internal override IEnumerable<ElementFound> Find(Options options)
        {
            return Driver.FindFrames(Locator, Scope, options.Exact);
        }

        internal override string QueryDescription
        {
            get { return "frame: " + Locator; }
        }

    }
}