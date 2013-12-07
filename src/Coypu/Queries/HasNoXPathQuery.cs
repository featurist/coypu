namespace Coypu.Queries
{
    internal class HasNoXPathQuery : DriverScopeQuery<bool>
    {
        private readonly Driver driver;
        private readonly string xpath;
        public override bool ExpectedResult { get { return true; } }


        protected internal HasNoXPathQuery(Driver driver, DriverScope scope, string xpath, Options options)
            : base(scope, options)
        {
            this.driver = driver;
            this.xpath = xpath;
        }

        public override bool Run()
        {
            try
            {
                DriverScope.FindXPath(xpath, Options);
                return true;
            }
            catch (MissingHtmlException)
            {
                return false;
            }
        }
    }
}