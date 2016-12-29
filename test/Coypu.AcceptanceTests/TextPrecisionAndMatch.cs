using System;
using System.IO;
using Coypu.Drivers;
using Xunit;

namespace Coypu.AcceptanceTests
{
    public class TextPrecisionAndMatch : IClassFixture<TextPrecisionAndMatchFixture>
    {
        private BrowserSession browser;

        public TextPrecisionAndMatch(TextPrecisionAndMatchFixture fixture)
        {
            browser = fixture.BrowserSession;
            ReloadTestPage();
        }

        protected void ReloadTestPage()
        {
            browser.Visit(TestPageLocation("InteractionTestsPage.htm"));
        }

        protected static string TestPageLocation(string page)
        {
            var testPageLocation = "file:///" + new FileInfo(@"html\" + page).FullName.Replace("\\", "/");
            return testPageLocation;
        }

        [Fact]
        public void First_allows_ambiguous_results()
        {
            Assert.Equal("forLabeledRadioFieldPartialMatchId", browser.FindField("Some for labeled", Options.FirstSubstring).Id);
            Assert.Equal("someFieldNameThatAppearsTwice_1", browser.FindField("someFieldNameThatAppearsTwice", Options.FirstExact).Id);
            Assert.Equal("someFieldNameThatAppearsTwice_1", browser.FindField("someFieldNameThatAppearsTwice", Options.FirstPreferExact).Id);
            Assert.Equal("forLabeledRadioFieldExactMatchId", browser.FindField("Some for labeled radio option", Options.FirstPreferExact).Id);
        }

        [Fact]
        public void Single_does_not_allow_ambiguous_results()
        {
            Assert.Throws<AmbiguousException>(() => browser.FindField("Some for labeled", Options.SingleSubstring).Now());
            Assert.Throws<AmbiguousException>(() => browser.FindField("someFieldNameThatAppearsTwice", Options.SingleExact).Now());
            Assert.Throws<AmbiguousException>(() => browser.FindField("someFieldNameThatAppearsTwice", Options.SinglePreferExact).Now());
            Assert.Throws<AmbiguousException>(() => browser.FindField("Some for labeled", Options.SinglePreferExact).Now());
        }

        [Fact]
        public void Exact_finds_only_exact_text_matches()
        {
            Assert.Equal("forLabeledRadioFieldExactMatchId", browser.FindField("Some for labeled radio option", Options.FirstExact).Id);
            Assert.Throws<MissingHtmlException>(() => browser.FindField("Some for labeled radio", Options.FirstExact).Now());
        }
        
        [Fact]
        public void Substring_finds_substring_text_matches()
        {
            Assert.Equal("forLabeledRadioFieldPartialMatchId", browser.FindField("Some for labeled radio option", Options.FirstSubstring).Id);
            Assert.Equal("forLabeledRadioFieldPartialMatchId", browser.FindField("Some for labeled", Options.FirstSubstring).Id);
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
        [Fact]
        public void PreferExact_finds_exact_matches_before_substring_matches()
        {
            Assert.Equal("forLabeledRadioFieldExactMatchId", browser.FindField("Some for labeled radio option", Options.FirstPreferExact).Id);
            Assert.Equal("forLabeledRadioFieldExactMatchId", browser.FindField("Some for labeled radio option", Options.SinglePreferExact).Id);
        }

        [Fact]
        public void PreferExact_finds_exact_match_select_option_before_substring_match()
        {
            browser.Select("one",Options.PreferExact).From("Ambiguous select options");
            Assert.Equal("one", browser.FindField("Ambiguous select options").SelectedOption);
        }
    }

    public class TextPrecisionAndMatchFixture : IDisposable
    {
        public BrowserSession BrowserSession;

        public TextPrecisionAndMatchFixture()
        {
            var configuration = new SessionConfiguration
            {
                Timeout = TimeSpan.FromMilliseconds(2000),
                Browser = Browser.InternetExplorer
            };
            BrowserSession = new BrowserSession(configuration);
        }

        public void Dispose()
        {
            BrowserSession.Dispose();
        }
    }
}