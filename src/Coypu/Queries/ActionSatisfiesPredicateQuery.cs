using System;
using Coypu.Actions;
using Coypu.Timing;

namespace Coypu.Queries
{
    internal class ActionSatisfiesPredicateQuery : Query<bool>
    {
        private readonly BrowserAction tryThis;
        private readonly PredicateQuery until;
        private readonly TimeSpan waitBeforeRetry;
        private readonly TimingStrategy timingStrategy;
        public TimeSpan RetryInterval { get; private set; }

        public TimeSpan Timeout { get; private set; }

        internal ActionSatisfiesPredicateQuery(BrowserAction tryThis, PredicateQuery until, TimeSpan overallTimeout, TimeSpan retryInterval, TimeSpan waitBeforeRetry, TimingStrategy timingStrategy)
        {
            this.tryThis = tryThis;
            this.until = until;
            this.waitBeforeRetry = waitBeforeRetry;
            this.timingStrategy = timingStrategy;
            RetryInterval = retryInterval;
            Timeout = overallTimeout;
        }

        public bool Run()
        {
            tryThis.Act();

            try
            {
                timingStrategy.SetOverrideTimeout(waitBeforeRetry);
                return until.Predicate();
            }
            finally
            {
                timingStrategy.ClearOverrideTimeout();
            }
        }

        public bool ExpectedResult
        {
            get { return true; }
        }
    }
}