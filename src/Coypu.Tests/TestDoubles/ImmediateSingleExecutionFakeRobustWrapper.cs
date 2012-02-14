using System;
using Coypu.Finders;
using Coypu.Predicates;
using Coypu.Queries;
using Coypu.Robustness;

namespace Coypu.Tests.TestDoubles
{
    public class ImmediateSingleExecutionFakeRobustWrapper : RobustWrapper
    {
        public void Robustly(Action action)
        {
            action();
        }

        public TResult Robustly<TResult>(Func<TResult> function)
        {
            return function();
        }

        public T Query<T>(Func<T> query, T expectedResult)
        {
            return query();
        }

        public T Query<T>(Query<T> query)
        {
            query.Run();
            return query.Result;
        }

        public void TryUntil(Action tryThis, Func<bool> until, TimeSpan waitBeforeRetry)
        {
            tryThis();
        }

        public void TryUntil(DriverAction tryThis, BrowserSessionPredicate until, TimeSpan waitBeforeRetry)
        {
            tryThis.Act();
        }

        public Element RobustlyFind(ElementFinder elementFinder)
        {
            return elementFinder.Find();
        }

        public void RobustlyDo(DriverAction action)
        {
            action.Act();
        }
    }
}