using System;
using Coypu.Actions;
using Coypu.Robustness;

namespace Coypu.Queries
{
    internal class ActionSatisfiesPredicateQuery : Query<bool>
    {
        private readonly DriverAction tryThis;
        private readonly Query<bool> until;
        private readonly TimeSpan waitBeforeRetry;
        private readonly RobustWrapper robustWrapper;
        public TimeSpan RetryInterval { get; private set; }

        public TimeSpan Timeout { get; private set; }

        internal ActionSatisfiesPredicateQuery(DriverAction tryThis, Query<bool> until, TimeSpan overallTimeout, TimeSpan retryInterval, TimeSpan waitBeforeRetry, RobustWrapper robustWrapper)
        {
            this.tryThis = tryThis;
            this.until = until;
            this.waitBeforeRetry = waitBeforeRetry;
            this.robustWrapper = robustWrapper;
            RetryInterval = retryInterval;
            Timeout = overallTimeout;
        }

        public void Run()
        {
            tryThis.Act();

            try
            {
                robustWrapper.SetOverrideTimeout(waitBeforeRetry);
                until.Run();
            }
            finally
            {
                robustWrapper.ClearOverrideTimeout();
            }
            Result = until.Result;
        }

        public object ExpectedResult
        {
            get { return true; }
        }

        public bool Result { get; private set; }
    }
}