using System.IO;
using NUnit.Framework;

namespace Coypu.AcceptanceTests
{
    [TestFixture]
    public class Screenshots : WaitAndRetryExamples
    {
        private static void SaveFileToAssertItExists(BrowserWindow browserWindow, string fileName)
        {
            try
            {
                browserWindow.SaveScreenshot(fileName);
                Assert.That(File.Exists(fileName), "Expected screenshot saved to " + new FileInfo(fileName).FullName);
            }
            finally
            {
                if (File.Exists(fileName))
                    File.Delete(fileName);
            }
        }

        [Test]
        public void CapturesCorrectWindow()
        {
            Browser.ClickLink("Open pop up window");
            var popUp = Browser.FindWindow("Pop Up Window");
            popUp.Visit(TestPageLocation("test-card.jpg"));
            popUp.ResizeTo(800, 600);
            Browser.FindCss("body").Click();

            SaveFileToAssertItExists(popUp, "screenshot-test-card.jpg");
        }

        [Test]
        public void SavesJpgToSpecifiedLocation()
        {
            Browser.Visit(TestPageLocation("test-card.jpg"));
            Browser.ResizeTo(800, 600);

            SaveFileToAssertItExists(Browser, "screenshot-test-card.jpg");
        }

        [Test]
        public void SavesPngToSpecifiedLocation()
        {
            Browser.Visit(TestPageLocation("test-card.png"));
            Browser.ResizeTo(800, 600);

            SaveFileToAssertItExists(Browser,"screenshot-test-card.png");
        }
    }
}