using System.Collections.Generic;

namespace Coypu.Finders
{
    internal abstract class QueryFinder : ElementFinder
    {
        protected QueryFinder(Driver driver, string locator, DriverScope scope) : base(driver, locator, scope)
        {
        }

        internal override ElementFound Find()
        {
            return Scope.Find(this);
        }

        internal abstract IEnumerable<ElementFound> FindAll(bool exact);

        internal abstract string QueryDescription { get; }
    }
}