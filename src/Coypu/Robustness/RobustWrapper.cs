using System;
using Coypu.Actions;
using Coypu.Queries;

namespace Coypu.Robustness
{
    internal interface RobustWrapper
    {
        T Robustly<T>(Query<T> query);
        void TryUntil(DriverAction tryThis, Query<bool> until, TimeSpan overallTimeout, TimeSpan waitBeforeRetry);
        bool ZeroTimeout { get; set; }
        void SetOverrideTimeout(TimeSpan timeout);
        void ClearOverrideTimeout();
    }
}