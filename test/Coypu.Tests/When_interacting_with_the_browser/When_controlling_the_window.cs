using System.Linq;
using Xunit;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    public class When_controlling_the_window : BrowserInteractionTests
    {
        [Fact]
        public void MaximiseWindow_acts_immediately_on_driver()
        {
            var mainWindow = browserSession;
            mainWindow.MaximiseWindow();

            Assert.Same(mainWindow, driver.MaximiseWindowCalls.Single());
        }

        [Fact]
        public void MaximiseWindow_acts_on_current_scope()
        {
            popupScope.MaximiseWindow();

            Assert.Same(popupScope, driver.MaximiseWindowCalls.Single());
        }

        [Fact]
        public void ResizeWindow_acts_immediately_on_driver()
        {
            var mainWindow = browserSession;
            mainWindow.ResizeTo(500, 600);

            Assert.Equal(500, driver.ResizeToCalls.Single().Request.Width);
            Assert.Equal(600, driver.ResizeToCalls.Single().Request.Height);
            Assert.Same(mainWindow, driver.ResizeToCalls.Single().Scope);
        }

        [Fact]
        public void ResizeWindow_acts_on_current_scope()
        {
            popupScope.ResizeTo(500, 600);

            Assert.Same(popupScope, driver.ResizeToCalls.Single().Scope);
        }
    }
}