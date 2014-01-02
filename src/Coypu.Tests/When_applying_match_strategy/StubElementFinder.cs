using System.Collections.Generic;
using Coypu.Finders;

namespace Coypu.Tests.When_applying_match_strategy
{
    public class StubElementFinder : ElementFinder
    {
        private readonly bool _supportsSubstringTextMatching;
        private readonly string _queryDescription;

        public IDictionary<Options, IEnumerable<ElementFound>> StubbedFindResults = new Dictionary<Options, IEnumerable<ElementFound>>();

        public StubElementFinder(Options options, bool supportsSubstringTextMatching = true, string queryDescription = null) : base(null, null, null, options)
        {
            _supportsSubstringTextMatching = supportsSubstringTextMatching;
            _queryDescription = queryDescription;
        }

        public override bool SupportsSubstringTextMatching
        {
            get { return _supportsSubstringTextMatching; }
        }

        internal override IEnumerable<ElementFound> Find(Options options)
        {
            return StubbedFindResults[options];
        }

        internal override string QueryDescription
        {
            get { return _queryDescription; }
        }
    }
}