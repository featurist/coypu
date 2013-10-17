using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using NUnit.Framework.Constraints;

namespace Coypu.Matchers
{
    public class HasAllCssInOrderMatcher : Constraint
    {
        private readonly Regex[] textPattern;
        private readonly string[] exactText;
        private readonly string _expectedCss;
        private readonly Options _options;
        private string _actualContent;

        public HasAllCssInOrderMatcher(string expectedCss, Options options)
        {
            _expectedCss = expectedCss;
            _options = options;
        }

        public HasAllCssInOrderMatcher(string expectedCss, IEnumerable<Regex> textPattern, Options options)
            : this(expectedCss, options)
        {
            this.textPattern = textPattern.ToArray();
        }

        public HasAllCssInOrderMatcher(string expectedCss, IEnumerable<string> exactText, Options options)
            : this(expectedCss, options)
        {
            this.exactText = exactText.ToArray();
        }

        public override bool Matches(object actual)
        {
            this.actual = actual;
            var scope = ((Scope)actual);

            _actualContent = string.Format("[{0}]", string.Join(",", scope.FindAllCss(_expectedCss).Select(t => t.Text).ToArray()));

            var hasCss = true;
            if (exactText != null)
                try
                {
                    scope.FindAllCss(_expectedCss, e =>
                        {
                            var textInScope = e.Select(t => t.Text).ToArray();
                            return textInScope.Where((t, i) => t == exactText[i]).Count() == exactText.Count();

                        }, _options);
                }
                catch (MissingHtmlException)
                {
                    hasCss = false;
                }
            else if (textPattern != null)
                try
                {
                    scope.FindAllCss(_expectedCss, e =>
                        {
                            var textInScope = e.Select(t => t.Text).ToArray();
                            return textInScope.Where((t, i) => textPattern[i].IsMatch(t)).Count() == textPattern.Count();

                        }, _options);
                }
                catch (MissingHtmlException)
                {
                    hasCss = false;
                }


            if (!hasCss)
                _actualContent = string.Format("[{0}]", string.Join(",",scope.FindAllCss(_expectedCss).Select(t => t.Text).ToArray()));

            return hasCss;
        }
        public override void WriteActualValueTo(MessageWriter writer)
        {
            writer.WriteMessageLine(string.Format("\r\n{0}", _actualContent));
        }

        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.WriteMessageLine("Expected to find elements from css selector: {0}\r\nContaining only:\r\n", _expectedCss);
            if (exactText != null)
                writer.WriteMessageLine(string.Format("[{0}]", string.Join(",", exactText)));
            if (textPattern != null)
                writer.WriteMessageLine(string.Format("[{0}]", string.Join(",", textPattern.Select(p => p.ToString()).ToArray())));
            
        }

    }
}