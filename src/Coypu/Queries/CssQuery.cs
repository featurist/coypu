namespace Coypu.Queries
{
    internal abstract class CssQuery : Query<bool>
    {
        private readonly Driver driver;
        private readonly DriverScope scope;
        private readonly string cssSelector;
        public abstract object ExpectedResult { get; }
        public bool Result { get; private set; }

        protected CssQuery(Driver driver, DriverScope scope, string cssSelector)
        {
            this.driver = driver;
            this.scope = scope;
            this.cssSelector = cssSelector;
        }

        public void Run()
        {
            Result = driver.HasCss(cssSelector, scope) == (bool) ExpectedResult;
        }
    }
}