using System.Text.RegularExpressions;

namespace Coypu.Queries
{
    internal class HasNoContentMatchQuery : DriverScopeQuery<bool>
    {
        private readonly Regex text;
        public override object ExpectedResult => true;

        protected internal HasNoContentMatchQuery(DriverScope scope, Regex text, Options options)
            : base(scope, options)
        {
            this.text = text;
        }

        public override bool Run()
        {
            return !text.IsMatch(Scope.FindElement().Text);
        }    
    }
}