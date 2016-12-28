using System;
using Coypu.Actions;
using Coypu.Queries;
using Coypu.Timing;

namespace Coypu.Tests.TestDoubles
{
    public class ImmediateSingleExecutionFakeTimingStrategy : TimingStrategy
    {
        public T Synchronise<T>(Query<T> query)
        {
            return query.Run();
        }

        public void TryUntil(BrowserAction tryThis, PredicateQuery until, Options options)
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