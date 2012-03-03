namespace Coypu.Actions
{
    internal class AcceptModalDialog : DriverAction
    {
        internal AcceptModalDialog(Driver driver, Options options) : base(driver,options)
        {
        }

        public override void Act()
        {
            Driver.AcceptModalDialog();
        }
    }
}