namespace Coypu.Queries
{
    internal class HasNoXPathQuery : DriverScopeQuery<bool>
    {
        private readonly Driver driver;
        private readonly string xpath;
        public override object ExpectedResult { get { return true; } }


        protected internal HasNoXPathQuery(Driver driver, DriverScope scope, string xpath) : base(scope)
        {
            this.driver = driver;
            this.xpath = xpath;
        }

        public override void Run()
        {
            Result = !driver.HasXPath(xpath, DriverScope);
        }
    }
}