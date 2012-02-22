namespace Coypu.Queries
{
    internal class HasCssQuery : Query<bool>
    {
        private readonly Driver driver;
        private readonly DriverScope scope;
        private readonly string cssSelector;
        public object ExpectedResult { get { return true; } }
        public bool Result { get; private set; }

        protected internal HasCssQuery(Driver driver, DriverScope scope, string cssSelector)
        {
            this.driver = driver;
            this.scope = scope;
            this.cssSelector = cssSelector;
        }

        public void Run()
        {
            Result = driver.HasCss(cssSelector, scope);
        }
    }
}