using System.Linq;
using NUnit.Framework;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    [TestFixture]
    public class When_interacting_with_modal_dialogs : BrowserInteractionTests
    {
        [Test]
        public void AcceptDialog_should_make_robust_call_to_underlying_driver()
        {
            browserSession.AcceptModalDialog();

            Assert.That(driver.ModalDialogsAccepted.Any(), Is.False);
            RunQueryAndCheckTiming();
            Assert.That(driver.ModalDialogsAccepted.Single(), Is.SameAs(browserSession.DriverScope));
        }

        [Test]
        public void CancelDialog_should_make_robust_call_to_underlying_driver()
        {
            browserSession.CancelModalDialog();

            Assert.That(driver.ModalDialogsCancelled.Any(), Is.False);
            RunQueryAndCheckTiming();
            Assert.That(driver.ModalDialogsCancelled.Single(), Is.SameAs(browserSession.DriverScope));
        }

    }
}