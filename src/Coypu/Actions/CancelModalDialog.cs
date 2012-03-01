using System;

namespace Coypu.Actions
{
    internal class CancelModalDialog : DriverAction
    {
        internal CancelModalDialog(Driver driver, TimeSpan timeout, TimeSpan retryInterval)
            : base(driver, timeout, retryInterval)
        {
        }

        public override void Act()
        {
            Driver.CancelModalDialog();
        }
    }
}