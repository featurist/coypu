using System.Text.RegularExpressions;
using Coypu.Finders;

namespace Coypu.Queries
{
    internal class HasCssQuery : DriverScopeQuery<bool>
    {
        private readonly Regex textPattern;
        private readonly Driver driver;
        private readonly string cssSelector;
        public override bool ExpectedResult { get { return true; } }

        protected internal HasCssQuery(Driver driver, DriverScope scope, string cssSelector, Options options)
            : base(scope, options)
        {
            this.driver = driver;
            this.cssSelector = cssSelector;
        }

        protected internal HasCssQuery(Driver driver, DriverScope scope, string cssSelector, Regex textPattern, Options options)
            : this(driver, scope, cssSelector, options)
        {
            this.textPattern = textPattern;
        }

        protected internal HasCssQuery(Driver driver, DriverScope scope, string cssSelector, string text, Options options)
            : this(driver, scope, cssSelector, CssFinder.ExactTextAsRegex(text, options.Exact), options)
        {
        }

        public override bool Run()
        {
            try
            {
                driver.FindCss(cssSelector, DriverScope, textPattern);
                return true;
            }
            catch (MissingHtmlException)
            {
                return false;
            }
        }
    }
}