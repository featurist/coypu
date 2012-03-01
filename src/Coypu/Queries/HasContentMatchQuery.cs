using System.Text.RegularExpressions;

namespace Coypu.Queries
{
    internal class HasContentMatchQuery : DriverScopeQuery<bool>
    {
        private readonly Driver driver;
        private readonly Regex text;
        public override object ExpectedResult { get { return true; } }

        protected internal HasContentMatchQuery(Driver driver, DriverScope scope, Regex text) : base(scope)
        {
            this.driver = driver;
            this.text = text;
        }

        public override void Run()
        {
            Result = driver.HasContentMatch(text, DriverScope);
        }
    }
}