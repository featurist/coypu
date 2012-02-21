using System;
using Coypu.Actions;
using Coypu.Finders;
using Coypu.Predicates;
using Coypu.Queries;

namespace Coypu.Robustness
{
    internal interface RobustWrapper
    {
        T Query<T>(Query<T> query);
        void TryUntil(DriverAction tryThis, Predicate until, TimeSpan waitBeforeRetry);
        Element RobustlyFind(ElementFinder elementFinder);
        void RobustlyDo(DriverAction action);
    }
}