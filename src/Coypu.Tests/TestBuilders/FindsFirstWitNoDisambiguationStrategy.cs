using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Coypu.Finders;

namespace Coypu.Tests.TestBuilders
{
    public class FirstOrDefaultNoDisambiguationStrategy : DisambiguationStrategy
    {
        public Element ResolveQuery(ElementFinder elementFinder)
        {
            return elementFinder.Find(elementFinder.Options).FirstOrDefault();
        }
    }

    public class ThrowsWhenMissingButNoDisambiguationStrategy : DisambiguationStrategy
    {
        public Element ResolveQuery(ElementFinder elementFinder)
        {
          var all = elementFinder.Find(elementFinder.Options);
          var array = AsArray(all, elementFinder);
          if (!array.Any())
            throw elementFinder.GetMissingException();

          return all.First();
        }

        private static Element[] AsArray(IEnumerable<Element> all, ElementFinder elementFinder)
        {
            try {
                 return all.ToArray();
            }
            catch(InvalidOperationException) {
                // Elements have changed due to async page behaviour
                throw elementFinder.GetMissingException();
            }
        }
    }
}
