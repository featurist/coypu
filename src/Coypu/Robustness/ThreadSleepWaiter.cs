using System;
using System.Threading;

namespace Coypu.Robustness
{
    public class ThreadSleepWaiter : Waiter
    {
        public void Wait(TimeSpan duration)
        {
            Thread.Sleep(duration);
        }
    }
}