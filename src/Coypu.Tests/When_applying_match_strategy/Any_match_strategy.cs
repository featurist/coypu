using System;
using System.Collections.Generic;
using Coypu.Finders;

namespace Coypu.Tests.When_applying_match_strategy
{
    public abstract class Any_match_strategy
    {
        public abstract Match Match {get;}
        public abstract TextPrecision TextPrecision {get;}

        public Options FinderOptions()
        {
            return new Options
                {
                    Match = this.Match,
                    TextPrecision = this.TextPrecision,
                    // Some other stuff to check they are merged
                    ConsiderInvisibleElements = true,
                    RetryInterval = TimeSpan.FromMilliseconds(4321)
                };
        }

        protected static void StubExactResults(StubElementFinder finder, Options finderOptions, IEnumerable<Element> exactResults)
        {
            finder.StubbedFindResults[Options.Merge(Options.Exact,finderOptions)] = exactResults;
        }

        protected static void StubSubstringResults(StubElementFinder finder, Options finderOptions, IEnumerable<Element> exactResults)
        {
            finder.StubbedFindResults[Options.Merge(Options.Substring, finderOptions)] = exactResults;
        }

        protected static Element ResolveQuery(StubElementFinder finder)
        {
            var strategy = new FinderOptionsDisambiguationStrategy();
            var results = strategy.ResolveQuery(finder);
            return results;
        }
    }
}