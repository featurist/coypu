using System;

namespace Coypu.Robustness
{
    public interface RobustWrapper
    {
        void Robustly(Action action);
        TResult Robustly<TResult>(Func<TResult> function);
        T Query<T>(Func<T> query, T expecting);
        void TryUntil(Action tryThis, Func<bool> until, TimeSpan waitBeforeRetry);
        Element RobustlyFind(ElementFinder elementFinder);
        void RobustlyDo(DriverAction action);
    }
}