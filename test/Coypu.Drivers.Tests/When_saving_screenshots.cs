using System;
using System.Drawing;
using System.IO;
using Xunit;

namespace Coypu.Drivers.Tests
{
    public class When_saving_screenshots : DriverSpecs
    {
        [Fact]
        public void SavesAScreenshot()
        {
            Scope driverScope = Root;
            Driver.Visit(TestHtmlPathLocation("html\\test-card.jpg"), driverScope);
            Driver.ResizeTo(new Size(800, 600), driverScope);

            const string saveAs = "expect-saved-here.jpg";
            try
            {
                Driver.SaveScreenshot(saveAs, driverScope);
                Assert.True(File.Exists(saveAs), "Expected screenshot saved to " + new FileInfo(saveAs).FullName);
                using (var saved = Image.FromFile(saveAs))
                {
                    var docWidth = (Int64) Driver.ExecuteScript("return window.document.body.clientWidth;", driverScope);
                    var docHeight = (Int64) Driver.ExecuteScript("return window.document.body.clientHeight;", driverScope);
                    Assert.InRange(saved.PhysicalDimension.Width, docWidth - 10, docWidth);
                    Assert.InRange(saved.PhysicalDimension.Height, docHeight - 75, docHeight);
                }
            }
            finally
            {
                if (File.Exists(saveAs))
                    File.Delete(saveAs);
            }
        }
    }
}