using Coypu.Finders;
using Coypu.Robustness;

namespace Coypu
{
    internal class WaitThenClick : DriverAction
    {
        private readonly Driver driver;
        private readonly Waiter waiter;
        private readonly ElementFinder elementFinder;

        internal WaitThenClick(Driver driver, Waiter waiter, ElementFinder elementFinder)
        {
            this.driver = driver;
            this.waiter = waiter;
            this.elementFinder = elementFinder;
        }

        public void Act()
        {
            var element = elementFinder.Find();
            waiter.Wait(Configuration.WaitBeforeClick);
            driver.Click(element);
        }
    }
}