using System;
using System.IO;
using System.Text;
using NUnit.Framework;

namespace Coypu.AcceptanceTests
{
    [TestFixture]
    public class WebRequests
    {
        [SetUp]
        public void SetUp()
        {
            Configuration.Timeout = TimeSpan.FromMilliseconds(1000);
            Configuration.Port = 4567;

            // myNancyController.Start(4567);
        }

        private Session browser
        {
            get { return Browser.Session; }
        }


        [TestFixtureTearDown]
        public void TearDown()
        {
            Browser.EndSession();
            // myNancyController.Stop(4567);
        }

        [Test]
        public void It_saves_a_resource_from_the_web()
        {
            var saveAs = TempFileName();
            var expectedResource = Encoding.Default.GetBytes("bdd");

            browser.SaveWebResource("/resource/bdd", saveAs);

            Assert.That(File.ReadAllBytes(saveAs), Is.EqualTo(expectedResource));
        }

        [Test, Ignore("Pending")]
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