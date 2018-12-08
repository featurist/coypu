namespace Coypu.Queries
{
    internal abstract class DriverScopeQuery<T> : Query<T>
    {
        public DriverScope Scope { get; }
        public Options Options { get; }

        internal DriverScopeQuery(DriverScope driverScope, Options options)
        {
            Scope = driverScope;
            Options = options;
        }

        public abstract T Run();
        public abstract object ExpectedResult { get; }
    }
}