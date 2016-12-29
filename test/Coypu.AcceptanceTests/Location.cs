using System;
using System.IO;
using Coypu.AcceptanceTests.Sites;
using Xunit;

namespace Coypu.AcceptanceTests
{
    public class Location : IDisposable
    {
        private BrowserSession browser;
        private SelfishSite site;
        
        public Location()
        {
            site = new SelfishSite();
            
            var sessionConfiguration = new SessionConfiguration();
            sessionConfiguration.Timeout = TimeSpan.FromMilliseconds(1000);
            sessionConfiguration.Port = site.BaseUri.Port;
            browser = new BrowserSession(sessionConfiguration);

            browser.Visit("/");
        }

        public void Dispose()
        {
            browser.Dispose();
            site.Dispose();
        }

        [Fact]
        public void It_exposes_the_current_page_location()
        {
            browser.Visit("/");
            Assert.Equal(new Uri(site.BaseUri, "/"), browser.Location);

            browser.Visit("/auto_login");
            Assert.Equal(new Uri(site.BaseUri, "/auto_login"), browser.Location);
        }

        [Fact]
        public void Go_back_and_forward_in_history()
        {
            browser.Visit("/");
            browser.Visit("/auto_login");
            Assert.Equal(new Uri(site.BaseUri, "/auto_login"), browser.Location);

            browser.GoBack();
            Assert.Equal(new Uri(site.BaseUri, "/"), browser.Location);

            browser.GoForward();
            Assert.Equal(new Uri(site.BaseUri, "/auto_login"), browser.Location);
        }

        private void ReloadTestPage()
        {
            browser.Visit("file:///" + new FileInfo(@"html\InteractionTestsPage.htm").FullName.Replace("\\", "/"));
        }

        [Fact]
        public void It_exposes_the_location_of_an_iframe_scope()
        {
            ReloadTestPage();
            Assert.Contains("iFrame1.htm", browser.FindFrame("iframe1").Location.AbsolutePath);
        }
    }
}