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
    }
}