using System;

namespace Coypu.Actions
{
    internal class AcceptModalDialog : DriverAction
    {
        internal AcceptModalDialog(Driver driver, TimeSpan timeout, TimeSpan retryInterval)
            : base(driver, timeout, retryInterval)
        {
        }

        public override void Act()
        {
            Driver.AcceptModalDialog();
        }
    }
}