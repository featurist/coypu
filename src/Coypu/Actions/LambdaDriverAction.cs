using System;

namespace Coypu.Actions
{
    public class LambdaDriverAction : DriverAction
    {
        private readonly Action action;

        public LambdaDriverAction(Action action, TimeSpan timeout) : base(null, timeout)
        {
            this.action = action;
        }

        public override void Act()
        {
            action();
        }
    }
}