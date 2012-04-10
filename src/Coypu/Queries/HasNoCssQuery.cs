namespace Coypu.Queries
{
    internal class HasNoCssQuery : DriverScopeQuery<bool>
    {
        private readonly Driver driver;
        private readonly string cssSelector;

        protected internal HasNoCssQuery(Driver driver, DriverScope scope, string cssSelector, Options options) : base(scope,options)
        {
            this.driver = driver;
            this.cssSelector = cssSelector;
        }

        public override bool ExpectedResult
        {
            get { return true; }
        }

        public override bool Run()
        {
            return !driver.HasCss(cssSelector, DriverScope);
        }
    }
}