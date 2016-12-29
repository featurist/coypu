using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Xunit;

namespace Coypu.AcceptanceTests
{
    public class Screenshots : WaitAndRetryExamples
    {
        public Screenshots(WaitAndRetryExamplesFixture fixture) : base(fixture)
        {
        }

        [Fact]
        public void SavesToSpecifiedLocation()
        {
            browser.Visit(TestPageLocation("test-card.jpg"));
            browser.ResizeTo(800, 600);

            SavesToSpecifiedLocation(browser);
        }

        [Fact]
        public void CapturesCorrectWindow()
        {
            browser.ClickLink("Open pop up window");
            var popUp = browser.FindWindow("Pop Up Window");
            popUp.Visit(TestPageLocation("test-card.jpg"));
            popUp.ResizeTo(800, 600);

            // Do something in the main window
            browser.FindCss("body").Click();

            SavesToSpecifiedLocation(popUp);
        }

        private static void SavesToSpecifiedLocation(BrowserWindow browserWindow)
        {
            
            const string fileName = "screenshot-test-card.jpg";
            try
            {
                browserWindow.SaveScreenshot(fileName, ImageFormat.Jpeg);

                Assert.True(File.Exists(fileName), "Expected screenshot saved to " + new FileInfo(fileName).FullName);
                using (var saved = Image.FromFile("screenshot-test-card.jpg"))
                {
                    var docWidth = float.Parse(browserWindow.ExecuteScript("return window.document.body.clientWidth;").ToString());
                    var docHeight = float.Parse(browserWindow.ExecuteScript("return window.document.body.clientHeight;").ToString());
                    Assert.Equal(new SizeF(docWidth, docHeight), saved.PhysicalDimension);
                }
            }
            finally
            {
                if (File.Exists(fileName))
                    File.Delete(fileName);
            }
        }
    }
}