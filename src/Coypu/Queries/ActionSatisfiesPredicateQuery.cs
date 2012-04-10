using System;
using Coypu.Actions;
using Coypu.Robustness;

namespace Coypu.Queries
{
    internal class ActionSatisfiesPredicateQuery : Query<bool>
    {
        private readonly BrowserAction tryThis;
        private readonly PredicateQuery until;
        private readonly TimeSpan waitBeforeRetry;
        private readonly RobustWrapper robustWrapper;
        public TimeSpan RetryInterval { get; private set; }

        public TimeSpan Timeout { get; private set; }

        internal ActionSatisfiesPredicateQuery(BrowserAction tryThis, PredicateQuery until, TimeSpan overallTimeout, TimeSpan retryInterval, TimeSpan waitBeforeRetry, RobustWrapper robustWrapper)
        {
            this.tryThis = tryThis;
            this.until = until;
            this.waitBeforeRetry = waitBeforeRetry;
            this.robustWrapper = robustWrapper;
            RetryInterval = retryInterval;
            Timeout = overallTimeout;
        }

        public bool Run()
        {
            tryThis.Act();

            try
            {
                robustWrapper.SetOverrideTimeout(waitBeforeRetry);
                return until.Predicate();
            }
            finally
            {
                robustWrapper.ClearOverrideTimeout();
            }
        }

        public bool ExpectedResult
        {
            get { return true; }
        }
    }
}