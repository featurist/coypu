using System.Collections.Generic;
using System.Linq;
using Coypu.Tests.TestDoubles;
using NUnit.Framework;

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

        [Test]
        public void When_there_is_only_one_exact_match_it_finds_it_and_only_looks_for_exact()
        {
            var finderOptions = FinderOptions();
            var finder = new StubElementFinder(finderOptions);
            var exactResults = new List<ElementFound> { new StubElement() };

            StubExactResults(finder, finderOptions, exactResults);

            var results = ResolveQuery(finder);

            Assert.That(results, Is.SameAs(exactResults.Single()));
        }

        [Test]
        public void When_there_is_more_than_one_exact_match_it_throws_ambiguous_execption()
        {
            var finderOptions = FinderOptions();
            var finder = new StubElementFinder(finderOptions);
            var exactResults = new List<ElementFound> {new StubElement(), new StubElement()};

            StubExactResults(finder, finderOptions, exactResults);

            Assert.Throws<AmbiguousException>(() => ResolveQuery(finder));
        }

        
        [Test]
        public void When_there_are_no_exact_matches_it_throws_missing_exception_and_only_looks_for_exact()
        {
            var finderOptions = FinderOptions();
            var finder = new StubElementFinder(finderOptions, queryDescription: "something from StubElementFinder");

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