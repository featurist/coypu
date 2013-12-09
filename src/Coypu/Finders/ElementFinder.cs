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
            var exactQuery = Find(Options.Merge(new Options {Exact = true}, options));
            var results = new ElementFound[]{};
            if (options.Match == Match.First)
            {
                var first = exactQuery.FirstOrDefault();
                if (first != null)
                    return first;
            }
            else
            {
                results = exactQuery.ToArray();
            }

            var count = results.Count();

            if (ShouldTryPartialMatch(options, count))
            {
                var partialQuery = Find(Options.Merge(new Options { Exact = false }, options));
                if (options.Match == Match.First)
                {
                    var first = partialQuery.FirstOrDefault();
                    if (first != null)
                        return first;
                }
                else
                {
                    results = partialQuery.ToArray();
                }
            }
                
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
                   count == 0 &&
                   !options.Exact;
        }
    }
}