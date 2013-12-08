using System;
using System.Collections.Generic;
using Coypu.Drivers;

namespace Coypu.Finders
{
    internal abstract class XPathQueryFinder : ElementFinder
    {
        protected XPathQueryFinder(Driver driver, string locator, DriverScope scope, Options options) : base(driver, locator, scope, options) { }
        
        internal override IEnumerable<ElementFound> Find(Options options)
        {
            var xpath = new XPath(Scope.Browser.UppercaseTagNames);
            return Driver.FindAllXPath(GetQuery(xpath)(Locator, options), Scope, options);
        }

        protected abstract Func<string, Options, string> GetQuery(XPath xpath);

    }
}