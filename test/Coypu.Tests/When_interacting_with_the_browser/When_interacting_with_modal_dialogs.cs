using System.Linq;
using Xunit;

namespace Coypu.Tests.When_interacting_with_the_browser
{
    public class When_interacting_with_modal_dialogs : BrowserInteractionTests
    {
        [Fact]
        public void AcceptDialog_should_make_robust_call_to_underlying_driver()
        {
            browserSession.AcceptModalDialog();

            Assert.False(driver.ModalDialogsAccepted.Any());
            RunQueryAndCheckTiming();
            Assert.Same(browserSession, driver.ModalDialogsAccepted.Single());
        }

        [Fact]
        public void CancelDialog_should_make_robust_call_to_underlying_driver()
        {
            browserSession.CancelModalDialog();

            Assert.False(driver.ModalDialogsCancelled.Any());
            RunQueryAndCheckTiming();
            Assert.Same(browserSession, driver.ModalDialogsCancelled.Single());
        }

    }
}