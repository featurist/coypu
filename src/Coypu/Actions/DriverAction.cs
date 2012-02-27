using System;
using Coypu.Queries;

namespace Coypu.Actions
{
    public abstract class DriverAction : Query<object>
    {
        protected readonly Driver Driver;
        public TimeSpan Timeout { get; private set; }

        protected DriverAction(Driver driver, TimeSpan timeout)
        {
            Driver = driver;
            Timeout = timeout;
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