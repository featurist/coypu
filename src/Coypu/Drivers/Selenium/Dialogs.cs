using System;
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
            try
            {
                return selenium.SwitchTo() != null &&
                       selenium.SwitchTo().Alert() != null &&
                       selenium.SwitchTo().Alert().Text == withText;
            }
            catch (OpenQA.Selenium.NoAlertPresentException)
            {
                return false;
            }
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