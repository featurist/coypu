using System;
using Coypu.Timing;

namespace Coypu.Actions
{
    internal class ClickAction : DriverAction
    {
        private readonly ElementScope _elementScope;
        private readonly TimeSpan _waitBeforeClick;
        private readonly Waiter _waiter;

        internal ClickAction(ElementScope elementScope,
                             IDriver driver,
                             Options options,
                             Waiter waiter) : base(driver, elementScope, options)
        {
            _waitBeforeClick = options.WaitBeforeClick;
            _elementScope = elementScope;
            _waiter = waiter;
        }

        public override void Act()
        {
            _waiter.Wait(_waitBeforeClick);
            Driver.Click(_elementScope);
        }
    }
}