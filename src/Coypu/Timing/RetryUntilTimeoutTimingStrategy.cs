using System;
using System.Diagnostics;
using System.Threading;
using Coypu.Actions;
using Coypu.Queries;
using OpenQA.Selenium;

namespace Coypu.Timing
{
    public class RetryUntilTimeoutTimingStrategy : TimingStrategy
    {
        public void TryUntil(BrowserAction tryThis, PredicateQuery until, Options options)
        {
            var outcome = Synchronise(new ActionSatisfiesPredicateQuery(tryThis, until, options, this));
            if (!outcome)
                throw new MissingHtmlException("Timeout from TryUntil: the page never reached the required state.");
        }

        public bool ZeroTimeout { get; set; }
        private TimeSpan? overrideTimeout;

        public void SetOverrideTimeout(TimeSpan timeout)
        {
            overrideTimeout = timeout;
        }

        public void ClearOverrideTimeout()
        {
            overrideTimeout = null;
        }

        public TResult Synchronise<TResult>(Query<TResult> query)
        {
            var interval = query.Options.RetryInterval;
            var stopWatch = Stopwatch.StartNew();
            while (true)
            {

                try
                {
                    var result = query.Run();
                    if (ExpectedResultNotFoundWithinTimeout(query.ExpectedResult, result, stopWatch, Timeout(query),
                                                            interval))
                    {
                        WaitForInterval(interval);
                        continue;
                    }
                    return result;
                }
                catch (NotSupportedException)
                {
                    throw;
                }
                catch (UnhandledAlertException)
                {
                    // Could come from anywhere. Throw straight up rather than retrying as requires user interaction.
                    throw;
                }
                catch (FinderException ex)
                {
                    if (TimeoutReached(stopWatch, Timeout(query), interval))
                        throw ex;
            
                    WaitForInterval(interval);
                }
                catch (Exception ex)
                {
                    MarkAsStale(query);
                    if (TimeoutReached(stopWatch, Timeout(query), interval))
                        throw ex;
            
                    WaitForInterval(interval);
                }
            }
        }

        private static void MarkAsStale<TResult>(Query<TResult> query)
        {
            if (query.Scope == null)
                return;

            if (query.Scope.Stale && query.Scope.OuterScope != null)
                query.Scope.OuterScope.Stale = true;
            else
                query.Scope.Stale = true;
        }

        private TimeSpan Timeout<TResult>(Query<TResult> query)
        {
            TimeSpan timeout;
            if (ZeroTimeout)
            {
                timeout = TimeSpan.Zero;
            }
            else if (overrideTimeout.HasValue)
            {
                timeout = overrideTimeout.Value;
            }
            else
            {
                timeout = query.Options.Timeout;
            }
            return timeout;
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