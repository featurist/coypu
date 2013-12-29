using NUnit.Framework;

namespace Coypu.AcceptanceTests
{
    internal class When_finding_fields_returns_ambiguous_results_Match_Single : WaitAndRetryExamples
    {
        protected Options SingleExact
        {
            get { return new Options { Match = Match.Single, Exact = true }; }
        }

        protected Options SinglePartial
        {
            get { return new Options { Match = Match.Single, Exact = false }; }
        }

        [Test]
        public void When_not_exact_and_more_than_one_for_labeled_partial_match_It_throws_ambiguous_exception()
        {
            Assert.Throws<AmbiguousHtmlException>(() => browser.FindField("Some for labeled radio", options: SinglePartial).Now());
        }

        [Test]
        public void When_not_exact_and_more_than_one_container_labeled_partial_match_It_throws_ambiguous_exception()
        {
            Assert.Throws<AmbiguousHtmlException>(() => browser.FindField("Some container labeled radio", options: SinglePartial).Now());
        }

        [Test]
        public void When_not_exact_and_one_exact_and_one_partial_for_labeled_match_It_returns_exact()
        {
            Assert.That(browser.FindField( "Some for labeled radio option", options: SinglePartial).Id, Is.EqualTo("forLabeledRadioFieldExactMatchId"));
        }

        [Test]
        public void When_not_exact_and_one_exact_and_one_partial_container_labeled_partial_match_It_returns_exact()
        {
            Assert.That(browser.FindField( "Some container labeled radio option", options: SinglePartial).Id, Is.EqualTo("containerLabeledRadioFieldExactMatchId"));
        }

        [Test]
        public void When_more_than_field_with_the_same_name_It_throws_ambiguous_exception()
        {
            Assert.Throws<AmbiguousHtmlException>(() => browser.FindField("someFieldNameThatAppearsTwice", options: SinglePartial).Now());
        }

        [Test]
        public void When_set_to_exact_And_more_than_one_for_labeled_partial_match_It_finds_the_exact_match()
        {
            Assert.That(browser.FindField( "Some for labeled radio option", options: SingleExact).Id, Is.EqualTo("forLabeledRadioFieldExactMatchId"));
        }

        [Test]
        public void When_set_to_exact_And_more_than_one_container_labeled_partial_match_It_finds_the_exact_match()
        {
            Assert.That(browser.FindField( "Some container labeled radio option", options: SingleExact).Id, Is.EqualTo("containerLabeledRadioFieldExactMatchId"));
        }

        [Test]
        public void When_set_to_exact_And_only_one_for_labeled_partial_match_It_does_NOT_find_the_partial_match()
        {
            Assert.Throws<MissingHtmlException>(() => browser.FindField("Some for labeled radio option that might conflict", options: SingleExact).Now());
        }

        [Test]
        public void When_set_to_exact_And_only_one_container_labeled_partial_match_It_does_NOT_find_the_partial_match()
        {
            Assert.Throws<MissingHtmlException>(() => browser.FindField("Some for container radio option that might conflict", options: SingleExact).Now());
        }

        [Test]
        public void When_set_to_exact_And_more_than_one_field_with_the_same_name_It_throws_ambiguous_exception()
        {
            Assert.Throws<AmbiguousHtmlException>(() => browser.FindField("someFieldNameThatAppearsTwice", options: SingleExact).Now());
        }

        [Test]
        public void When_set_to_partial_And_only_one_for_labeled_partial_match_It_finds_the_partial_match()
        {
            Assert.That(browser.FindField( "Some for labeled radio option that might conflict", options: SinglePartial).Id,
                        Is.EqualTo("forLabeledRadioFieldPartialMatchId"));
        }

        [Test]
        public void When_set_to_partial_And_only_one_container_labeled_partial_match_It_finds_the_partial_match()
        {
            Assert.That(browser.FindField( "Some container labeled radio option that might conflict", options: SinglePartial).Id,
                        Is.EqualTo("containerLabeledRadioFieldPartialMatchId"));
        }
    }
}