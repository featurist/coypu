using OpenQA.Selenium;

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
            try
            {
                selenium.SwitchTo().Alert().Accept();
            }
            catch (NoAlertPresentException ex)
            {
                throw new MissingDialogException("No dialog was present to accept", ex);
            }
        }

        public void CancelModalDialog()
        {
            try
            {
                selenium.SwitchTo().Alert().Dismiss();
            }
            catch (NoAlertPresentException ex)
            {
                throw new MissingDialogException("No dialog was present to accept", ex);
            }
        }
    }
}