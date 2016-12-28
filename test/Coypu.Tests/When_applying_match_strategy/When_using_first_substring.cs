using System;
using System.Collections.Generic;
using System.Linq;
using Coypu.Tests.TestDoubles;
using Xunit;

namespace Coypu.Tests.When_applying_match_strategy
{
    public class When_using_first_substring : Any_match_strategy
    {
        public override Match Match
        {
            get { return Match.First; }
        }

        public override TextPrecision TextPrecision
        {
            get { return TextPrecision.Substring; }
        }

        [Fact]
        public void When_there_is_more_than_one_substring_match_it_picks_the_first_one()
        {
            var finderOptions = FinderOptions();
            var finder = new StubElementFinder(finderOptions);
            var exactResults = new List<Element> { new StubElement(), new StubElement() };

            StubSubstringResults(finder, finderOptions, exactResults);

            var results = ResolveQuery(finder);

            Assert.Same(exactResults.First(), results);
        }
        
        [Fact]
        public void When_there_are_no_substring_matches_it_throws_missing_exception()
        {
            var finderOptions = FinderOptions();
            var finder = new StubElementFinder(finderOptions, queryDescription: "something from StubElementFinder");

            StubSubstringResults(finder, finderOptions, new List<Element>());

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