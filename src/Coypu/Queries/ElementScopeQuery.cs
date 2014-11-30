namespace Coypu.Queries
{
    internal abstract class ElementScopeQuery<T> : Query<T>
    {
        public Options Options { get; private set; }
        public DriverScope Scope { get { return DriverScope; } }
        public DriverScope DriverScope { get; private set; }

        internal ElementScopeQuery(DriverScope scope, Options options)
        {
            DriverScope = scope;
            Options = options;
        }

        public abstract T Run();
        public abstract T ExpectedResult { get; }
    }
}