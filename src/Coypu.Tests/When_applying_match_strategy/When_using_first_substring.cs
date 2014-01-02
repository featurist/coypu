using System;
using System.Collections.Generic;
using System.Linq;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

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

        [Test]
        public void When_there_is_more_than_one_substring_match_it_picks_the_first_one()
        {
            var finderOptions = FinderOptions();
            var finder = new StubElementFinder(finderOptions);
            var exactResults = new List<ElementFound> { new StubElement(), new StubElement() };

            StubSubstringResults(finder, finderOptions, exactResults);

            var results = ResolveQuery(finder);

            Assert.That(results, Is.SameAs(exactResults.First()));
        }
        
        [Test]
        public void When_there_are_no_substring_matches_it_throws_missing_exception()
        {
            var finderOptions = FinderOptions();
            var finder = new StubElementFinder(finderOptions, queryDescription: "something from StubElementFinder");

            StubSubstringResults(finder, finderOptions, new List<ElementFound>());

            try
            {
                ResolveQuery(finder);
                Assert.Fail("Expected missing html exception");
            }
            catch (MissingHtmlException ex)
            {
                Assert.That(ex.Message, Is.EqualTo("Unable to find something from StubElementFinder"));
            }
        }
    }

}