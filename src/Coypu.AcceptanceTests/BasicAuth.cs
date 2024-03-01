using System;
using System.IO;
using System.Text;
using System.Threading;
using Coypu.AcceptanceTests.Sites;
using Coypu.Drivers.Playwright;
using Coypu.NUnit.Matchers;
using NUnit.Framework;

namespace Coypu.AcceptanceTests
{
    [TestFixture]
    public class BasicAuth
    {
        private SelfHostedSite site;
        private BrowserSession browser;

        [SetUp]
        public void SetUp()
        {
            site = new SelfHostedSite();
        }

        [TearDown]
        public void TearDown()
        {
            browser.Dispose();
            site.Dispose();
        }

        [Test]
        public void It_passes_through_basic_auth_from_url_as_auth_header()
        {
            var configuration = new SessionConfiguration
            {
              Timeout = TimeSpan.FromMilliseconds(1000),
              Port = site.BaseUri.Port,
              AppHost = "http://someUser:passw0rd@localhost",
              Driver = typeof(PlaywrightDriver), // Selenium can't do this
              Headless = false,
              Browser = Drivers.Browser.Chromium
            };

            browser = new BrowserSession(configuration);
            browser.Visit("/");

            browser.Visit("/headers");
            Assert.That(browser, Shows.Content("Authorization: " + GetBasicAuthHeader("someUser", "passw0rd")));
        }

        private string GetBasicAuthHeader(string username, string password)
        {
            var auth = $"{username}:{password}";
            var bytes = Encoding.UTF8.GetBytes(auth);
            return "Basic " + Convert.ToBase64String(bytes);
        }
    }
}
