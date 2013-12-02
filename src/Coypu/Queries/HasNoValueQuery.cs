namespace Coypu.Queries
{
    internal class HasNoValueQuery : ElementScopeQuery<bool>
    {
        private readonly string text;
        public override bool ExpectedResult { get { return true; } }

        internal HasNoValueQuery(ElementScope scope, string text, Options options)
            : base(scope, options)
        {
            this.text = text;
        }

        public override bool Run()
        {
            return ElementScope.Find().Value != text;
        }
    }
}