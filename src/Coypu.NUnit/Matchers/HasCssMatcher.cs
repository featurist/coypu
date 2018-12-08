using System.Text.RegularExpressions;
using NUnit.Framework.Constraints;

namespace Coypu.NUnit.Matchers
{
    public class HasCssMatcher : Constraint
    {
        private readonly Regex _textPattern;
        private readonly string _exactText;
        private readonly string _expectedCss;
        private readonly Options _options;
        private string _actualContent;

        public HasCssMatcher(string expectedCss, Options options)
        {
            _expectedCss = expectedCss;
            _options = options;
        }

        public HasCssMatcher(string expectedCss, Regex textPattern, Options options)
            : this(expectedCss, options)
        {
            _textPattern = textPattern;
        }

        public HasCssMatcher(string expectedCss, string exactText, Options options)
            : this(expectedCss, options)
        {
            _exactText = exactText;
        }

        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            var scope = (Scope)actual;

            bool hasCss;
            if (_exactText != null)
                hasCss = scope.FindCss(_expectedCss, _exactText, _options).Exists();
            else if (_textPattern != null)
                hasCss = scope.FindCss(_expectedCss, _textPattern, _options).Exists();
            else
                hasCss = scope.FindCss(_expectedCss, _options).Exists();

            if (!hasCss)
                _actualContent = scope.Now().InnerHTML;

            return new ConstraintResult(this, actual, hasCss);
        }

        public override string Description
        {
            get
            {
                var description = "Expected to find element with css selector: " + _expectedCss + "\n";

                if (_exactText != null)
                    description += "With text: \"" + _exactText + "\"\r\n";
                if (_textPattern != null)
                    description += "With text matching: \"" + _textPattern + "\"\r\n";

                description += "in:\n"+_actualContent+"\r\n";

                return description;
            }
        }
    }
}