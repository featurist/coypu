using System;
using Coypu.Actions;
using Coypu.Predicates;
using Coypu.Queries;
using Coypu.Robustness;

namespace Coypu.Tests.TestDoubles
{
    public class ImmediateSingleExecutionFakeRobustWrapper : RobustWrapper
    {
        public T Robustly<T>(Query<T> query)
        {
            query.Run();
            return query.Result;
        }

        public void TryUntil(DriverAction tryThis, Predicate until, TimeSpan waitBeforeRetry, TimeSpan overallTimeout)
        {
            tryThis.Act();
        }
    }
}