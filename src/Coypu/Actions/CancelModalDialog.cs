namespace Coypu.Actions
{
    internal class CancelModalDialog : DriverAction
    {
        private readonly DriverScope driverScope;

        internal CancelModalDialog(DriverScope driverScope, Driver driver, Options options) : base(driver, driverScope, options)
        {
            this.driverScope = driverScope;
        }

        public override void Act()
        {
            Driver.CancelModalDialog(driverScope);
        }
    }
}