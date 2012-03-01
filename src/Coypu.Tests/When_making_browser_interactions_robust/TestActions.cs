using System;
using Coypu.Actions;

namespace Coypu.Tests.When_making_browser_interactions_robust
{
    public class CountTriesAction : DriverAction
    {
        public CountTriesAction(TimeSpan timeout, TimeSpan retryInterval) : base(null,timeout,retryInterval){}

        public int Tries { get; private set; }

        public override void Act()
        {
            Tries++;
        }
    }
}