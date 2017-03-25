using System.Collections.Generic;
using Coypu.Finders;

namespace Coypu.Tests.When_applying_match_strategy
{
    public class StubElementFinder : ElementFinder
    {
        private readonly bool _supportsSubstringTextMatching;
        private readonly string _queryDescription;

        public IDictionary<Options, IEnumerable<Element>> StubbedFindResults = new Dictionary<Options, IEnumerable<Element>>();

        public StubElementFinder(Options options, bool supportsSubstringTextMatching = true, string queryDescription = null) : base(null, null, null, options)
        {
            _supportsSubstringTextMatching = supportsSubstringTextMatching;
            _queryDescription = queryDescription;
        }

        public override bool SupportsSubstringTextMatching
        {
            get { return _supportsSubstringTextMatching; }
        }

        public override IEnumerable<Element> Find(Options options)
        {
            return StubbedFindResults[options];
        }

        public override string QueryDescription
        {
            get { return _queryDescription; }
        }
    }
}