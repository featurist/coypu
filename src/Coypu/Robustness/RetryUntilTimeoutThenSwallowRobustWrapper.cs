using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Coypu.Robustness
{
    using Coypu.Actions;
    using Coypu.Queries;

    public class RetryUntilTimeoutThenSwallowRobustWrapper : RetryUntilTimeoutRobustWrapper
    {
        public override void TryUntil(BrowserAction tryThis, PredicateQuery until, TimeSpan overrallTimeout, TimeSpan waitBeforeRetry)
        {
            try
            {
                var outcome = Robustly(new ActionSatisfiesPredicateQuery(tryThis, until, overrallTimeout, until.RetryInterval, waitBeforeRetry, this));
                if (!outcome)
                    throw new MissingHtmlException("Timeout from TryUntil: the page never reached the required state.");
            }
            catch (MissingHtmlException)
            {   
            }
        }
    }
}
