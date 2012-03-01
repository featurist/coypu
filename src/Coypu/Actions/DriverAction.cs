using System;
using Coypu.Queries;

namespace Coypu.Actions
{
    public abstract class DriverAction : Query<object>
    {
        protected readonly Driver Driver;
        public TimeSpan Timeout { get; private set; }
        public TimeSpan RetryInterval { get; private set; }

        protected DriverAction(Driver driver, TimeSpan timeout, TimeSpan retryInterval)
        {
            Driver = driver;
            Timeout = timeout;
            RetryInterval = retryInterval;
        }

        public abstract void Act();

        public void Run()
        {
            Act();
        }

        public object ExpectedResult
        {
            get { return null; }
        }

        public object Result
        {
            get { return null; }
        }
    }
}