using System;
using System.Collections.Generic;
using Coypu.Robustness;

namespace Coypu.Tests.TestDoubles
{
    public class SpyRobustWrapper : RobustWrapper
    {
        public IList<Action> DeferredActions = new List<Action>();
        public IList<object> DeferredFunctions = new List<object>();
        public IList<object> DeferredQueries = new List<object>();
        public IList<TryUntilArgs> DeferredTryUntils = new List<TryUntilArgs>();
        
        private readonly IDictionary<Type,object> stubbedResults = new Dictionary<Type, object>();
        private readonly IDictionary<object, object> stubbedWaitForResult = new Dictionary<object, object>();

        public void Robustly(Action action)
        {
            DeferredActions.Add(action);
        }

        public TResult Robustly<TResult>(Func<TResult> function)
        {
            DeferredFunctions.Add(function);
            return stubbedResults.ContainsKey(typeof(TResult)) ? (TResult) stubbedResults[typeof(TResult)] : default(TResult);
        }

        public T Query<T>(Func<T> query, T expecting)
        {
            DeferredQueries.Add(query);
            return stubbedWaitForResult.ContainsKey(expecting) ? (T) stubbedWaitForResult[expecting] : default(T);
        }

        public void TryUntil(Action tryThis, Func<bool> until, TimeSpan waitBeforeRetry)
        {
            DeferredTryUntils.Add(new TryUntilArgs(tryThis, until, waitBeforeRetry));
        }

        public void AlwaysReturnFromRobustly(Type type, object result)
        {
            stubbedResults.Add(type,result);
        }

        public void AlwaysReturnFromWaitFor(bool expected, bool result)
        {
            stubbedWaitForResult[expected] = result;
        }

        public class TryUntilArgs
        {
            public Action TryThis { get; private set; }
            public Func<bool> Until { get; private set; }
            public TimeSpan WaitBeforeRetry { get; private set; }

            public TryUntilArgs(Action tryThis, Func<bool> until, TimeSpan waitBeforeRetry)
            {
                WaitBeforeRetry = waitBeforeRetry;
                TryThis = tryThis;
                Until = until;
            }
        }
    }
}