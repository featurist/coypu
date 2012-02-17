using Coypu.Robustness;

namespace Coypu.Actions
{
    internal class CancelModalDialog : DriverAction
    {
        private readonly Driver driver;

        internal CancelModalDialog(Driver driver)
        {
            this.driver = driver;
        }

        public void Act()
        {
            driver.CancelModalDialog();
        }
    }
}