using System;

namespace Coypu.Queries
{
    internal abstract class ElementScopeQuery<T> : Query<T>
    {
        protected readonly Options Options;
        protected DriverScope Scope { get; private set; }
        public TimeSpan Timeout { get; private set; }
        public TimeSpan RetryInterval { get; private set; }

        internal ElementScopeQuery(DriverScope scope, Options options)
        {
            Options = options;
            Scope = scope;
            Timeout = options.Timeout;
            RetryInterval = options.RetryInterval;
        }

        public abstract T Run();
        public abstract T ExpectedResult { get; }
    }
}