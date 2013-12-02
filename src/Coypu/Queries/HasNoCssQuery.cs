using System.Text.RegularExpressions;
using Coypu.Finders;

namespace Coypu.Queries
{
    internal class HasNoCssQuery : DriverScopeQuery<bool>
    {
        private readonly Driver driver;
        private readonly string cssSelector;
        private Regex textPattern;

        protected internal HasNoCssQuery(Driver driver, DriverScope scope, string cssSelector, Options options) : base(scope,options)
        {
            this.driver = driver;
            this.cssSelector = cssSelector;
        }

        protected internal HasNoCssQuery(Driver driver, DriverScope scope, string cssSelector, Regex textPattern, Options options)
            : this(driver, scope, cssSelector, options)
        {
            this.textPattern = textPattern;
        }

        protected internal HasNoCssQuery(Driver driver, DriverScope scope, string cssSelector, string text, Options options)
            : this(driver, scope, cssSelector, CssFinder.TextAsRegex(text, options.Exact), options)
        {
        }

        public override bool ExpectedResult
        {
            get { return true; }
        }

        public override bool Run()
        {
            try
            {
                driver.FindCss(cssSelector, DriverScope, textPattern);
                return false;
            }
            catch (MissingHtmlException)
            {
                return true;
            }
        }
    }
}