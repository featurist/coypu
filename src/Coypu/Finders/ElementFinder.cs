using System;
using System.Collections.Generic;
using System.Linq;

namespace Coypu.Finders
{
    public abstract class ElementFinder
    {
        protected internal readonly Driver Driver;
        private readonly string locator;
        protected readonly DriverScope Scope;
        protected readonly Options options;

        protected ElementFinder(Driver driver, string locator, DriverScope scope, Options options)
        {
            Driver = driver;
            this.locator = locator;
            Scope = scope;
            this.options = options;
        }

        public Options Options
        {
            get { return options; }
        }

        public abstract bool SupportsPartialTextMatching { get; }

        internal string Locator { get { return locator; } }

        internal abstract IEnumerable<ElementFound> Find(Options options = null);

        internal abstract string QueryDescription { get; }

        protected internal virtual Exception GetMissingException()
        {
            return new MissingHtmlException("Unable to find " + QueryDescription);
        }

        internal ElementScope AsScope()
        {
            return new RobustElementScope(this, Scope, options);
        }
    }

}