using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using NUnit.Framework;
using OpenQA.Selenium;

namespace Coypu.AcceptanceTests
{
    [TestFixture]
    public class Screenshots : WaitAndRetryExamples
    {
        [Test]
        public void SavesToSpecifiedLocation()
        {
            browser.Visit(TestPageLocation("test-card.jpg"));

            browser.ResizeTo(800,600);
            const string saveTo = "screenshot-test-card.jpg";
            browser.SaveScreenshot(saveTo, ImageFormat.Jpeg);
  
            Assert.That(File.Exists(saveTo), "Expected screenshot saved to " + new FileInfo(saveTo).FullName);
            var saved = Image.FromFile("screenshot-test-card.jpg");

            var docWidth = float.Parse(browser.ExecuteScript("window.document.width"));
            var docHeight = float.Parse(browser.ExecuteScript("window.document.height"));
            Assert.That(saved.PhysicalDimension, Is.EqualTo(new SizeF(docWidth, docHeight)));
                
        }
    }
}