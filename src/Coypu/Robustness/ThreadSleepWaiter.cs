using System;
using System.Diagnostics;
using System.Threading;

namespace Coypu.Robustness
{
    internal class ThreadSleepWaiter : Waiter
    {
        public void Wait(TimeSpan duration)
        {
            Thread.Sleep(duration);
        }
    }
}