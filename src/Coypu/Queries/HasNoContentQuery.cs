namespace Coypu.Queries
{
    internal class HasNoContentQuery : DriverScopeQuery<bool>
    {
        private readonly Driver driver;
        private readonly string text;
        public override bool ExpectedResult { get { return true; } }

        protected internal HasNoContentQuery(Driver driver, DriverScope scope, string text, Options options) : base(scope,options)
        {
            this.driver = driver;
            this.text = text;
        }

        public override bool Run()
        {
            return !driver.HasContent(text, DriverScope);
        }
    }
}