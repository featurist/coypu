using System;

namespace Coypu.Queries
{
    internal class HasNoXPathQuery : Query<bool>
    {
        private readonly Driver driver;
        private readonly DriverScope scope;
        private readonly string xpath;
        public object ExpectedResult { get { return true; } }
        public bool Result { get; private set; }

        public TimeSpan Timeout
        {
            get { return scope.IndividualTimeout; }
        }

        protected internal HasNoXPathQuery(Driver driver, DriverScope scope, string xpath)
        {
            this.driver = driver;
            this.scope = scope;
            this.xpath = xpath;
        }

        public void Run()
        {
            Result = !driver.HasXPath(xpath, scope);
        }
    }
}