using System;
using System.IO;
using System.Linq;
using NUnit.Framework;

namespace Coypu.AcceptanceTests
{
    [TestFixture]
    public class SnapshotElementScope
    {
        private SessionConfiguration SessionConfiguration;
        private BrowserSession browser;

        [TestFixtureSetUp]
        public void SetUpFixture()
        {
            SessionConfiguration = new SessionConfiguration();
            SessionConfiguration.Timeout = TimeSpan.FromMilliseconds(1000);
            browser = new BrowserSession(SessionConfiguration);
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            browser.Dispose();
        }

        [SetUp]
        public void SetUp()
        {
            
            ReloadTestPage();
        }

        private void ReloadTestPage()
        {
            browser.Visit(Helper.GetProjectFile(@"html\InteractionTestsPage.htm"));
        }

        [Test]
        public void FindAllCss_returns_scopes()
        {
            var all = browser.FindAllCss("ul.snapshot-scope").ToList();
            Assert.That(all.Count(), Is.EqualTo(2));

            Assert.That(all[0].FindCss("li:first-child").Text, Is.EqualTo("Some"));
            Assert.That(all[1].FindCss("li:first-child").Text, Is.EqualTo("one"));
        }

        [Test]
        public void FindAllXPath_returns_scopes()
        {
            var all = browser.FindAllXPath("//ul[@class='snapshot-scope']").ToList();
            Assert.That(all.Count(), Is.EqualTo(2));

            Assert.That(all[0].FindCss("li:first-child").Text, Is.EqualTo("Some"));
            Assert.That(all[1].FindCss("li:first-child").Text, Is.EqualTo("one"));

            ReloadTestPage();

            Assert.That(all[0].Missing(), Is.True);
        }

        [Test]
        public void SnapshotScopes_raise_MissingHtml_immediately_when_stale()
        {
            var all = browser.FindAllXPath("//ul[@class='snapshot-scope']").ToList();

            ReloadTestPage();

            Assert.That(all[0].FindCss("li").Missing(), Is.True);
            Assert.Throws<StaleElementException>(() => { var text = all[0].FindCss("li").Text; });
        }

        [Test]
        public void SnapshotScopes_work_with_hasContent_queries()
        {
            var all = browser.FindAllCss("ul.snapshot-scope li:first-child").ToList();
            Assert.That(all[0].HasContent("Some"), Is.True);
        }
    }
}