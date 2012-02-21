using System;
using System.Diagnostics;
using System.Threading;
using Coypu.Actions;
using Coypu.Finders;
using Coypu.Predicates;
using Coypu.Queries;

namespace Coypu.Robustness
{
    public class RetryUntilTimeoutRobustWrapper : RobustWrapper
    {

        public void TryUntil(DriverAction tryThis, Predicate until, TimeSpan waitBeforeRetry)
        {
            var outcome = Query(new ActionSatisfiesPredicateQuery(tryThis,until,waitBeforeRetry));
            if (!outcome)
                throw new MissingHtmlException("Timeout from TryUntil: the page never reached the required state.");
        }

        public TResult Query<TResult>(Query<TResult> query)
        {
            var interval = Configuration.RetryInterval;
            var timeout = Configuration.Timeout;
            var stopWatch = Stopwatch.StartNew();
            while (true)
            {
                try
                {
                    query.Run();
                    var result = query.Result;
                    if (ExpectedResultNotFoundWithinTimeout(query.ExpectedResult, result, stopWatch, timeout, interval))
                    {
                        WaitForInterval(interval);
                        continue;
                    }
                    return result;
                }
                catch (NotSupportedException) { throw; }
                catch (Exception)
                {
                    if (TimeoutReached(stopWatch, timeout, interval))
                    {
                        throw;
                    }
                    WaitForInterval(interval);
                }
            }
        }

        public Element RobustlyFind(ElementFinder elementFinder)
        {
            var interval = Configuration.RetryInterval;
            var timeout = Configuration.Timeout;
            var stopWatch = Stopwatch.StartNew();
            while (true)
            {
                try
                {
                    return elementFinder.Find();
                }
                catch (NotSupportedException) { throw; }
                catch (Exception)
                {
                    if (TimeoutReached(stopWatch, timeout, interval))
                    {
                        throw;
                    }
                    WaitForInterval(interval);
                }
            }
        }

        public void RobustlyDo(DriverAction action)
        {
            var interval = Configuration.RetryInterval;
            var timeout = Configuration.Timeout;
            var stopWatch = Stopwatch.StartNew();
            while (true)
            {
                try
                {
                    action.Act();
                    return;
                }
                catch (NotSupportedException) { throw; }
                catch (Exception)
                {
                    if (TimeoutReached(stopWatch, timeout, interval))
                    {
                        throw;
                    }
                    WaitForInterval(interval);
                }
            }
        }

        private void WaitForInterval(TimeSpan interval)
        {
            Thread.Sleep(interval);
        }

        private bool ExpectedResultNotFoundWithinTimeout<TResult>(object expectedResult, TResult result, Stopwatch stopWatch, TimeSpan timeout, TimeSpan interval)
        {
            return expectedResult != null && !result.Equals(expectedResult) && !TimeoutReached(stopWatch, timeout, interval);
        }

        private bool TimeoutReached(Stopwatch stopWatch, TimeSpan timeout, TimeSpan interval)
        {
            var elapsedTimeToNextCall = TimeSpan.FromMilliseconds(stopWatch.ElapsedMilliseconds) + interval;
            var timeoutReached = elapsedTimeToNextCall >= timeout;

            return timeoutReached;
        }
    }
}