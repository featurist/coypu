using System.Text.RegularExpressions;

namespace Coypu.Queries
{
    internal class HasContentMatchQuery : DriverScopeQuery<bool>
    {
        private readonly Regex text;
        public override object ExpectedResult => true;

        protected internal HasContentMatchQuery(DriverScope scope, Regex text, Options options) : base(scope,options)
        {
            this.text = text;
        }

        public override bool Run()
        {
            return text.IsMatch(Scope.FindElement().Text);
        }
    }
}