using System.Collections.Generic;
using System.Linq;

namespace Coypu.Finders
{
    public abstract class ElementFinder
    {
        protected readonly Driver Driver;
        private readonly string _locator;
        protected readonly DriverScope Scope;

        protected ElementFinder(Driver driver, string locator, DriverScope scope)
        {
            Driver = driver;
            _locator = locator;
            Scope = scope;
        }

        public abstract bool SupportsPartialTextMatching { get; }

        internal string Locator { get { return _locator; } }

        internal abstract IEnumerable<ElementFound> Find(Options options);

        internal abstract string QueryDescription { get; }

        internal ElementFound ResolveQuery()
        {
            var options = Scope.Options;
            var results = Find(new Options { Exact = true }.Merge(Scope.Options));
            if (options.Match == Match.First && results.Any())
                return results.First();

            var count = results.Count();

            if (ShouldTryPartialMatch(options, count))
                results = Find(new Options {Exact = false}.Merge(options));

            count = results.Count();
            if (count > 1)
                throw new AmbiguousHtmlException(options.BuildAmbiguousMessage(QueryDescription, count));

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