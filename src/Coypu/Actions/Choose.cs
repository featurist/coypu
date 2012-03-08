namespace Coypu.Actions
{
    internal class Choose : DriverAction
    {
        private readonly DriverScope scope;
        private readonly string locator;

        internal Choose(Driver driver, DriverScope scope, string locator, Options options) : base(driver, options)
        {
            this.scope = scope;
            this.locator = locator;
        }

        public override void Act()
        {
            Driver.Choose(Driver.FindField(locator, scope));
        }
    }
}