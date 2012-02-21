using System;

namespace Coypu.Actions
{
    public class LambdaDriverAction : DriverAction
    {
        private readonly Action action;

        public LambdaDriverAction(Action action)
        {
            this.action = action;
        }

        public void Act()
        {
            action();
        }
    }
}