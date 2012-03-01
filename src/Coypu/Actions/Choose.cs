using System;

namespace Coypu.Actions
{
    internal class Choose : DriverAction
    {
        private readonly DriverScope scope;
        private readonly string locator;

        internal Choose(Driver driver, DriverScope scope, string locator) : base(driver,scope.Timeout, scope.RetryInterval)
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