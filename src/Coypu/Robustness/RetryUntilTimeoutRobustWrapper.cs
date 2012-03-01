using System;
using System.Diagnostics;
using System.Threading;
using Coypu.Actions;
using Coypu.Queries;

namespace Coypu.Robustness
{
    public class RetryUntilTimeoutRobustWrapper : RobustWrapper
    {

        public void TryUntil(DriverAction tryThis, Query<bool> until, TimeSpan overrallTimeout)
        {
            var outcome = Robustly(new ActionSatisfiesPredicateQuery(tryThis,until,overrallTimeout,until.RetryInterval));
            if (!outcome)
                throw new MissingHtmlException("Timeout from TryUntil: the page never reached the required state.");
        }

        public bool ZeroTimeout { get; set; }

        public TResult Robustly<TResult>(Query<TResult> query)
        {
            var interval = query.RetryInterval;
            //var timeout = ZeroTimeout ? TimeSpan.Zero : query.Timeout;
            var timeout = query.Timeout;
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
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Timeout: " + timeout);
                    Console.WriteLine("Interval: " + interval);
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
            Console.WriteLine("Elapsed: " + elapsedTimeToNextCall);
            var timeoutReached = elapsedTimeToNextCall >= timeout;

            return timeoutReached;
        }
    }
}