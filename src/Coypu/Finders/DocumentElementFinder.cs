using System.Collections.Generic;

namespace Coypu.Finders
{
    internal class DocumentElementFinder : ElementFinder
    {
        public DocumentElementFinder(Driver driver, Options options) : base(driver, "Window", null, options)
        {
        }

        public override bool SupportsSubstringTextMatching
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