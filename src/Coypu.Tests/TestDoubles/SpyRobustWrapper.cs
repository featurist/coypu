using System;
using System.Collections.Generic;
using System.Linq;
using Coypu.Finders;
using Coypu.Queries;
using Coypu.Robustness;

namespace Coypu.Tests.TestDoubles
{
    public class SpyRobustWrapper : RobustWrapper
    {
        internal IList<Action> DeferredActions = new List<Action>();
        internal IList<object> DeferredFunctions = new List<object>();
        internal IList<ElementFinder> DeferredFinders = new List<ElementFinder>();
        internal IList<DriverAction> DeferredDriverActions = new List<DriverAction>();
        internal IList<object> DeferredQueries = new List<object>();
        internal IList<TryUntilArgs> DeferredTryUntils = new List<TryUntilArgs>();

        private readonly IList<object> stubbedResults = new List<object>();
        private readonly IDictionary<Element, object> stubbedFinds = new Dictionary<Element, object>();
        private readonly IDictionary<object, object> stubbedQueryResult = new Dictionary<object, object>();
        internal IList<object> QueriesRan = new List<object>();

        public void Robustly(Action action)
        {
            DeferredActions.Add(action);
        }

        public TResult Robustly<TResult>(Func<TResult> function)
        {
            DeferredFunctions.Add(function);
            return stubbedResults.OfType<TResult>().FirstOrDefault();
        }

        public T Query<T>(Func<T> query, T expecting)
        {
            DeferredQueries.Add(query);
            return (T)stubbedQueryResult[expecting];
        }

        public T Query<T>(Query<T> query)
        {
            QueriesRan.Add(query);
            return default(T);
        }

        public void TryUntil(Action tryThis, Func<bool> until, TimeSpan waitBeforeRetry)
        {
            DeferredTryUntils.Add(new TryUntilArgs(tryThis, until, waitBeforeRetry));
        }

        public void TryUntil(DriverAction tryThis, DriverPredicate until, TimeSpan waitBeforeRetry)
        {
            DeferredTryUntils.Add(new TryUntilArgs(tryThis, until, waitBeforeRetry));
        }

        public Element RobustlyFind(ElementFinder elementFinder)
        {
            DeferredFinders.Add(elementFinder);
            return stubbedResults.OfType<Element>().FirstOrDefault();
        }

        public void RobustlyDo(DriverAction action)
        {
            DeferredDriverActions.Add(action);
        }

        public void AlwaysReturnFromRobustly<T>(T result)
        {
            stubbedResults.Add(result);
        }

        public void StubQueryResult<T>(T expected, T result)
        {
            stubbedQueryResult[expected] = result;
        }

        public void StubQueryResult(bool result)
        {
            stubbedQueryResult[true] = result;
            stubbedQueryResult[false] = result;
        }

        public class TryUntilArgs
        {
            public Action TryThisAction { get; private set; }
            public Func<bool> UntilThisFunction { get; private set; }
            public TimeSpan WaitBeforeRetry { get; private set; }
            protected DriverAction TryThisDriverAction { get; private set; }
            protected DriverPredicate UntilThisPredicate { get; private set; }

            public TryUntilArgs(Action tryThis, Func<bool> until, TimeSpan waitBeforeRetry)
            {
                WaitBeforeRetry = waitBeforeRetry;
                TryThisAction = tryThis;
                UntilThisFunction = until;
            }

            public TryUntilArgs(DriverAction tryThis, DriverPredicate until, TimeSpan waitBeforeRetry)
            {
                WaitBeforeRetry = waitBeforeRetry;
                TryThisDriverAction = tryThis;
                UntilThisPredicate = until;
            }
        }
    }
}