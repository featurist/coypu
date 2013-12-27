using System;

namespace Coypu.Queries
{
    internal abstract class DriverScopeQuery<T> : Query<T>
    {
        protected DriverScope DriverScope { get; private set; }
        public Options Options { get; private set; }

        internal DriverScopeQuery(DriverScope driverScope, Options options)
        {
            Options = options;
            DriverScope = driverScope;
        }

        public abstract T Run();
        public abstract T ExpectedResult { get; }
    }
}