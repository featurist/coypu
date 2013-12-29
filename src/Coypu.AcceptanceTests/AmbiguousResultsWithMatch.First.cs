using NUnit.Framework;

namespace Coypu.AcceptanceTests
{
    internal class When_finding_fields_returns_ambiguous_results_Match_First : WaitAndRetryExamples
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
            Assert.That(browser.FindField("Some for labeled radio option", options: FirstPartial).Id, Is.EqualTo("forLabeledRadioFieldExactMatchId"));
        }

        [Test]
        public void When_more_than_one_container_labeled_partial_match_It_returns_the_exact_match()
        {
            Assert.That(browser.FindField("Some container labeled radio option", options: FirstExact).Id, Is.EqualTo("containerLabeledRadioFieldExactMatchId"));
        }

        [Test]
        public void When_more_than_field_with_the_same_name_It_returns_the_first_match()
        {
            Assert.That(browser.FindField("someFieldNameThatAppearsTwice", options: FirstExact).Id, Is.EqualTo("someFieldNameThatAppearsTwice_1"));
        }

        [Test]
        public void When_set_to_exact_and_first_And_more_one_exact_and_one_partial_for_labeled_partial_match_It_finds_the_exact_match()
        {
            Assert.That(browser.FindField("Some for labeled radio option", options: new Options { Exact = true, Match = Match.First }).Id, Is.EqualTo("forLabeledRadioFieldExactMatchId"));
        }

        [Test]
        public void When_set_to_partial_and_first_And_more_than_one_for_labeled_partial_match_It_finds_the_first_match()
        {
            Assert.That(browser.FindField("Some for labeled radio", options: new Options { Exact = false, Match = Match.First }).Id, Is.EqualTo("forLabeledRadioFieldPartialMatchId"));
        }

        // There is a race condition where this test fails every 3 or 4 times:
        //
        // If you have Match.First and Exact:False and the expected content appears after some async delay then the
        // disambiguation strategy may have just failed to find an exact match when the content appears and so the first
        // check to find anything returns the first partial match, whereas the desired behaviour is to prefer the Exact match.
        //
        // Could possibly recheck for an exact match after finding multiple partial matches rather than just picking the first
        // but this would be a performance hit all over the place. 
        //
        // Probably best just to encourage switching to Exact:True where you hit this situation.
        [Test]
        public void When_set_to_exact_And_more_than_one_container_labeled_partial_match_It_finds_the_exact_match()
        {
            Assert.That(browser.FindField("Some container labeled radio option", options: FirstPartial).Id, Is.EqualTo("containerLabeledRadioFieldExactMatchId"));
        }

        [Test]
        public void When_set_to_exact_And_only_one_for_labeled_partial_match_It_does_NOT_find_the_partial_match()
        {
            Assert.Throws<MissingHtmlException>(() => browser.FindField("Some for labeled radio option that might conflict", options: FirstExact).Now());
        }

        [Test]
        public void When_set_to_exact_And_only_one_container_labeled_partial_match_It_does_NOT_find_the_partial_match()
        {
            Assert.Throws<MissingHtmlException>(() => browser.FindField( "Some for container radio option that might conflict", options: FirstExact).Now());
        }

        [Test]
        public void When_set_to_exact_And_more_than_one_field_with_the_same_name_It_returns_the_first_match()
        {
            Assert.That(browser.FindField( "someFieldNameThatAppearsTwice", options: FirstPartial).Id, Is.EqualTo("someFieldNameThatAppearsTwice_1"));
        }

        [Test]
        public void When_set_to_partial_And_only_one_for_labeled_partial_match_It_finds_the_partial_match()
        {
            Assert.That(browser.FindField( "Some for labeled radio option that might conflict", options: FirstPartial).Id,
                        Is.EqualTo("forLabeledRadioFieldPartialMatchId"));
        }

        [Test]
        public void When_set_to_partial_And_only_one_container_labeled_partial_match_It_finds_the_partial_match()
        {
            Assert.That(browser.FindField( "Some container labeled radio option that might conflict", options: FirstPartial).Id,
                        Is.EqualTo("containerLabeledRadioFieldPartialMatchId"));
        }
    }
}