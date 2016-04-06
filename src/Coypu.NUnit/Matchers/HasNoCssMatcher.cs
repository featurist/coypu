using System.Text.RegularExpressions;
using NUnit.Framework.Constraints;

namespace Coypu.NUnit.Matchers
{
    public class HasNoCssMatcher : Constraint
    {
        private readonly string _expectedCss;
        private readonly Options _options;
        private string _actualContent;
        private readonly Regex _textPattern;
        private readonly string _exactText;

        public HasNoCssMatcher(string expectedCss, Options options)
        {
            _expectedCss = expectedCss;
            _options = options;
        }

        public HasNoCssMatcher(string expectedCss, Regex textPattern, Options options)
            : this(expectedCss, options)
        {
            this._textPattern = textPattern;
        }

        public HasNoCssMatcher(string expectedCss, string exactText, Options options)
            : this(expectedCss, options)
        {
            this._exactText = exactText;
        }

        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            var scope = ((Scope)actual);

            bool hasNoCss;
            if (_exactText != null)
                hasNoCss = scope.FindCss(_expectedCss, _exactText, _options).Missing();
            else if (_textPattern != null)
                hasNoCss = scope.FindCss(_expectedCss, _textPattern, _options).Missing();
            else
                hasNoCss = scope.FindCss(_expectedCss, _options).Missing();

            if (!hasNoCss)
                _actualContent = scope.Now().InnerHTML;

            return new ConstraintResult(this, _actualContent, hasNoCss);
        }

        public override string Description
        {
            get
            {
                var description = $"Expected NOT to find element with css selector: {_expectedCss}\nin:\n{_actualContent}\r\n";

                if (_exactText != null)
                   description += "With text: \"" + _exactText + "\"\r\n";
                if (_textPattern != null)
                    description += "With text matching: \"" + _textPattern + "\"\r\n";

                return description;
            }
        }
    }
}