using Coypu.Robustness;

namespace Coypu.Actions
{
    internal class Choose : DriverAction
    {
        private readonly Driver driver;
        private readonly DriverScope scope;
        private readonly string locator;

        internal Choose(Driver driver, DriverScope scope, string locator)
        {
            this.driver = driver;
            this.scope = scope;
            this.locator = locator;
        }

        public void Act()
        {
            driver.Choose(driver.FindField(locator, scope));
        }
    }
}