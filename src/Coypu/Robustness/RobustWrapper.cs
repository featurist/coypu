using System;
using Coypu.Finders;
using Coypu.Predicates;
using Coypu.Queries;

namespace Coypu.Robustness
{
    internal interface RobustWrapper
    {
        void Robustly(Action action);
        TResult Robustly<TResult>(Func<TResult> function);
        T Query<T>(Func<T> query, T expecting);
        T Query<T>(Query<T> query);
        void TryUntil(Action tryThis, Func<bool> until, TimeSpan waitBeforeRetry);
        void TryUntil(DriverAction tryThis, BrowserSessionPredicate until, TimeSpan waitBeforeRetry);
        Element RobustlyFind(ElementFinder elementFinder);
        void RobustlyDo(DriverAction action);
    }
}