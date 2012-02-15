namespace Coypu.Queries
{
    internal class HasNoCssQuery : CssQuery
    {
        public HasNoCssQuery(Driver driver, DriverScope scope, string cssSelector)
            : base(driver, scope, cssSelector)
        {
        }

        public override bool ExpectedResult
        {
            get { return false; }
        }
    }
}