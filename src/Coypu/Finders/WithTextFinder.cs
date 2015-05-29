using System.Text.RegularExpressions;

namespace Coypu.Finders
{
    internal abstract class WithTextFinder : ElementFinder
    {
        private readonly Regex textPattern;
        private readonly string text;

        internal WithTextFinder(Driver driver, string locator, DriverScope scope, Options options)
            : base(driver, locator, scope, options)
        {
        }

        internal WithTextFinder(Driver driver, string locator, DriverScope scope, Options options, Regex textPattern)
            : this(driver, locator, scope, options)
        {
            this.textPattern = textPattern;
        }

        internal WithTextFinder(Driver driver, string locator, DriverScope scope, Options options, string text)
            : this(driver, locator, scope, options)
        {
            this.text = text;
        }

        public Regex TextPattern(bool exact)
        {
            if (text != null)
                return TextAsRegex(text, exact);

            if (textPattern != null)
                return textPattern;

            return null;
        }

        internal override string QueryDescription
        {
            get
            {
                var queryDesciption = SelectorType + ": " + Locator;
                if (textPattern != null)
                    queryDesciption += " with text matching /" + text ?? textPattern + "/";

                return queryDesciption;
            }
        }

        protected abstract string SelectorType { get; }

        public static Regex TextAsRegex(string textEquals, bool exact)
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