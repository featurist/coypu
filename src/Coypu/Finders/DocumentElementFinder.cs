using System.Collections.Generic;

namespace Coypu.Finders
{
    internal class DocumentElementFinder : ElementFinder
    {
        public DocumentElementFinder(Driver driver) : base(driver, "Window", null, new Options{Match = Match.First})
        {
        }

        public override bool SupportsPartialTextMatching
        {
            get { return false; }
        }

        internal override IEnumerable<ElementFound> Find(Options options)
        {
            return new [] {Driver.Window};
        }

        internal override string QueryDescription
        {
            get { return "Document Element"; }
        }
    }
}