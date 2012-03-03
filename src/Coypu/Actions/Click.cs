namespace Coypu.Actions
{
    internal class Click : DriverAction
    {
        private readonly Driver driver;
        private readonly DriverScope driverScope;

        internal Click(DriverScope driverScope, Driver driver, Options options) : base(null,options)
        {
            this.driverScope = driverScope;
            this.driver = driver;
        }

        public override void Act()
        {
            var element = driverScope.Now();
            driver.Click(element);
        }
    }
}