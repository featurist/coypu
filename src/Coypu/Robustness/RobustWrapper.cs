using System;
using Coypu.Actions;
using Coypu.Predicates;
using Coypu.Queries;

namespace Coypu.Robustness
{
    internal interface RobustWrapper
    {
        T Robustly<T>(Query<T> query);
        void TryUntil(DriverAction tryThis, Predicate until, TimeSpan waitBeforeRetry, TimeSpan overallTimeout);
        bool ZeroTimeout { get; set; }
    }
}