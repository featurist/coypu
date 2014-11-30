using System.Collections.Generic;
using System.Linq;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_applying_match_strategy
{
    public class When_using_single_substring : Any_match_strategy
    {
        public override Match Match
        {
            get { return Match.Single; }
        }

        public override TextPrecision TextPrecision
        {
            get { return TextPrecision.Substring; }
        }

        [Test]
        public void When_there_is_only_one_substring_match_it_finds_it_with_exact_false()
        {
            var finderOptions = FinderOptions();
            var finder = new StubElementFinder(finderOptions);
            var exactResults = new List<Element> { new StubElement() };

            StubSubstringResults(finder, finderOptions, exactResults);

            var results = ResolveQuery(finder);

            Assert.That(results, Is.SameAs(exactResults.First()));
        }

        [Test]
        public void When_there_is_more_than_one_substring_match_it_throws_ambiguous_execption()
        {
            var finderOptions = FinderOptions();
            var finder = new StubElementFinder(finderOptions);
            var exactResults = new List<Element> {new StubElement(), new StubElement()};

            StubSubstringResults(finder, finderOptions, exactResults);

            Assert.Throws<AmbiguousException>(() => ResolveQuery(finder));
        }

        [Test]
        public void When_there_are_no_substring_matches_it_throws_missing_exception()
        {
            var finderOptions = FinderOptions();
            var finder = new StubElementFinder(finderOptions, queryDescription: "something from StubElementFinder");

            StubSubstringResults(finder, finderOptions, new List<Element>());

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