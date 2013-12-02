namespace Coypu.Queries
{
    internal class HasContentQuery : DriverScopeQuery<bool>
    {
        private readonly string text;
        public override bool ExpectedResult { get { return true; } }

        internal HasContentQuery(DriverScope scope, string text, Options options) : base(scope,options)
        {
            this.text = text;
        }

        public override bool Run()
        {
            return DriverScope.Find().Text.Contains(text);
        }
    }
}