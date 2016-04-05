using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework.Constraints;

namespace Coypu.NUnit.Matchers
{
    public class HasAllCssInOrderMatcher : Constraint
    {
        private readonly Regex[] _textPattern;
        private readonly string[] _exactText;
        private readonly string _expectedCss;
        private readonly Options _options;

        public HasAllCssInOrderMatcher(string expectedCss, Options options)
        {
            _expectedCss = expectedCss;
            _options = options;
        }

        public HasAllCssInOrderMatcher(string expectedCss, IEnumerable<Regex> textPattern, Options options)
            : this(expectedCss, options)
        {
            this._textPattern = textPattern.ToArray();
        }

        public HasAllCssInOrderMatcher(string expectedCss, IEnumerable<string> exactText, Options options)
            : this(expectedCss, options)
        {
            this._exactText = exactText.ToArray();
        }

        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            var scope = (Scope)actual;

            var actualContent = $"[{string.Join(",", scope.FindAllCss(_expectedCss).Select(t => t.Text).ToArray())}]";

            var hasCss = true;
            if (_exactText != null)
                try
                {
                    scope.FindAllCss(_expectedCss, e =>
                    {
                        var textInScope = e.Select(t => t.Text).ToArray();
                        return textInScope.Where((t, i) => t == _exactText[i]).Count() == _exactText.Count();

                    }, _options);
                }
                catch (MissingHtmlException)
                {
                    hasCss = false;
                }
            else if (_textPattern != null)
                try
                {
                    scope.FindAllCss(_expectedCss, e =>
                    {
                        var textInScope = e.Select(t => t.Text).ToArray();
                        return textInScope.Where((t, i) => _textPattern[i].IsMatch(t)).Count() == _textPattern.Count();

                    }, _options);
                }
                catch (MissingHtmlException)
                {
                    hasCss = false;
                }


            if (!hasCss)
                actualContent = $"[{string.Join(",", scope.FindAllCss(_expectedCss).Select(t => t.Text).ToArray())}]";

            return new ConstraintResult(this, actualContent, hasCss);
        }

        public override string Description
        {
            get
            {
                var description = $"Expected to find elements from css selector: {_expectedCss}\r\nContaining only:\r\n";

                if (_exactText != null)
                    description += $"[{string.Join(",", _exactText)}]\r\n";
                if (_textPattern != null)
                    description += $"[{string.Join(",", _textPattern.Select(p => p.ToString()).ToArray())}]\r\n";

                return description;
            }
        }
    }
}