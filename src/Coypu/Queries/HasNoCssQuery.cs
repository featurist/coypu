using System.Text.RegularExpressions;
using Coypu.Finders;

namespace Coypu.Queries
{
    internal class HasNoCssQuery : DriverScopeQuery<bool>
    {
        private readonly string text;
        private readonly string cssSelector;
        private readonly Regex textPattern;

        protected internal HasNoCssQuery(DriverScope scope, string cssSelector, Options options) : base(scope,options)
        {
            this.cssSelector = cssSelector;
        }

        protected internal HasNoCssQuery(DriverScope scope, string cssSelector, Regex textPattern, Options options)
            : this(scope, cssSelector, options)
        {
            this.textPattern = textPattern;
        }

        protected internal HasNoCssQuery(DriverScope scope, string cssSelector, string text, Options options)
            : this(scope, cssSelector, options)
        {
            this.text = text;
        }

        public override bool ExpectedResult
        {
            get { return true; }
        }

        public override bool Run()
        {
            try
            {
                if (textPattern != null)
                    DriverScope.FindCss(cssSelector, textPattern, Options);
                else if (text != null)
                    DriverScope.FindCss(cssSelector, text, Options);
                else
                    DriverScope.FindCss(cssSelector, Options);
                return false;
            }
            catch (MissingHtmlException)
            {
                return true;
            }
        }
    }
}