namespace Coypu.Actions
{
    internal class Hover : DriverAction
    {
        private readonly DriverScope driverScope;

        internal Hover(DriverScope driverScope, Driver driver, Options options)
            : base(driver, options)
        {
            this.driverScope = driverScope;
        }

        public override void Act()
        {
            var element = driverScope.Find();
            Driver.Hover(element);
        }
    }
}