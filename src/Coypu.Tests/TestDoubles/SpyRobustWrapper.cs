using System;
using System.Collections.Generic;
using Coypu.Finders;
using Coypu.Queries;
using Coypu.Robustness;

namespace Coypu.Tests.TestDoubles
{
    public class SpyRobustWrapper : RobustWrapper
    {
        public IList<Action> DeferredActions = new List<Action>();
        public IList<object> DeferredFunctions = new List<object>();
        public IList<ElementFinder> DeferredFinders = new List<ElementFinder>();
        public IList<DriverAction> DeferredDriverActions = new List<DriverAction>();
        public IList<object> DeferredQueries = new List<object>();
        public IList<TryUntilArgs> DeferredTryUntils = new List<TryUntilArgs>();

        private readonly IDictionary<Type, object> stubbedResults = new Dictionary<Type, object>();
        private readonly IDictionary<Element, object> stubbedFinds = new Dictionary<Element, object>();
        private readonly IDictionary<object, object> stubbedQueryResult = new Dictionary<object, object>();

        public void Robustly(Action action)
        {
            DeferredActions.Add(action);
        }

        public TResult Robustly<TResult>(Func<TResult> function)
        {
            DeferredFunctions.Add(function);
            return (TResult)stubbedResults[typeof(TResult)];
        }

        public T Query<T>(Func<T> query, T expecting)
        {
            DeferredQueries.Add(query);
            return (T)stubbedQueryResult[expecting];
        }

        public bool Query(ElementPresenceQuery query)
        {
            //TODO
            throw new NotImplementedException();
        }

        public void TryUntil(Action tryThis, Func<bool> until, TimeSpan waitBeforeRetry)
        {
            DeferredTryUntils.Add(new TryUntilArgs(tryThis, until, waitBeforeRetry));
        }

        public Element RobustlyFind(ElementFinder elementFinder)
        {
            DeferredFinders.Add(elementFinder);
            return (Element) stubbedResults[typeof (Element)];
        }

        public void RobustlyDo(DriverAction action)
        {
            DeferredDriverActions.Add(action);
        }

        public void AlwaysReturnFromRobustly(Type type, object result)
        {
            stubbedResults.Add(type,result);
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