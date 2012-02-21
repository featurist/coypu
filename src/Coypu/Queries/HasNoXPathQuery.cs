namespace Coypu.Queries
{
    internal class HasNoXPathQuery : XPathQuery
    {
        public HasNoXPathQuery(Driver driver, DriverScope scope, string xpath)
            : base(driver, scope, xpath)
        {
        }

        public override object ExpectedResult
        {
            get { return false; }
        }
    }
}