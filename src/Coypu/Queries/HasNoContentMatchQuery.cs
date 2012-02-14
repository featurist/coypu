using System.Text.RegularExpressions;

namespace Coypu.Queries
{
    internal class HasNoContentMatchQuery : ContentMatchQuery
    {
        public HasNoContentMatchQuery(Driver driver, DriverScope scope, Regex text)
            : base(driver, scope, text)
        {
        }

        public override bool ExpectedResult
        {
            get { return false; }
        }
    }
}