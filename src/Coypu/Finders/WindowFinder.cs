using System;
using System.Collections.Generic;

namespace Coypu.Finders
{
    internal class WindowFinder : ElementFinder
    {
        internal WindowFinder(Driver driver, string locator, DriverScope scope, Options options) : base(driver, locator, scope, options) { }

        public override bool SupportsSubstringTextMatching
        {
            get { return true; }
        }

        public override IEnumerable<Element> Find(Options options)
        {
            return Driver.FindWindows(Locator, Scope, options);
        }

        public override string QueryDescription
        {
            get { return "window: " + Locator; }
        }

        public override Exception GetMissingException()
        {
            return new MissingWindowException("Unable to find " + QueryDescription);
        }
    }
}