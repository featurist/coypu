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

        internal CssFinder(Driver driver, string locator, DriverScope scope, bool exact, string text)
            : this(driver, locator, scope, ExactTextAsRegex(text, exact)) { }

        internal override ElementFound Find()
        {
            return Driver.FindCss(Locator, Scope, textPattern);
        }

        public static Regex ExactTextAsRegex(string textEquals, bool exact)
        {
            Regex textMatches = null;
            if (textEquals != null)
            {
                var escapedText = Regex.Escape(textEquals);
                if (exact)
                    escapedText = string.Format("^{0}$", escapedText);

                textMatches = new Regex(escapedText, RegexOptions.Multiline);
            }

            return textMatches;
        }
    }
}