using Coypu.Finders;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_finding_fields_returns_ambiguous_results_Match_First : DriverSpecs
    {
        private static DriverScope ExactTrue
        {
            get { return GetScope(new SessionConfiguration {Match = Match.First, Exact = true}); }
        }

        private static DriverScope ExactFalse
        {
            get { return GetScope(new SessionConfiguration {Match = Match.First, Exact = false}); }
        }

        private static DriverScope GetScope(SessionConfiguration configuration)
        {
            return new DriverScope(configuration, new DocumentElementFinder(Driver), null, null, null, null);
        }

        [Test]
        public void When_more_than_one_for_labeled_partial_match_It_selects_the_exact_match()
        {
            Assert.That(Driver.FindField("Some for labeled radio option", ExactFalse).Id, Is.EqualTo("forLabeledRadioFieldExactMatchId"));
        }

        [Test]
        public void When_more_than_one_container_labeled_partial_match_It_returns_the_exact_match()
        {
            Assert.That(Driver.FindField("Some container labeled radio option", ExactFalse).Id, Is.EqualTo("containerLabeledRadioFieldExactMatchId"));
        }

        [Test]
        public void When_more_than_field_with_the_same_name_It_returns_the_first_match()
        {
            Assert.That(Driver.FindField("someFieldNameThatAppearsTwice", ExactFalse).Id, Is.EqualTo("someFieldNameThatAppearsTwice_1"));
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
        public void When_set_to_exact_And_more_than_one_field_with_the_same_name_It_returns_the_first_match()
        {
            Assert.That(Driver.FindField("someFieldNameThatAppearsTwice", ExactTrue).Id, Is.EqualTo("someFieldNameThatAppearsTwice_1"));
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