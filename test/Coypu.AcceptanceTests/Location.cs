using System;
using System.IO;
using Coypu.AcceptanceTests.Sites;
using NUnit.Framework;

namespace Coypu.AcceptanceTests
{
    [TestFixture]
    public class Location
    {
        private BrowserSession browser;
        private SelfishSite site;
        
        [SetUp]
        public void SetUp()
        {
            site = new SelfishSite();
            
            var sessionConfiguration = new SessionConfiguration();
            sessionConfiguration.Timeout = TimeSpan.FromMilliseconds(1000);
            sessionConfiguration.Port = site.BaseUri.Port;
            browser = new BrowserSession(sessionConfiguration);

            browser.Visit("/");
        }

        [TearDown]
        public void TearDown()
        {
            browser.Dispose();
            site.Dispose();
        }

        [Test]
        public void It_exposes_the_current_page_location()
        {
            browser.Visit("/");
            Assert.That(browser.Location, Is.EqualTo(new Uri(site.BaseUri, "/")));

            browser.Visit("/auto_login");
            Assert.That(browser.Location, Is.EqualTo(new Uri(site.BaseUri, "/auto_login")));
        }

        [Test]
        public void Go_back_and_forward_in_history()
        {
            browser.Visit("/");
            browser.Visit("/auto_login");
            Assert.That(browser.Location, Is.EqualTo(new Uri(site.BaseUri, "/auto_login")));

            browser.GoBack();
            Assert.That(browser.Location, Is.EqualTo(new Uri(site.BaseUri, "/")));

            browser.GoForward();
            Assert.That(browser.Location, Is.EqualTo(new Uri(site.BaseUri, "/auto_login")));
        }

        private void ReloadTestPage()
        {
            browser.Visit("file:///" + new FileInfo(@"html\InteractionTestsPage.htm").FullName.Replace("\\", "/"));
        }

        [Test]
        public void It_exposes_the_location_of_an_iframe_scope()
        {
            ReloadTestPage();
            Assert.That(browser.FindFrame("iframe1").Location.AbsolutePath, Is.StringContaining("iFrame1.htm"));
        }
    }
}