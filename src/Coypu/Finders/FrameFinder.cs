using System.Collections.Generic;

namespace Coypu.Finders
{
    internal class FrameFinder : ElementFinder
    {
        internal FrameFinder(Driver driver, string locator, DriverScope scope, Options options) : base(driver, locator, scope, options) { }

        public override bool SupportsSubstringTextMatching
        {
            get { return true; }
        }

        public override IEnumerable<Element> Find(Options options)
        {
            return Driver.FindFrames(Locator, Scope, options);
        }

        public override string QueryDescription
        {
            get { return "frame: " + Locator; }
        }

    }
}