namespace Coypu.Queries
{
    internal abstract class XPathQuery : Query<bool>
    {
        private readonly Driver driver;
        private readonly DriverScope scope;
        private readonly string xpath;
        public abstract object ExpectedResult { get; }
        public bool Result { get; private set; }

        protected XPathQuery(Driver driver, DriverScope scope, string xpath)
        {
            this.driver = driver;
            this.scope = scope;
            this.xpath = xpath;
        }

        public void Run()
        {
            Result = driver.HasXPath(xpath, scope) == (bool) ExpectedResult;
        }
    }
}