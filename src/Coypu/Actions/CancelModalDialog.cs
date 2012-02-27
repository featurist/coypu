using System;

namespace Coypu.Actions
{
    internal class CancelModalDialog : DriverAction
    {
        internal CancelModalDialog(Driver driver, TimeSpan timeout) : base(driver, timeout)
        {
        }

        public override void Act()
        {
            Driver.CancelModalDialog();
        }
    }
}