using System.Text.RegularExpressions;

namespace Coypu.Queries
{
    internal class HasContentMatchQuery : DriverScopeQuery<bool>
    {
        private readonly Driver driver;
        private readonly Regex text;
        public override bool ExpectedResult { get { return true; } }

        protected internal HasContentMatchQuery(Driver driver, DriverScope scope, Regex text, Options options) : base(scope,options)
        {
            this.driver = driver;
            this.text = text;
        }

        public override bool Run()
        {
            return driver.HasContentMatch(text, DriverScope);
        }
    }
}