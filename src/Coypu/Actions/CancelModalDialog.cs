namespace Coypu.Actions
{
    internal class CancelModalDialog : DriverAction
    {
        internal CancelModalDialog(Driver driver, Options options) : base(driver,options)
        {
        }

        public override void Act()
        {
            Driver.CancelModalDialog();
        }
    }
}