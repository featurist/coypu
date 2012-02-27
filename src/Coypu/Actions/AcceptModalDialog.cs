using System;

namespace Coypu.Actions
{
    internal class AcceptModalDialog : DriverAction
    {
        internal AcceptModalDialog(Driver driver, TimeSpan timeout)
            : base(driver, timeout)
        {
        }

        public override void Act()
        {
            Driver.AcceptModalDialog();
        }
    }
}