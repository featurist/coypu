using System;
using System.IO;
using System.Text;
using Coypu.AcceptanceTests.Sites;
using Xunit;

namespace Coypu.AcceptanceTests
{
    public class WebRequests : IDisposable
    {
        private SelfishSite site;
        private BrowserSession browser;

        public WebRequests()
        {
            site = new SelfishSite();

            var configuration = new SessionConfiguration();

            configuration.Timeout = TimeSpan.FromMilliseconds(1000);
            configuration.Port = site.BaseUri.Port;

            browser = new BrowserSession(configuration);
            browser.Visit("/");
        }

        public void Dispose()
        {
            browser.Dispose();
            site.Dispose();
        }

        [Fact]
        public void It_saves_a_resource_from_the_web()
        {
            var saveAs = TempFileName();
            var expectedResource = Encoding.UTF8.GetBytes("bdd");

            browser.SaveWebResource("/resource/bdd", saveAs);

            Assert.Equal(expectedResource, File.ReadAllBytes(saveAs));
        }

        [Fact]
        public void It_saves_a_restricted_file_from_a_site_you_are_logged_into()
        {
            var saveAs = TempFileName();
            var expectedResource = "bdd";
            
            browser.SaveWebResource("/restricted_resource/bdd", saveAs);
            Assert.NotEqual(expectedResource, File.ReadAllText(saveAs));

            browser.Visit("/auto_login");

            browser.SaveWebResource("/restricted_resource/bdd", saveAs);
            Assert.Equal(expectedResource, File.ReadAllText(saveAs));
        }

        private string TempFileName()
        {
            var saveAs = Path.GetTempFileName();
            Clean(saveAs);
            return saveAs;
        }

        private void Clean(string saveAs)
        {
            if (File.Exists(saveAs))
                File.Delete(saveAs);
        }
    }
}