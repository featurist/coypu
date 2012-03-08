using Coypu.Finders;
using Coypu.Robustness;

namespace Coypu.Actions
{
    internal class WaitThenClick : DriverAction
    {
        private readonly Options options;
        private readonly Waiter waiter;
        private readonly ElementFinder elementFinder;

        internal WaitThenClick(Driver driver, Options options, Waiter waiter, ElementFinder elementFinder)
            : base(driver, options)
        {
            this.options = options;
            this.waiter = waiter;
            this.elementFinder = elementFinder;
        }

        public override void Act()
        {
            var element = elementFinder.Find();
            waiter.Wait(options.WaitBeforeClick);
            Driver.Click(element);
        }
    }
}