using System.Text.RegularExpressions;

namespace Coypu.Queries
{
    internal class HasContentMatchQuery : ContentMatchQuery
    {
        public HasContentMatchQuery(Driver driver, DriverScope scope, Regex text)
            : base(driver, scope, text)
        {
        }

        public override object ExpectedResult
        {
            get { return true; }
        }
    }
}