using System;
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
            var startTime = DateTime.Now;
            while (true)
            {
                try
                {
                    var result = function();
                    if (ExpectedResultNotFoundWithinTimeout(expectedResult, result, startTime))
                    {
                        WaitForInterval(interval);
                        continue;
                    }
                    return result;
                }
                catch (NotSupportedException) { throw; }
                catch (Exception)
                {
                    if (TimeoutExceeded(startTime))
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

        private bool ExpectedResultNotFoundWithinTimeout<TResult>(object expectedResult, TResult result, DateTime startTime)
        {
            return expectedResult != null && !result.Equals(expectedResult) && !TimeoutExceeded(startTime);
        }

        private bool TimeoutExceeded(DateTime startTime)
        {
            return DateTime.Now - startTime >= Configuration.Timeout;
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