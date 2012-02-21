namespace Coypu.Queries
{
    internal class HasContentQuery : ContentQuery
    {
        public HasContentQuery(Driver driver, DriverScope scope, string text)
            : base(driver, scope, text)
        {
        }

        public override object ExpectedResult
        {
            get { return true; }
        }
    }
}