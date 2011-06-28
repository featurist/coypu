using OpenQA.Selenium.Remote;

namespace Coypu.Drivers.Selenium
{
    internal class Dialogs
    {
        private readonly RemoteWebDriver selenium;

        public Dialogs(RemoteWebDriver selenium)
        {
            this.selenium = selenium;
        }

        public bool HasDialog(string withText)
        {
            return selenium.SwitchTo() != null &&
                   selenium.SwitchTo().Alert() != null &&
                   selenium.SwitchTo().Alert().Text == withText;
        }

        public void AcceptModalDialog()
        {
            selenium.SwitchTo().Alert().Accept();
        }

        public void CancelModalDialog()
        {
            selenium.SwitchTo().Alert().Dismiss();
        }
    }
}