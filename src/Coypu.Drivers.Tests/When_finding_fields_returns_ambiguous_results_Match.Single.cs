using Coypu.Finders;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_fields_returns_ambiguous_results_Match_Single : DriverSpecs
    {
        private static DriverScope ExactTrue
        {
            get { return GetScope(new SessionConfiguration { Match = Match.Single, Exact = true }); }
        }

        private static DriverScope ExactFalse
        {
            get { return GetScope(new SessionConfiguration { Match = Match.Single, Exact = false }); }
        }

        private static DriverScope GetScope(SessionConfiguration configuration)
        {
            return new DriverScope(configuration, new DocumentElementFinder(Driver), null, null, null, null);
        }

        [Test]
        public void When_not_exact_and_more_than_one_for_labeled_partial_match_It_throws_ambiguous_exception()
        {
            Assert.Throws<AmbiguousHtmlException>(() => Driver.FindField("Some for labeled radio", ExactFalse));
        }

        [Test]
        public void When_not_exact_and_more_than_one_container_labeled_partial_match_It_throws_ambiguous_exception()
        {
            Assert.Throws<AmbiguousHtmlException>(() => Driver.FindField("Some container labeled radio", ExactFalse));
        }

        [Test]
        public void When_not_exact_and_one_exact_and_one_partial_for_labeled_match_It_returns_exact()
        {
            Assert.That(Driver.FindField("Some for labeled radio option", ExactFalse).Id, Is.EqualTo("forLabeledRadioFieldExactMatchId"));
        }

        [Test]
        public void When_not_exact_and_one_exact_and_one_partial_container_labeled_partial_match_It_returns_exact()
        {
            Assert.That(Driver.FindField("Some container labeled radio option", ExactFalse).Id, Is.EqualTo("containerLabeledRadioFieldExactMatchId"));
        }

        [Test]
        public void When_more_than_field_with_the_same_name_It_throws_ambiguous_exception()
        {
            Assert.Throws<AmbiguousHtmlException>(() => Driver.FindField("someFieldNameThatAppearsTwice", ExactFalse));
        }

        [Test]
        public void When_set_to_exact_And_more_than_one_for_labeled_partial_match_It_finds_the_exact_match()
        {
            Assert.That(Driver.FindField("Some for labeled radio option", ExactTrue).Id, Is.EqualTo("forLabeledRadioFieldExactMatchId"));
        }

        [Test]
        public void When_set_to_exact_And_more_than_one_container_labeled_partial_match_It_finds_the_exact_match()
        {
            Assert.That(Driver.FindField("Some container labeled radio option", ExactTrue).Id, Is.EqualTo("containerLabeledRadioFieldExactMatchId"));
        }

        [Test]
        public void When_set_to_exact_And_only_one_for_labeled_partial_match_It_does_NOT_find_the_partial_match()
        {
            Assert.Throws<MissingHtmlException>(() => Driver.FindField("Some for labeled radio option that might conflict", ExactTrue));
        }

        [Test]
        public void When_set_to_exact_And_only_one_container_labeled_partial_match_It_does_NOT_find_the_partial_match()
        {
            Assert.Throws<MissingHtmlException>(() => Driver.FindField("Some for container radio option that might conflict", ExactTrue));
        }

        [Test]
        public void When_set_to_exact_And_more_than_one_field_with_the_same_name_It_throws_ambiguous_exception()
        {
            Assert.Throws<AmbiguousHtmlException>(() => Driver.FindField("someFieldNameThatAppearsTwice", ExactTrue));
        }

        [Test]
        public void When_set_to_partial_And_only_one_for_labeled_partial_match_It_finds_the_partial_match()
        {
            Assert.That(Driver.FindField("Some for labeled radio option that might conflict", ExactFalse).Id,
                        Is.EqualTo("forLabeledRadioFieldPartialMatchId"));
        }

        [Test]
        public void When_set_to_partial_And_only_one_container_labeled_partial_match_It_finds_the_partial_match()
        {
            Assert.That(Driver.FindField("Some container labeled radio option that might conflict", ExactFalse).Id,
                        Is.EqualTo("containerLabeledRadioFieldPartialMatchId"));
        }
    }
}