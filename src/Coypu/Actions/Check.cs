using System;

namespace Coypu.Actions
{
    internal class Check : DriverAction
    {
        private readonly DriverScope scope;
        private readonly string locator;

        internal Check(Driver driver, DriverScope scope, string locator) : base (driver, scope.Timeout)
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