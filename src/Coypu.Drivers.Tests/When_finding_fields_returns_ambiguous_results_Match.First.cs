using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_fields_returns_ambiguous_results_Match_First : DriverSpecs
    {
        protected Options FirstExact
        {
            get { return new Options { Match = Match.First, Exact = true }; }
        }

        protected Options FirstPartial
        {
            get { return new Options { Match = Match.First, Exact = false }; }
        }

        [Test]
        public void When_more_than_one_for_labeled_partial_match_It_selects_the_exact_match()
        {
            Assert.That(Field("Some for labeled radio option", options: FirstPartial).Id, Is.EqualTo("forLabeledRadioFieldExactMatchId"));
        }

        [Test]
        public void When_more_than_one_container_labeled_partial_match_It_returns_the_exact_match()
        {
            Assert.That(Field("Some container labeled radio option", options: FirstExact).Id, Is.EqualTo("containerLabeledRadioFieldExactMatchId"));
        }

        [Test]
        public void When_more_than_field_with_the_same_name_It_returns_the_first_match()
        {
            Assert.That(Field("someFieldNameThatAppearsTwice", options: FirstExact).Id, Is.EqualTo("someFieldNameThatAppearsTwice_1"));
        }

        [Test]
        public void When_set_to_exact_and_first_And_more_than_one_for_labeled_partial_match_It_finds_the_exact_match()
        {
            Assert.That(Field("Some for labeled radio option", options: new Options { Exact = true, Match = Match.First }).Id, Is.EqualTo("forLabeledRadioFieldExactMatchId"));
        }

        [Test]
        public void When_set_to_exact_And_more_than_one_container_labeled_partial_match_It_finds_the_exact_match()
        {
            Assert.That(Field("Some container labeled radio option", options: FirstPartial).Id, Is.EqualTo("containerLabeledRadioFieldExactMatchId"));
        }

        [Test]
        public void When_set_to_exact_And_only_one_for_labeled_partial_match_It_does_NOT_find_the_partial_match()
        {
            Assert.Throws<MissingHtmlException>(() => Field("Some for labeled radio option that might conflict", options: FirstExact));
        }

        [Test]
        public void When_set_to_exact_And_only_one_container_labeled_partial_match_It_does_NOT_find_the_partial_match()
        {
            Assert.Throws<MissingHtmlException>(() => Field("Some for container radio option that might conflict", options: FirstExact));
        }

        [Test]
        public void When_set_to_exact_And_more_than_one_field_with_the_same_name_It_returns_the_first_match()
        {
            Assert.That(Field("someFieldNameThatAppearsTwice", options: FirstPartial).Id, Is.EqualTo("someFieldNameThatAppearsTwice_1"));
        }

        [Test]
        public void When_set_to_partial_And_only_one_for_labeled_partial_match_It_finds_the_partial_match()
        {
            Assert.That(Field("Some for labeled radio option that might conflict", options: FirstPartial).Id,
                        Is.EqualTo("forLabeledRadioFieldPartialMatchId"));
        }

        [Test]
        public void When_set_to_partial_And_only_one_container_labeled_partial_match_It_finds_the_partial_match()
        {
            Assert.That(Field("Some container labeled radio option that might conflict", options: FirstPartial).Id,
                        Is.EqualTo("containerLabeledRadioFieldPartialMatchId"));
        }
    }
}