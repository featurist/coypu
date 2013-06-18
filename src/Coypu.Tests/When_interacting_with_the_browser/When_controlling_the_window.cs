using System.Linq;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    [TestFixture]
    public class When_controlling_the_window : BrowserInteractionTests
    {
        [Test]
        public void MaximiseWindow_acts_immediately_on_driver()
        {
            var mainWindow = browserSession;
            mainWindow.MaximiseWindow();

            Assert.That(driver.MaximiseWindowCalls.Single(), Is.SameAs(mainWindow));
        }

        [Test]
        public void MaximiseWindow_acts_on_current_scope()
        {
            popupScope.MaximiseWindow();

            Assert.That(driver.MaximiseWindowCalls.Single(), Is.SameAs(popupScope));
        }

        [Test]
        public void ResizeWindow_acts_immediately_on_driver()
        {
            var mainWindow = browserSession;
            mainWindow.ResizeTo(500, 600);

            Assert.That(driver.ResizeToCalls.Single().Request.Width, Is.EqualTo(500));
            Assert.That(driver.ResizeToCalls.Single().Request.Height, Is.EqualTo(600));
            Assert.That(driver.ResizeToCalls.Single().Scope, Is.SameAs(mainWindow));
        }

        [Test]
        public void ResizeWindow_acts_on_current_scope()
        {
            popupScope.ResizeTo(500, 600);

            Assert.That(driver.ResizeToCalls.Single().Scope, Is.SameAs(popupScope));
        }
    }
}