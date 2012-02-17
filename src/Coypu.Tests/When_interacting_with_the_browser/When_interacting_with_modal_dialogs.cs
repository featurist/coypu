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
            session.AcceptModalDialog();

            Assert.That(driver.ModalDialogsAccepted, Is.EqualTo(0));
            spyRobustWrapper.DeferredDriverActions.Single().Act();
            Assert.That(driver.ModalDialogsAccepted, Is.EqualTo(1));
        }

        [Test]
        public void CancelDialog_should_make_robust_call_to_underlying_driver()
        {
            session.CancelModalDialog();

            Assert.That(driver.ModalDialogsCancelled, Is.EqualTo(0));
            spyRobustWrapper.DeferredDriverActions.Single().Act();
            Assert.That(driver.ModalDialogsCancelled, Is.EqualTo(1));
        }

    }
}