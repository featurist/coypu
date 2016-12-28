using System.Collections.Generic;
using System.Linq;
using Coypu.Tests.TestDoubles;
using Xunit;

namespace Coypu.Tests.When_applying_match_strategy
{
    public class When_using_single_exact : Any_match_strategy
    {
        public override Match Match
        {
            get { return Match.Single; }
        }

        public override TextPrecision TextPrecision
        {
            get { return TextPrecision.Exact; }
        }

        [Fact]
        public void When_there_is_only_one_exact_match_it_finds_it_and_only_looks_for_exact()
        {
            var finderOptions = FinderOptions();
            var finder = new StubElementFinder(finderOptions);
            var exactResults = new List<Element> { new StubElement() };

            StubExactResults(finder, finderOptions, exactResults);

            var results = ResolveQuery(finder);

            Assert.Same(exactResults.Single(), results);
        }

        [Fact]
        public void When_there_is_more_than_one_exact_match_it_throws_ambiguous_execption()
        {
            var finderOptions = FinderOptions();
            var finder = new StubElementFinder(finderOptions);
            var exactResults = new List<Element> {new StubElement(), new StubElement()};

            StubExactResults(finder, finderOptions, exactResults);

            Assert.Throws<AmbiguousException>(() => ResolveQuery(finder));
        }

        
        [Fact]
        public void When_there_are_no_exact_matches_it_throws_missing_exception_and_only_looks_for_exact()
        {
            var finderOptions = FinderOptions();
            var finder = new StubElementFinder(finderOptions, queryDescription: "something from StubElementFinder");

            StubExactResults(finder, finderOptions, new List<Element>());

            try
            {
                ResolveQuery(finder);
                Assert.True(false, "Expected missing html exception");
            }
            catch (MissingHtmlException ex)
            {
                Assert.Equal("Unable to find something from StubElementFinder", ex.Message);
            }
        }
    }
}