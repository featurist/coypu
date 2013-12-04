namespace Coypu.Actions
{
    internal class Select : DriverAction
    {
        private readonly DriverScope scope;
        private readonly string locator;
        private readonly string optionToSelect;
        private readonly Options options;

        internal Select(Driver driver, DriverScope scope, string locator, string optionToSelect, Options options)
            : base(driver, options)
        {
            this.scope = scope;
            this.locator = locator;
            this.optionToSelect = optionToSelect;
            this.options = options;
        }

        public override void Act()
        {
            Driver.Select(scope.FindField(locator, options), optionToSelect);
        }
    }
}