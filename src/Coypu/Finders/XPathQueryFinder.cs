using System;
using System.Collections.Generic;
using Coypu.Drivers;

namespace Coypu.Finders
{
    internal abstract class XPathQueryFinder : ElementFinder
    {
        protected XPathQueryFinder(Driver driver, string locator, DriverScope scope, Options options) : base(driver, locator, scope, options) { }
        
        internal override IEnumerable<Element> Find(Options options)
        {
            var html = new Html(Scope.Browser.UppercaseTagNames);
            return Driver.FindAllXPath(GetQuery(html)(Locator, options), Scope, options);
        }

        protected abstract Func<string, Options, string> GetQuery(Html html);

    }
}