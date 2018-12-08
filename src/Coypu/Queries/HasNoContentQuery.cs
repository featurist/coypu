namespace Coypu.Queries
{
    internal class HasNoContentQuery : DriverScopeQuery<bool>
    {
        private readonly string text;
        public override object ExpectedResult => true;

        protected internal HasNoContentQuery(DriverScope scope, string text, Options options) : base(scope,options)
        {
            this.text = text;
        }

        public override bool Run()
        {
            return !Scope.FindElement().Text.Contains(text);
        }
    }
}