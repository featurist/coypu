using System;

namespace Coypu.Robustness
{
    internal interface Waiter
    {
        void Wait(TimeSpan duration);
    }
}