namespace Coypu.Actions
{
    internal class Uncheck : DriverAction
    {
        private readonly DriverScope scope;
        private readonly string locator;

        internal Uncheck(Driver driver, DriverScope scope, string locator, Options options)
            : base(driver, options)
        {
            this.scope = scope;
            this.locator = locator;
        }

        public override void Act()
        {
            Driver.Uncheck(Driver.FindField(locator, scope));
        }
    }
}