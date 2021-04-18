using System.Drawing;
using System.IO;
using NUnit.Framework;

namespace Coypu.Drivers.Tests
{
    internal class When_saving_screenshots : DriverSpecs
    {
        [Test]
        public void SavesAScreenshot()
        {
            Scope driverScope = Root;
            Driver.Visit(TestPageLocation("test-card.jpg"), driverScope);
            Driver.ResizeTo(new Size(800, 600), driverScope);

            string saveAs = Path.Combine(Path.GetTempPath(), "expect-saved-here.jpg");
            try
            {
                Driver.SaveScreenshot(saveAs, driverScope);
                Assert.That(File.Exists(saveAs), "Expected screenshot saved to " + new FileInfo(saveAs).FullName);
            }
            finally
            {
                if (File.Exists(saveAs))
                    File.Delete(saveAs);
            }
        }
    }
}