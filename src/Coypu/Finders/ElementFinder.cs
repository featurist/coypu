using System;
using System.Collections.Generic;
using System.Linq;

namespace Coypu.Finders
{
    public abstract class ElementFinder
    {
        protected internal readonly IDriver Driver;
        protected readonly DriverScope Scope;

        protected ElementFinder(IDriver driver, string locator, DriverScope scope, Options options)
        {
            Driver = driver;
            Locator = locator;
            Scope = scope;
            Options = options;
        }

        public Options Options { get; }

        public abstract bool SupportsSubstringTextMatching { get; }

        internal string Locator { get; }

        internal abstract IEnumerable<Element> Find(Options options);

        internal abstract string QueryDescription { get; }

        protected internal virtual Exception GetMissingException()
        {
            return new MissingHtmlException("Unable to find " + QueryDescription);
        }

        internal ElementScope AsScope()
        {
            return new SynchronisedElementScope(this, Scope, Options);
        }

        public override string ToString()
        {
            return QueryDescription;
        }
    }

}