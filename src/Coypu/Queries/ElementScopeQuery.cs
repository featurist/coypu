namespace Coypu.Queries
{
    internal abstract class ElementScopeQuery<T> : Query<T>
    {
        public Options Options { get; }
        public DriverScope Scope => DriverScope;
        public DriverScope DriverScope { get; }

        internal ElementScopeQuery(DriverScope scope, Options options)
        {
            DriverScope = scope;
            Options = options;
        }

        public abstract T Run();
        public abstract object ExpectedResult { get; }
    }
}