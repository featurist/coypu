using Coypu.Robustness;

namespace Coypu
{
    internal class Clicker
    {
        private readonly Driver driver;
        private readonly Waiter waiter;

        internal Clicker(Driver driver, Waiter waiter)
        {
            this.driver = driver;
            this.waiter = waiter;
        }

        public void FindAndClickButton(string locator)
        {
            var findLink = driver.FindButton(locator);
            waiter.Wait(Configuration.WaitBeforeClick);
            driver.Click(findLink);
        }

        public void FindAndClickLink(string locator)
        {
            var findLink = driver.FindLink(locator);
            waiter.Wait(Configuration.WaitBeforeClick);
            driver.Click(findLink);
        }
    }
}