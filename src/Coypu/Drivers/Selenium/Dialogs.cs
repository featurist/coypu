using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;

namespace Coypu.Drivers.Selenium
{
    internal class Dialogs
    {
        private readonly IWebDriver selenium;

        public Dialogs(IWebDriver selenium)
        {
            this.selenium = selenium;
        }

        public bool HasDialog(string withText)
        {
            return HasAnyDialog() && selenium.SwitchTo().Alert().Text == withText;
        }

        public bool HasAnyDialog()
        {
            try
            {
                return selenium.SwitchTo() != null &&
                       selenium.SwitchTo().Alert() != null;
            }
            catch (NoAlertPresentException)
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