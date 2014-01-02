using System;
using Coypu.Drivers;

namespace Coypu.Finders
{
    internal class OptionFinder : ElementFinder
    {
        private readonly string fieldLocator;

        internal OptionFinder(Driver driver, string fieldLocator, string optionLocator, DriverScope scope, Options options) : base(driver, optionLocator, scope, options)
        {
            this.fieldLocator = fieldLocator;
        }

        public override bool SupportsSubstringTextMatching
        {
            get { return true; }
        }

        internal override System.Collections.Generic.IEnumerable<ElementFound> Find(Options options)
        {
            var html = new Html(Scope.Browser.UppercaseTagNames);
            return Driver.FindAllXPath(html.SelectOption(fieldLocator, Locator, options), Scope, options);
        }
        internal override string QueryDescription
        {
            get { return "Field: " + fieldLocator + ", with option: " + Locator; }
        }
    }
}