namespace Coypu.Actions
{
    internal class Select : DriverAction
    {
        private readonly DriverScope scope;
        private readonly string locator;
        private readonly string option;

        internal Select(Driver driver, DriverScope scope, string locator, string option) 
            : base(driver,scope.Timeout,scope.RetryInterval)
        {
            this.scope = scope;
            this.locator = locator;
            this.option = option;
        }

        public override void Act()
        {
            Driver.Select(Driver.FindField(locator, scope),option);
        }
    }
}