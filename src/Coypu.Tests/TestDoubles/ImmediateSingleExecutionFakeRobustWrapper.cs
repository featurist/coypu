using System;
using Coypu.Actions;
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

        public void TryUntil(BrowserAction tryThis, Query<bool> until, TimeSpan overallTimeout, TimeSpan waitBeforeRetry)
        {
            tryThis.Act();
        }

        public bool ZeroTimeout{get; set; }
        public void SetOverrideTimeout(TimeSpan timeout)
        {
        }

        public void ClearOverrideTimeout()
        {
        }
    }
}