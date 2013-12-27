using System;
using Coypu.Actions;
using Coypu.Queries;

namespace Coypu.Timing
{
    public interface TimingStrategy
    {
        T Synchronise<T>(Query<T> query);
        void TryUntil(BrowserAction tryThis, PredicateQuery until, Options options);
        bool ZeroTimeout { get; set; }
        void SetOverrideTimeout(TimeSpan timeout);
        void ClearOverrideTimeout();
    }
}