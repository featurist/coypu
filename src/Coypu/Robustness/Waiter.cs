using System;

namespace Coypu.Robustness
{
    public interface Waiter
    {
        void Wait(TimeSpan duration);
    }
}