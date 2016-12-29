using System;
using System.IO;
using System.Linq;
using Xunit;

namespace Coypu.AcceptanceTests
{
    public class SnapshotElementScope : IClassFixture<SnapshotElementScopeFixture>
    {
        private BrowserSession browser;

        public SnapshotElementScope(SnapshotElementScopeFixture fixture)
        {
            browser = fixture.BrowserSession;
            ReloadTestPage();
        }

        private void ReloadTestPage()
        {
            browser.Visit("file:///" + new FileInfo(@"html\InteractionTestsPage.htm").FullName.Replace("\\", "/"));
        }

        [Fact]
        public void FindAllCss_returns_scopes()
        {
            var all = browser.FindAllCss("ul.snapshot-scope").ToList();
            Assert.Equal(2, all.Count());

            Assert.Equal("Some", all[0].FindCss("li:first-child").Text);
            Assert.Equal("one", all[1].FindCss("li:first-child").Text);
        }

        [Fact]
        public void FindAllXPath_returns_scopes()
        {
            var all = browser.FindAllXPath("//ul[@class='snapshot-scope']").ToList();
            Assert.Equal(2, all.Count());

            Assert.Equal("Some", all[0].FindCss("li:first-child").Text);
            Assert.Equal("one", all[1].FindCss("li:first-child").Text);

            ReloadTestPage();

            Assert.True(all[0].Missing());
        }

        [Fact]
        public void SnapshotScopes_raise_MissingHtml_immediately_when_stale()
        {
            var all = browser.FindAllXPath("//ul[@class='snapshot-scope']").ToList();

            ReloadTestPage();

            Assert.True(all[0].FindCss("li").Missing());
            Assert.Throws<StaleElementException>(() => { var text = all[0].FindCss("li").Text; });
        }

        [Fact]
        public void SnapshotScopes_work_with_hasContent_queries()
        {
            var all = browser.FindAllCss("ul.snapshot-scope li:first-child").ToList();
            Assert.True(all[0].HasContent("Some"));
        }
    }

    public class SnapshotElementScopeFixture : IDisposable
    {
        public BrowserSession BrowserSession;

        public SnapshotElementScopeFixture()
        {
            var configuration = new SessionConfiguration
            {
                Timeout = TimeSpan.FromMilliseconds(1000),
            };
            BrowserSession = new BrowserSession(configuration);
        }

        public void Dispose()
        {
            BrowserSession.Dispose();
        }
    }
}