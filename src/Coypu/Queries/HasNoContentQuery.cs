namespace Coypu.Queries
{
    internal class HasNoContentQuery : ContentQuery
    {
        public HasNoContentQuery(Driver driver, DriverScope scope, string text)
            : base(driver, scope, text)
        {
        }

        public override bool ExpectedResult
        {
            get { return false; }
        }
    }
}