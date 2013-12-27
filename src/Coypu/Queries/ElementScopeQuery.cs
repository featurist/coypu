namespace Coypu.Queries
{
    internal abstract class ElementScopeQuery<T> : Query<T>
    {
        public Options Options { get; private set; }
        protected DriverScope Scope { get; private set; }

        internal ElementScopeQuery(DriverScope scope, Options options)
        {
            Options = options;
            Scope = scope;
        }

        public abstract T Run();
        public abstract T ExpectedResult { get; }
    }
}