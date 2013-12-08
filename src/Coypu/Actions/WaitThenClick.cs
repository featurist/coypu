using System;
using Coypu.Finders;
using Coypu.Robustness;

namespace Coypu.Actions
{
    internal class WaitThenClick : DriverAction
    {
        private readonly Waiter waiter;
        private readonly ElementFinder elementFinder;
        private readonly TimeSpan waitBeforeClick;

        internal WaitThenClick(Driver driver, Options options, Waiter waiter, ElementFinder elementFinder)
            : base(driver, options)
        {
            waitBeforeClick = options.WaitBeforeClick;
            this.waiter = waiter;
            this.elementFinder = elementFinder;
        }

        public override void Act()
        {
            var element = elementFinder.ResolveQuery();
            waiter.Wait(waitBeforeClick);
            Driver.Click(element);
        }
    }
}