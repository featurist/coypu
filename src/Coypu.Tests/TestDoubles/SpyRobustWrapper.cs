using System;
using System.Collections.Generic;
using System.Linq;
using Coypu.Actions;
using Coypu.Finders;
using Coypu.Predicates;
using Coypu.Queries;
using Coypu.Robustness;

namespace Coypu.Tests.TestDoubles
{
    public class SpyRobustWrapper : RobustWrapper
    {
        internal IList<ElementFinder> DeferredFinders = new List<ElementFinder>();
        internal IList<DriverAction> DeferredDriverActions = new List<DriverAction>();
        internal IList<TryUntilArgs> DeferredTryUntils = new List<TryUntilArgs>();

        private readonly IList<object> stubbedResults = new List<object>();
        private readonly IDictionary<object, object> stubbedQueryResult = new Dictionary<object, object>();
        internal IList<object> QueriesRan = new List<object>();
        public static readonly object NO_EXPECTED_RESULT = new object();

        public T Query<T>(Query<T> query)
        {
            QueriesRan.Add(query);
            return (T)stubbedQueryResult[query.ExpectedResult ?? NO_EXPECTED_RESULT];
        }

        public void TryUntil(DriverAction tryThis, Predicate until, TimeSpan waitBeforeRetry)
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

        public void StubQueryResult<T>(T expectedResult, T result)
        {
            stubbedQueryResult[expectedResult] = result;
        }

        public class TryUntilArgs
        {
            public TimeSpan WaitBeforeRetry { get; private set; }
            public DriverAction TryThisDriverAction { get; private set; }
            public Predicate UntilThisPredicate { get; private set; }

            public TryUntilArgs(DriverAction tryThis, Predicate until, TimeSpan waitBeforeRetry)
            {
                WaitBeforeRetry = waitBeforeRetry;
                TryThisDriverAction = tryThis;
                UntilThisPredicate = until;
            }
        }
    }
}