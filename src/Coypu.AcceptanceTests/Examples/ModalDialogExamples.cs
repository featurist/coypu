using NUnit.Framework;

namespace Coypu.AcceptanceTests.Examples
{
    public class ModalDialogExamples : WaitAndRetryExamples
    {
        [Test]
        public void AcceptModalDialog_example()
        {
            Browser.ClickLink("Trigger an alert");
            Assert.IsTrue(Browser.HasDialog("You have triggered an alert and this is the text."));

            Browser.AcceptModalDialog();
            Assert.IsTrue(Browser.HasNoDialog("You have triggered an alert and this is the text."));
        }

        [Test]
        public void CancelModalDialog_example()
        {
            Browser.ClickLink("Trigger a confirm");
            Browser.CancelModalDialog();
            Browser.FindLink("Trigger a confirm - cancelled")
                   .Now();
        }

        [Test]
        public void ModalDialog_while_multiple_windows_are_open()
        {
            Browser.ClickLink("Open pop up window");
            Browser.ClickLink("Trigger a confirm");
            Browser.CancelModalDialog();
            Browser.FindLink("Trigger a confirm - cancelled")
                   .Now();
        }
    }
}