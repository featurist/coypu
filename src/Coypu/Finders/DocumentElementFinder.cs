using System.Collections.Generic;

namespace Coypu.Finders
{
    internal class DocumentElementFinder : ElementFinder
    {
        private Element window;

        public DocumentElementFinder(IDriver driver, Options options) : base(driver, "Window", null, options)
        {
        }

        public override bool SupportsSubstringTextMatching
        {
            get { return false; }
        }

        internal override IEnumerable<Element> Find(Options options)
        {
            return new[] { window = (window ?? Driver.Window) };
        }

        internal override string QueryDescription
        {
            get { return "Document Element"; }
        }
    }
}