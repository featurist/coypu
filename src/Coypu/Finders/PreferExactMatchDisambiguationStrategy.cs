using System.Collections.Generic;
using System.Linq;

namespace Coypu.Finders
{
    internal class PreferExactMatchDisambiguationStrategy : DisambiguationStrategy
    {
        public ElementFound ResolveQuery(ElementFinder elementFinder)
        {
            var results = GetResults(elementFinder, elementFinder.Find(Options.Merge(Options.ExactTrue, elementFinder.Options)));

            if (ShouldTryPartialMatch(elementFinder, elementFinder.Options, results.Count()))
                results = GetResults(elementFinder, elementFinder.Find(Options.Merge(Options.ExactFalse, elementFinder.Options)));

            var count = results.Length;
            if (count > 1)
                throw new AmbiguousHtmlException(elementFinder.Options.BuildAmbiguousMessage(elementFinder.QueryDescription, count));

            if (count == 0)
                throw elementFinder.GetMissingException();

            return results.First();
        }

        private static ElementFound[] GetResults(ElementFinder elementFinder, IEnumerable<ElementFound> query)
        {
            if (elementFinder.Options.Match == Match.First)
            {
                var first = query.FirstOrDefault();
                return first != null 
                           ? new[] {first} 
                           : new ElementFound[]{};
            }
            
            return query.ToArray();
        }

        private static bool ShouldTryPartialMatch(ElementFinder elementFinder, Options options, int count)
        {
            return elementFinder.SupportsPartialTextMatching &&
                   count == 0 &&
                   !options.Exact;
        }
    }
}