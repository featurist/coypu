namespace Coypu.Actions
{
    internal class AcceptModalDialog : DriverAction
    {
        private readonly DriverScope driverScope;

        internal AcceptModalDialog(DriverScope driverScope, Driver driver, Options options) : base(driver, options)
        {
            this.driverScope = driverScope;
        }

        public override void Act()
        {
            Driver.AcceptModalDialog(driverScope);
        }
    }
}