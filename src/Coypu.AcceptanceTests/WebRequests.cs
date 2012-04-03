using System;
using System.IO;
using System.Text;
using Coypu.Drivers.Tests.Sites;
using NUnit.Framework;

namespace Coypu.AcceptanceTests
{
    [TestFixture]
    public class WebRequests
    {
        private SinatraSite sinatraSite;
        private BrowserSession browser;

        [SetUp]
        public void SetUp()
        {
            sinatraSite = new SinatraSite(string.Format(@"sites\{0}.rb", "site_with_secure_resources"));

            var configuration = new SessionConfiguration();

            configuration.Timeout = TimeSpan.FromMilliseconds(1000);
            configuration.Port = 4567;

            browser = new BrowserSession(configuration);
            browser.Visit("/");
        }

        [TearDown]
        public void TearDown()
        {
            browser.Dispose();
            sinatraSite.Dispose();
        }

        [Test]
        public void It_saves_a_resource_from_the_web()
        {
            var saveAs = TempFileName();
            var expectedResource = Encoding.Default.GetBytes("bdd");

            browser.SaveWebResource("/resource/bdd", saveAs);

            Assert.That(File.ReadAllBytes(saveAs), Is.EqualTo(expectedResource));
        }

        [Test]
        public void It_saves_a_restricted_file_from_a_site_you_are_logged_into()
        {
            var saveAs = TempFileName();
            var expectedResource = Encoding.Default.GetBytes("bdd");
            
            browser.SaveWebResource("/restricted_resource/bdd", saveAs);
            Assert.That(File.ReadAllBytes(saveAs), Is.Not.EqualTo(expectedResource));

            browser.Visit("/auto_login");

            browser.SaveWebResource("/restricted_resource/bdd", saveAs);
            Assert.That(File.ReadAllBytes(saveAs), Is.EqualTo(expectedResource));
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