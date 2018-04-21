namespace Coypu.Actions
{
    internal class AcceptModalDialog : DriverAction
    {
        private readonly DriverScope driverScope;

        internal AcceptModalDialog(DriverScope driverScope, IDriver driver, Options options) : base(driver, driverScope, options)
        {
            this.driverScope = driverScope;
        }

        public override void Act()
        {
            Driver.AcceptModalDialog(driverScope);
        }
    }
}