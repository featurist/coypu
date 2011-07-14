using System;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace Coypu.AcceptanceTests
{
    [TestFixture]
    public class WebRequests
    {
        private Session browser {
            get { return Browser.Session; }
        }
        
        [SetUp]
        public void SetUp()
        {
            Configuration.Timeout = TimeSpan.FromMilliseconds(1000);
            Configuration.Port = 4567;
            browser.Visit("/");
        }

        [TestFixtureTearDown]
        public void TearDown()
        {
            Browser.EndSession();
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