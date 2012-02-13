using Coypu.Robustness;

namespace Coypu.Actions
{
    internal class Select : DriverAction
    {
        private readonly Driver driver;
        private readonly DriverScope scope;
        private readonly string locator;
        private readonly string option;

        internal Select(Driver driver, DriverScope scope, string locator, string option)
        {
            this.driver = driver;
            this.scope = scope;
            this.locator = locator;
            this.option = option;
        }

        public void Act()
        {
            driver.Select(driver.FindField(locator, scope),option);
        }
    }
}