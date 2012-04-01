namespace Coypu.Actions
{
    internal class Click : DriverAction
    {
        private readonly DriverScope driverScope;

        internal Click(DriverScope driverScope, Driver driver, Options options) : base(driver, options)
        {
            this.driverScope = driverScope;
        }

        public override void Act()
        {
            var element = driverScope.Now();
            Driver.Click(element);
        }
    }
}