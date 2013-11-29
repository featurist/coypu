using System;
using System.Collections.Generic;
using Coypu.Drivers;

namespace Coypu.Finders
{
    internal abstract class XPathQueryFinder : QueryFinder
    {
        protected XPathQueryFinder(Driver driver, string locator, DriverScope scope) : base(driver, locator, scope)
        {
        }

        internal override IEnumerable<ElementFound> FindAll(bool exact)
        {
            var xpath = new XPath(Scope.Browser.UppercaseTagNames);
            return Driver.FindAllXPath(GetQuery(xpath)(Locator, exact),Scope);
        }

        protected abstract Func<string, bool, string> GetQuery(XPath xpath);

    }
}