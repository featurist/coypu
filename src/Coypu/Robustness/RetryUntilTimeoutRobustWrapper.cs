using System;
using System.Diagnostics;
using System.Threading;

namespace Coypu.Robustness
{
    public class RetryUntilTimeoutRobustWrapper : RobustWrapper
    {
        public void Robustly(Action action)
        {
            Robustly<object>(() =>
                         {
                             action();
                             return null;
                         });
        }

        public TResult Robustly<TResult>(Func<TResult> function)
        {
            return Robustly(function, null);
        }

        public T Query<T>(Func<T> query, T expecting)
        {
            return Robustly(query, expecting);
        }

        public TResult Robustly<TResult>(Func<TResult> function, object expectedResult)
        {
            var interval = Configuration.RetryInterval;
            var timeout = Configuration.Timeout;
            var stopWatch = Stopwatch.StartNew();
            while (true)
            {
                try
                {
                    var result = function();
                    if (ExpectedResultNotFoundWithinTimeout(expectedResult, result, stopWatch, timeout, interval))
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
            return TimeSpan.FromMilliseconds(stopWatch.ElapsedMilliseconds) + interval >= timeout;
        }

        public void TryUntil(Action tryThis, Func<bool> until, TimeSpan retryAfter)
        {
            var outcome = Robustly(() =>
                                        {
                                            tryThis();
                                            bool result;
                                            var outerTimeout = Configuration.Timeout;
                                            try
                                            {
                                                Configuration.Timeout = retryAfter;
                                                result = until();
                                            }
                                            finally
                                            {
                                                Configuration.Timeout = outerTimeout;
                                            }
                                            return result;
                                        }
                                    , true);
            if (!outcome)
                throw new MissingHtmlException("Timeout from TryUntil: the page never reached the required state.");
        }
    }
}