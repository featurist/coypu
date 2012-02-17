using Coypu.Robustness;

namespace Coypu.Actions
{
    internal class AcceptModalDialog : DriverAction
    {
        private readonly Driver driver;

        internal AcceptModalDialog(Driver driver)
        {
            this.driver = driver;
        }

        public void Act()
        {
            driver.AcceptModalDialog();
        }
    }
}