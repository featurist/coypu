namespace Coypu.Queries
{
    internal abstract class DriverScopeQuery<T> : Query<T>
    {
        public DriverScope Scope { get; private set; }
        public Options Options { get; private set; }

        internal DriverScopeQuery(DriverScope driverScope, Options options)
        {
            Scope = driverScope;
            Options = options;
        }

        public abstract T Run();
        public abstract object ExpectedResult { get; }
    }
}