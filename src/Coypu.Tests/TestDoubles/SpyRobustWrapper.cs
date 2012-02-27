using System;
using System.Collections.Generic;
using System.Linq;
using Coypu.Actions;
using Coypu.Predicates;
using Coypu.Queries;
using Coypu.Robustness;

namespace Coypu.Tests.TestDoubles
{
    public class SpyRobustWrapper : RobustWrapper
    {
        internal IList<TryUntilArgs> DeferredTryUntils = new List<TryUntilArgs>();

        private readonly IList<object> stubbedResults = new List<object>();
        private readonly IDictionary<object, object> stubbedQueryResult = new Dictionary<object, object>();
        private readonly IList<object> queriesRan = new List<object>();
        public static readonly object NO_EXPECTED_RESULT = new object();

        public IEnumerable<Query<T>> QueriesRan<T>()
        {
            return queriesRan.OfType<Query<T>>();
        }   

        public T Query<T>(Query<T> query)
        {
            queriesRan.Add(query);
            return (T)stubbedQueryResult[query.ExpectedResult ?? NO_EXPECTED_RESULT];
        }

        public void TryUntil(DriverAction tryThis, Predicate until, TimeSpan waitBeforeRetry, TimeSpan overallTimeout)
        {
            DeferredTryUntils.Add(new TryUntilArgs(tryThis, until, waitBeforeRetry, overallTimeout));
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
            public TimeSpan OverallTimeout { get; private set; }
            public DriverAction TryThisDriverAction { get; private set; }
            public Predicate UntilThisPredicate { get; private set; }

            public TryUntilArgs(DriverAction tryThis, Predicate until, TimeSpan waitBeforeRetry, TimeSpan overallTimeout)
            {
                WaitBeforeRetry = waitBeforeRetry;
                OverallTimeout = overallTimeout;
                TryThisDriverAction = tryThis;
                UntilThisPredicate = until;
            }
        }
    }
}