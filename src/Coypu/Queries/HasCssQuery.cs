namespace Coypu.Queries
{
    internal class HasCssQuery : CssQuery
    {
        public HasCssQuery(Driver driver, DriverScope scope, string cssSelector)
            : base(driver, scope, cssSelector)
        {
        }

        public override object ExpectedResult
        {
            get { return true; }
        }
    }
}