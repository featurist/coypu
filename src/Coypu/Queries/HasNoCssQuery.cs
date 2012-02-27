using System;

namespace Coypu.Queries
{
    internal class HasNoCssQuery : Query<bool>
    {
        private readonly Driver driver;
        private readonly DriverScope scope;
        private readonly string cssSelector;

        protected internal HasNoCssQuery(Driver driver, DriverScope scope, string cssSelector)
        {
            this.driver = driver;
            this.scope = scope;
            this.cssSelector = cssSelector;
        }

        public object ExpectedResult
        {
            get { return true; }
        }

        public bool Result { get; private set; }

        public TimeSpan Timeout
        {
            get { return scope.IndividualTimeout; }
        }

        public void Run()
        {
            Result = !driver.HasCss(cssSelector, scope);
        }
    }
}