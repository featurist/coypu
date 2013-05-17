using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coypu.Robustness
{
    using System.Diagnostics;

    using Coypu.Actions;
    using Coypu.Queries;

    public class RetryUntilTimeoutThenSwallowRobustWrapper : RetryUntilTimeoutRobustWrapper
    {
        public override TResult Robustly<TResult>(Query<TResult> query)
        {
            var interval = query.RetryInterval;
            var timeout = Timeout(query);
            var stopWatch = Stopwatch.StartNew();
            while (true)
            {
                try
                {
                    var result = query.Run();
                    if (ExpectedResultNotFoundWithinTimeout(query.ExpectedResult, result, stopWatch, timeout, interval))
                    {
                        WaitForInterval(interval);
                        continue;
                    }
                    return result;
                }
                catch (NotSupportedException) { throw; }
                catch (MissingHtmlException)
                {
                    return default(TResult);
                }
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
    }
}
