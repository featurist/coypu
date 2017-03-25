using System.Collections.Generic;

namespace Coypu.Finders
{
    internal class DocumentElementFinder : ElementFinder
    {
        private Element window;

        public DocumentElementFinder(Driver driver, Options options) : base(driver, "Window", null, options)
        {
        }

        public override bool SupportsSubstringTextMatching
        {
            get { return false; }
        }

        public override IEnumerable<Element> Find(Options options)
        {
            return new[] { window = (window ?? Driver.Window) };
        }

        public override string QueryDescription
        {
            get { return "Document Element"; }
        }
    }
}