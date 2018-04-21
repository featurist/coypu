using System;
using System.Collections.Generic;
using System.Linq;

namespace Coypu.Finders
{
    public abstract class ElementFinder
    {
        protected internal readonly IDriver Driver;
        private readonly string locator;
        protected readonly DriverScope Scope;
        protected readonly Options options;

        protected ElementFinder(IDriver driver, string locator, DriverScope scope, Options options)
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

        public abstract bool SupportsSubstringTextMatching { get; }

        internal string Locator { get { return locator; } }

        internal abstract IEnumerable<Element> Find(Options options);

        internal abstract string QueryDescription { get; }

        protected internal virtual Exception GetMissingException()
        {
            return new MissingHtmlException("Unable to find " + QueryDescription);
        }

        internal ElementScope AsScope()
        {
            return new SynchronisedElementScope(this, Scope, options);
        }
    }

}