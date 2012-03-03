namespace Coypu.Actions
{
    internal class Hover : DriverAction
    {
        private readonly DriverScope driverScope;
        private readonly Driver driver;

        internal Hover(DriverScope driverScope, Driver driver, Options options)
            : base(null, options)
        {
            this.driverScope = driverScope;
            this.driver = driver;
        }

        public override void Act()
        {
            var element = driverScope.Now();
            driver.Hover(element);
        }
    }
}