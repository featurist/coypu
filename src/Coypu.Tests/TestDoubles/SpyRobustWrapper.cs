using System;
using System.Collections.Generic;
using System.Linq;
using Coypu.Actions;
using Coypu.Queries;
using Coypu.Robustness;

namespace Coypu.Tests.TestDoubles
{
    public class SpyRobustWrapper : RobustWrapper
    {
        internal IList<TryUntilArgs> DeferredTryUntils = new List<TryUntilArgs>();

        private object alwaysReturn;
        private readonly IDictionary<object, object> stubbedQueryResult = new Dictionary<object, object>();
        private readonly IList<object> queriesRan = new List<object>();
        public static readonly object NO_EXPECTED_RESULT = new object();

        public IEnumerable<Query<T>> QueriesRan<T>()
        {
            return queriesRan.OfType<Query<T>>();
        }

        public IEnumerable<DriverAction> ActionsRan()
        {
            return queriesRan.OfType<DriverAction>();
        }

        public bool NoQueriesRan { get { return !queriesRan.Any(); } }

        public T Robustly<T>(Query<T> query)
        {
            queriesRan.Add(query);

            if (alwaysReturn != null)
                return (T) alwaysReturn;

            var key = query.ExpectedResult ?? NO_EXPECTED_RESULT;
            
            if (stubbedQueryResult.ContainsKey(key))
                return (T)stubbedQueryResult[key];

            return default(T);
        }

        public void TryUntil(BrowserAction tryThis, Query<bool> until, TimeSpan overallTimeout, TimeSpan waitBeforeRetry)
        {
            DeferredTryUntils.Add(new TryUntilArgs(tryThis, until, overallTimeout, waitBeforeRetry));
        }

        public bool ZeroTimeout { get; set; }
        public void SetOverrideTimeout(TimeSpan timeout)
        {
        }

        public void ClearOverrideTimeout()
        {
        }

        public void AlwaysReturnFromRobustly(object result)
        {
            alwaysReturn = result;
        }

        public void StubQueryResult<T>(T expectedResult, T result)
        {
            stubbedQueryResult[expectedResult] = result;
        }

        public class TryUntilArgs
        {
            public TimeSpan OverallTimeout { get; private set; }
            public TimeSpan WaitBeforeRetry { get; private set; }
            public BrowserAction TryThisBrowserAction { get; private set; }
            public Query<bool> Until { get; private set; }

            public TryUntilArgs(BrowserAction tryThis, Query<bool> until, TimeSpan overallTimeout, TimeSpan waitBeforeRetry)
            {
                OverallTimeout = overallTimeout;
                WaitBeforeRetry = waitBeforeRetry;
                TryThisBrowserAction = tryThis;
                Until = until;
            }
        }
    }
}