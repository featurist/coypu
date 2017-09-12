using System.Drawing.Imaging;
using System.Linq;
using Xunit;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    public class When_saving_screenshots : BrowserInteractionTests
    {
        [Fact]
        public void TakeScreenshot_acts_immediately_on_driver()
        {
            TakeScreenshot_acts_immediately_on_driver_theory(popupScope);
        }

        [Fact]
        public void TakeScreenshot_uses_current_window_scope()
        {
            TakeScreenshot_acts_immediately_on_driver_theory(popupScope);
        }

        private void TakeScreenshot_acts_immediately_on_driver_theory(BrowserWindow mainWindow)
        {
            mainWindow.SaveScreenshot("save-me-here.png", ImageFormat.Png);

            var saveScreenshotCall = driver.SaveScreenshotCalls.Single();

            Assert.Equal("save-me-here.png", saveScreenshotCall.Request);

            Assert.Equal(mainWindow, saveScreenshotCall.Scope);
        }
    }
}