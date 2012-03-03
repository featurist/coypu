namespace Coypu.Actions
{
    internal class Check : DriverAction
    {
        private readonly DriverScope scope;
        private readonly string locator;

        internal Check(Driver driver, DriverScope scope, string locator, Options options) : base (driver, options)
        {
            this.scope = scope;
            this.locator = locator;
        }

        public override void Act()
        {
            Driver.Check(Driver.FindField(locator, scope));
        }
    }
}