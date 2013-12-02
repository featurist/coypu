namespace Coypu.Queries
{
    internal class HasValueQuery : ElementScopeQuery<bool>
    {
        private readonly string text;
        public override bool ExpectedResult { get { return true; } }

        internal HasValueQuery(ElementScope scope, string text, Options options)
            : base(scope, options)
        {
            this.text = text;
        }

        public override bool Run()
        {
            return ElementScope.Find().Value == text;
        }
    }
}