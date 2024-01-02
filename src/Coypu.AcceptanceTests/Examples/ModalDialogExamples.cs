using NUnit.Framework;

namespace Coypu.AcceptanceTests.Examples
{
    public class ModalDialogExamples : WaitAndRetryExamples
    {
        [Test]
        public void AcceptModalDialog_example()
        {
            Browser.AcceptAlert("You have triggered an alert and this is the text.", () => {
                Browser.ClickLink("Trigger an alert");
            });
            Browser.FindLink("Trigger an alert - accepted")
                   .Now();
        }

        [Test]
        public void CancelModalDialog_example()
        {
            Browser.CancelConfirm(() => {
                Browser.ClickLink("Trigger a confirm");
            });
            Browser.FindLink("Trigger a confirm - cancelled")
                   .Now();
        }

        [Test]
        public void ModalDialog_while_multiple_windows_are_open()
        {
            Browser.ClickLink("Open pop up window");
            Browser.CancelConfirm(() => {
                Browser.ClickLink("Trigger a confirm");
            });
            Browser.FindLink("Trigger a confirm - cancelled")
                   .Now();
        }
    }
}
