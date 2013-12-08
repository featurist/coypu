using System.Collections.Generic;
using System.Linq;

namespace Coypu.Finders
{
    public abstract class ElementFinder
    {
        protected readonly Driver Driver;
        private readonly string locator;
        protected readonly DriverScope Scope;
        private readonly Options options;

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

        internal abstract IEnumerable<ElementFound> Find(Options options);

        internal abstract string QueryDescription { get; }

        internal ElementFound ResolveQuery()
        {
            var results = Find(Options.Merge(new Options { Exact = true }, Options));
            if (Options.Match == Match.First && results.Any())
                return results.First();

            var count = results.Count();

            if (ShouldTryPartialMatch(Options, count))
                results = Find(Options.Merge(new Options {Exact = false}, Options));

            count = results.Count();
            if (count > 1)
                throw new AmbiguousHtmlException(Options.BuildAmbiguousMessage(QueryDescription, count));

            if (count == 0)
                throw new MissingHtmlException("Unable to find " + QueryDescription);

            return results.First();
        }

        private bool ShouldTryPartialMatch(Options options, int count)
        {
            return SupportsPartialTextMatching &&
                   options.Match == Match.Single &&
                   count == 0 &&
                   !options.Exact;
        }
    }
}