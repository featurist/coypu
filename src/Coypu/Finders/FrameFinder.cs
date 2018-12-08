using System.Collections.Generic;

namespace Coypu.Finders
{
    internal class FrameFinder : ElementFinder
    {
        internal FrameFinder(IDriver driver, string locator, DriverScope scope, Options options) : base(driver, locator, scope, options) { }

        public override bool SupportsSubstringTextMatching => true;

        internal override IEnumerable<Element> Find(Options options)
        {
            return Driver.FindFrames(Locator, Scope, options);
        }

        internal override string QueryDescription => "frame: " + Locator;
    }
}