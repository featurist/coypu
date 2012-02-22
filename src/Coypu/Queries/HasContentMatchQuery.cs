using System.Text.RegularExpressions;

namespace Coypu.Queries
{
    internal class HasContentMatchQuery : Query<bool>
    {
        private readonly Driver driver;
        private readonly DriverScope scope;
        private readonly Regex text;
        public object ExpectedResult { get { return true; } }
        public bool Result { get; private set; }

        protected internal HasContentMatchQuery(Driver driver, DriverScope scope, Regex text)
        {
            this.driver = driver;
            this.scope = scope;
            this.text = text;
        }

        public void Run()
        {
            Result = driver.HasContentMatch(text, scope);
        }
    }
}