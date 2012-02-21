using System;
using System.IO;
using Coypu.Drivers.Tests.Sites;
using NUnit.Framework;

namespace Coypu.AcceptanceTests
{
    [TestFixture]
    public class Location
    {
        private SinatraSite sinatraSite;

        private Session browser
        {
            get { return Browser.Session; }
        }

        [SetUp]
        public void SetUp()
        {
            sinatraSite = new SinatraSite(string.Format(@"sites\{0}.rb", "site_with_secure_resources"));

            Configuration.Timeout = TimeSpan.FromMilliseconds(1000);
            Configuration.Port = 4567;
            browser.Visit("/");
        }

        [TearDown]
        public void TearDown()
        {
            Browser.EndSession();
            sinatraSite.Dispose();
        }

        [Test]
        public void It_exposes_the_current_page_location()
        {
            browser.Visit("/");
            Assert.That(browser.Location, Is.EqualTo(new Uri("http://localhost:4567/")));

            browser.Visit("/auto_login");
            Assert.That(browser.Location, Is.EqualTo(new Uri("http://localhost:4567/auto_login")));
        }

        private void ReloadTestPage()
        {
            browser.Visit("file:///" + new FileInfo(@"html\InteractionTestsPage.htm").FullName.Replace("\\", "/"));
        }

        [Test]
        public void It_exposes_the_location_of_an_iframe_scope()
        {
            ReloadTestPage();
            browser.FindIFrame("iframe1").Now();
            Assert.That(browser.Location.AbsolutePath, Is.StringContaining("iFrame1.htm"));
        }
    }
}