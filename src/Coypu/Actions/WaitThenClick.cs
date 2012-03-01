using System;
using Coypu.Finders;
using Coypu.Robustness;

namespace Coypu.Actions
{
    internal class WaitThenClick : DriverAction
    {
        private readonly Waiter waiter;
        private readonly ElementFinder elementFinder;

        internal WaitThenClick(Driver driver, TimeSpan timeout, Waiter waiter, ElementFinder elementFinder) : base(driver,timeout)
        {
            this.waiter = waiter;
            this.elementFinder = elementFinder;
        }

        public override void Act()
        {
            var element = elementFinder.Find();
            waiter.Wait(Configuration.WaitBeforeClick);
            Driver.Click(element);
        }
    }
}