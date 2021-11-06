using System;
using NUnit.Framework;

namespace Coypu.AcceptanceTests
{
    [TestFixture]
    public class TextPrecisionAndMatch
    {

        protected BrowserSession browser;

        [OneTimeSetUp]
        public void SetUpFixture()
        {
            if (!OperatingSystem.IsWindows())
            {
                Assert.Inconclusive();
            }
            
            var configuration = new SessionConfiguration
            {
                Timeout = TimeSpan.FromMilliseconds(2000),
                Browser = Drivers.Browser.InternetExplorer
            };
            browser = new BrowserSession(configuration);

        }
        [OneTimeTearDown]
        public void TearDown()
        {
            browser?.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
            ReloadTestPage();
        }

        protected void ReloadTestPage()
        {
            var testPageLocation = PathHelper.GetPageHtmlPath("InteractionTestsPage.htm");
            browser.Visit(testPageLocation);
        }

        [Test]
        public void First_allows_ambiguous_results()
        {
            Assert.That(browser.FindField("Some for labeled", Options.FirstSubstring).Id, Is.EqualTo("forLabeledRadioFieldPartialMatchId"));
            Assert.That(browser.FindField("someFieldNameThatAppearsTwice", Options.FirstExact).Id, Is.EqualTo("someFieldNameThatAppearsTwice_1"));
            Assert.That(browser.FindField("someFieldNameThatAppearsTwice", Options.FirstPreferExact).Id, Is.EqualTo("someFieldNameThatAppearsTwice_1"));
            Assert.That(browser.FindField("Some for labeled radio option", Options.FirstPreferExact).Id, Is.EqualTo("forLabeledRadioFieldExactMatchId"));
        }

        [Test]
        public void Single_does_not_allow_ambiguous_results()
        {
            Assert.Throws<AmbiguousException>(() => browser.FindField("Some for labeled", Options.SingleSubstring).Now());
            Assert.Throws<AmbiguousException>(() => browser.FindField("someFieldNameThatAppearsTwice", Options.SingleExact).Now());
            Assert.Throws<AmbiguousException>(() => browser.FindField("someFieldNameThatAppearsTwice", Options.SinglePreferExact).Now());
            Assert.Throws<AmbiguousException>(() => browser.FindField("Some for labeled", Options.SinglePreferExact).Now());
        }

        [Test]
        public void Exact_finds_only_exact_text_matches()
        {
            Assert.That(browser.FindField("Some for labeled radio option", Options.FirstExact).Id, Is.EqualTo("forLabeledRadioFieldExactMatchId"));
            Assert.Throws<MissingHtmlException>(() => browser.FindField("Some for labeled radio", Options.FirstExact).Now());
        }
        
        [Test]
        public void Substring_finds_substring_text_matches()
        {
            Assert.That(browser.FindField("Some for labeled radio option", Options.FirstSubstring).Id, Is.EqualTo("forLabeledRadioFieldPartialMatchId"));
            Assert.That(browser.FindField("Some for labeled", Options.FirstSubstring).Id, Is.EqualTo("forLabeledRadioFieldPartialMatchId"));
        }

        // There is a race condition where these tests can fail occasionally
        //
        // If you have TextPrecision.PreferExact and the expected content appears after some async delay then the
        // disambiguation strategy may have just failed to find an exact match when the content appears and so the first
        // check to find anything is one for substring matches, whereas the desired behaviour is to prefer the Exact match.
        //
        // Depending on if you have Match.First or Match.Single you may see the wrong element, or an AmbiguousHtmlException
        //
        // There is a (default) 50ms retry interval, but no delay between checking exact and substring, which is why it is only
        // seen occassionally.
        //
        // Could possibly recheck for an exact match after finding multiple substring matches rather than just picking the first
        // but this would be a performance hit all over the place. 
        //
        // Probably best just to encourage switching to TextPrecision.Exact where you hit this situation.
        [Test]
        public void PreferExact_finds_exact_matches_before_substring_matches()
        {
            Assert.That(browser.FindField("Some for labeled radio option", Options.FirstPreferExact).Id, Is.EqualTo("forLabeledRadioFieldExactMatchId"));
            Assert.That(browser.FindField("Some for labeled radio option", Options.SinglePreferExact).Id, Is.EqualTo("forLabeledRadioFieldExactMatchId"));
        }

        [Test]
        public void PreferExact_finds_exact_match_select_option_before_substring_match()
        {
            browser.Select("one",Options.PreferExact).From("Ambiguous select options");
            Assert.That(browser.FindField("Ambiguous select options").SelectedOption, Is.EqualTo("one"));
        }
    }
}