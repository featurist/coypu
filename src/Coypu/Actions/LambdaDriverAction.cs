using System;

namespace Coypu.Actions
{
    public class LambdaDriverAction : DriverAction
    {
        private readonly Action action;

        public LambdaDriverAction(Action action, TimeSpan timeout, TimeSpan retryInterval)
            : base(null, timeout, retryInterval)
        {
            this.action = action;
        }

        public override void Act()
        {
            action();
        }
    }
}