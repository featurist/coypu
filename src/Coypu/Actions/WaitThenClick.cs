using System;
using Coypu.Finders;
using Coypu.Robustness;

namespace Coypu.Actions
{
    internal class WaitThenClick : DriverAction
    {
        private readonly TimeSpan waitBeforeClick;
        private readonly Waiter waiter;
        private readonly ElementFinder elementFinder;

        internal WaitThenClick(Driver driver, TimeSpan timeout, TimeSpan waitBeforeClick, TimeSpan retryInterval, Waiter waiter, ElementFinder elementFinder)
            : base(driver,timeout,retryInterval)
        {
            this.waitBeforeClick = waitBeforeClick;
            this.waiter = waiter;
            this.elementFinder = elementFinder;
        }

        public override void Act()
        {
            var element = elementFinder.Find();
            waiter.Wait(waitBeforeClick);
            Driver.Click(element);
        }
    }
}