using System;

namespace Coypu.Queries
{
    internal abstract class DriverScopeQuery<T> : Query<T>
    {
        protected DriverScope DriverScope { get; private set; }
        public TimeSpan Timeout { get; private set; }
        public TimeSpan RetryInterval { get; private set; }

        internal DriverScopeQuery(DriverScope driverScope, Options options)
        {
            DriverScope = driverScope;
            Timeout = options.Timeout;
            RetryInterval = options.RetryInterval;
        }

        public abstract void Run();
        public abstract object ExpectedResult { get; }
        public T Result { get; set; }
    }
}