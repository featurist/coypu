using System;

namespace Coypu.Queries
{
    internal abstract class ElementScopeQuery<T> : Query<T>
    {
        protected readonly Options Options;
        protected ElementScope ElementScope { get; private set; }
        public TimeSpan Timeout { get; private set; }
        public TimeSpan RetryInterval { get; private set; }

        internal ElementScopeQuery(ElementScope driverScope, Options options)
        {
            Options = options;
            ElementScope = driverScope;
            Timeout = options.Timeout;
            RetryInterval = options.RetryInterval;
        }

        public abstract T Run();
        public abstract T ExpectedResult { get; }
    }
}