using Coypu.Robustness;

namespace Coypu.Actions
{
    internal class Uncheck : DriverAction
    {
        private readonly Driver driver;
        private readonly DriverScope scope;
        private readonly string locator;

        internal Uncheck(Driver driver, DriverScope scope, string locator)
        {
            this.driver = driver;
            this.scope = scope;
            this.locator = locator;
        }

        public void Act()
        {
            driver.Uncheck(driver.FindField(locator, scope));
        }
    }
}