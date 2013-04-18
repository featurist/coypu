using System;

namespace Coypu.Queries
{
    internal abstract class DriverScopeQuery<T> : Query<T>
    {
        protected readonly Options Options;
        protected DriverScope DriverScope { get; private set; }
        public TimeSpan Timeout { get; private set; }
        public TimeSpan RetryInterval { get; private set; }

        internal DriverScopeQuery(DriverScope driverScope, Options options)
        {
            Options = options;
            DriverScope = driverScope;
            Timeout = options.Timeout;
            RetryInterval = options.RetryInterval;
        }

        public abstract T Run();
        public abstract T ExpectedResult { get; }
    }
}