using System;
using System.Collections.Generic;
using System.Linq;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

namespace Coypu.Tests.When_applying_match_strategy
{
    public class When_using_single_prefer_exact : Any_match_strategy
    {
        public override Match Match
        {
            get { return Match.Single; }
        }

        public override TextPrecision TextPrecision
        {
            get { return TextPrecision.PreferExact; }
        }

        [Test]
        public void When_there_is_only_one_exact_match_it_only_looks_for_exact_and_finds_it()
        {
            var finderOptions = FinderOptions();
            var finder = new StubElementFinder(finderOptions);

            var exactResults = new List<ElementFound> { new StubElement() };
            StubExactResults(finder, finderOptions, exactResults);

            var results = ResolveQuery(finder);

            Assert.That(results, Is.SameAs(exactResults.Single()));
        }

        [Test]
        public void When_there_is_more_than_one_exact_match_it_throws_ambiguous_exception()
        {
            var finderOptions = FinderOptions();
            var finder = new StubElementFinder(finderOptions);

            var exactResults = new List<ElementFound> { new StubElement(), new StubElement() };
            StubExactResults(finder, finderOptions, exactResults);

            Assert.Throws<AmbiguousException>(() => ResolveQuery(finder));
        }

        [Test]
        public void When_there_are_no_matches_it_throws_missing_exception()
        {
            var finderOptions = FinderOptions();
            var finder = new StubElementFinder(finderOptions, queryDescription: "something from StubElementFinder");

            StubExactResults(finder, finderOptions, new List<ElementFound>());
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

        [Test]
        public void When_there_is_no_exact_match_but_one_substring_match_it_returns_the_substring_match()
        {
            var finderOptions = FinderOptions();
            var finder = new StubElementFinder(finderOptions);

            var exactResults = new List<ElementFound> ();
            var substringResults = new List<ElementFound> { new StubElement() };

            StubExactResults(finder, finderOptions, exactResults);
            StubSubstringResults(finder, finderOptions, substringResults);

            var results = ResolveQuery(finder);

            Assert.That(results, Is.SameAs(substringResults.First()));
        }

        [Test]
        public void When_there_is_no_exact_match_but_multiple_substring_matches_it_throws_ambiguous_exception ()
        {
            var finderOptions = FinderOptions();
            var finder = new StubElementFinder(finderOptions);

            var exactResults = new List<ElementFound> ();
            var substringResults = new List<ElementFound> { new StubElement(), new StubElement() };
            StubExactResults(finder, finderOptions, exactResults);
            StubSubstringResults(finder, finderOptions, substringResults);

            Assert.Throws<AmbiguousException>(() => ResolveQuery(finder));
        }

        [Test]
        public void When_there_are_no_exact_matches_But_the_finder_does_not_support_substring_text_matching_It_doesnt_bother_trying_substrings()
        {
            var finderOptions = FinderOptions();
            var finder = new StubElementFinder(finderOptions, queryDescription: "something from StubElementFinder",
                supportsSubstringTextMatching: false);

            StubExactResults(finder, finderOptions, new List<ElementFound>());

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