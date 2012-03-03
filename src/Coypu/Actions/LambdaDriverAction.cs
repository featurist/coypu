using System;

namespace Coypu.Actions
{
    public class LambdaDriverAction : DriverAction
    {
        private readonly Action action;

        public LambdaDriverAction(Action action, Options options)
            : base(null, options)
        {
            this.action = action;
        }

        public override void Act()
        {
            action();
        }
    }
}