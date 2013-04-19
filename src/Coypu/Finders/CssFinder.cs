using System.Text.RegularExpressions;

namespace Coypu.Finders
{
    internal class CssFinder : ElementFinder
    {
        private readonly Regex textPattern;

        internal CssFinder(Driver driver, string locator, DriverScope scope)
            : base(driver, locator, scope)
        {
        }

        internal CssFinder(Driver driver, string locator, DriverScope scope, Regex textPattern)
            : this(driver, locator, scope)
        {
            this.textPattern = textPattern;
        }

        internal CssFinder(Driver driver, string locator, DriverScope scope, string text)
            : this(driver, locator, scope, ExactTextAsRegex(text)) { }

        internal override ElementFound Find()
        {
            return Driver.FindCss(Locator, Scope, textPattern);
        }

        public static Regex ExactTextAsRegex(string textEquals)
        {
            Regex textMatches = null;
            if (textEquals != null)
                textMatches = new Regex(string.Format("^{0}$", textEquals), RegexOptions.Multiline);

            return textMatches;
        }
    }
}