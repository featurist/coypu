using System;

namespace Coypu.Actions
{
    public class LambdaBrowserAction : BrowserAction
    {
        private readonly Action action;

        public LambdaBrowserAction(Action action, Options options)
            : base(options)
        {
            this.action = action;
        }

        public override void Act()
        {
            action();
        }
    }
}