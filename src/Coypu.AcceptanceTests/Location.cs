using System;
using System.IO;
using Coypu.AcceptanceTests.Sites;
using Coypu.Drivers;
using Coypu.Drivers.Playwright;
using Microsoft.Playwright;
using NUnit.Framework;
using OpenQA.Selenium.Remote;

namespace Coypu.AcceptanceTests
{
    [TestFixture]
    public class Location
    {
        [SetUp]
        public void SetUp()
        {
            _site = new SelfHostedSite();
            var sessionConfiguration = new SessionConfiguration
                                       {
                                           Driver = typeof(PlaywrightDriver),
                                           Browser = Browser.Chrome,
                                           Timeout = TimeSpan.FromMilliseconds(1000),
                                           AppHost = "127.0.0.1",
                                           Port = _site.BaseUri.Port
                                       };
            _browser = new BrowserSession(sessionConfiguration);
            _browser.Visit("/");
        }

        [TearDown]
        public void TearDown()
        {
            _browser.Dispose();
            _site.Dispose();
        }

        private BrowserSession _browser;
        private SelfHostedSite _site;

        private void ReloadTestPage()
        {
            _browser.Visit(PathHelper.GetPageHtmlPath("InteractionTestsPage.htm"));
        }

        [Test]
        public void Go_back_and_forward_in_history()
        {
            _browser.Visit("/");
            _browser.Visit("/auto_login");
            Assert.That(_browser.Location, Is.EqualTo(new Uri(_site.BaseUri, "/auto_login")));

            _browser.GoBack();
            Assert.That(_browser.Location, Is.EqualTo(new Uri(_site.BaseUri, "/")));

            _browser.GoForward();
            Assert.That(_browser.Location, Is.EqualTo(new Uri(_site.BaseUri, "/auto_login")));
        }

        [Test]
        public void It_exposes_the_current_page_location()
        {
            _browser.Visit("/");
            Assert.That(_browser.Location, Is.EqualTo(new Uri(_site.BaseUri, "/")));

            _browser.Visit("/auto_login");
            Assert.That(_browser.Location, Is.EqualTo(new Uri(_site.BaseUri, "/auto_login")));
        }

        [Test]
        public void It_exposes_the_location_of_an_iframe_scope()
        {
            ReloadTestPage();
            Assert.That(_browser.FindFrame("iframe1")["src"],
                        Does.Contain("iFrame1.htm"));
        }
    }
}
