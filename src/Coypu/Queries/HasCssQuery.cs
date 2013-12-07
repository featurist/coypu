using System.Text.RegularExpressions;
using Coypu.Finders;

namespace Coypu.Queries
{
    internal class HasCssQuery : DriverScopeQuery<bool>
    {
        ElementScope elementScope = null;

        public override bool ExpectedResult
        {
            get { return true; }
        }

        protected internal HasCssQuery(DriverScope scope, string cssSelector, Options options)
            : base(scope, options)
        {
            elementScope = DriverScope.FindCss(cssSelector, Options);
        }

        protected internal HasCssQuery(DriverScope scope, string cssSelector, Regex textPattern, Options options)
            : this(scope, cssSelector, options)
        {
            elementScope = DriverScope.FindCss(cssSelector, textPattern, Options);
        }

        protected internal HasCssQuery(DriverScope scope, string cssSelector, string text, Options options)
            : this(scope, cssSelector, options)
        {
            elementScope = DriverScope.FindCss(cssSelector, text, Options);
        }

        public override bool Run()
        {
            return elementScope.Exists();
        }
    }
}