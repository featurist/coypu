using System;

namespace Coypu.Timing
{
    public interface Waiter
    {
        void Wait(TimeSpan duration);
    }
}