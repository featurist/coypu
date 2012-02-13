using Coypu.Robustness;

namespace Coypu.Actions
{
    internal class Check : DriverAction
    {
        private readonly Driver driver;
        private readonly DriverScope scope;
        private readonly string locator;

        internal Check(Driver driver, DriverScope scope, string locator)
        {
            this.driver = driver;
            this.scope = scope;
            this.locator = locator;
        }

        public void Act()
        {
            driver.Check(driver.FindField(locator, scope));
        }
    }
}