using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Coypu.Finders
{
    internal class XPathFinder : WithTextFinder
    {
        protected override string SelectorType
        {
            get { return "xpath"; }
        }

        public XPathFinder(Driver driver, string locator, DriverScope scope, Options options)
            : base(driver, locator, scope, options)
        {
        }

        public XPathFinder(Driver driver, string locator, DriverScope scope, Options options, Regex textPattern)
            : base(driver, locator, scope, options, textPattern)
        {
        }

        public XPathFinder(Driver driver, string locator, DriverScope scope, Options options, string text)
            : base(driver, locator, scope, options, text)
        {
        }

        public override bool SupportsSubstringTextMatching
        {
            get { return true; }
        }

        internal override IEnumerable<Element> Find(Options options)
        {
            return Driver.FindAllXPath(Locator, Scope, options, TextPattern(options.TextPrecision == TextPrecision.Exact));
        }
    }
}