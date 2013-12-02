using System.Text.RegularExpressions;
using NUnit.Framework.Constraints;

namespace Coypu.Matchers
{
    public class HasNoCssMatcher : Constraint
    {
        private readonly string _expectedCss;
        private readonly Options _options;
        private string _actualContent;
        private Regex textPattern;
        private string exactText;

        public HasNoCssMatcher(string expectedCss, Options options)
        {
            _expectedCss = expectedCss;
            _options = options;
        }

        public HasNoCssMatcher(string expectedCss, Regex textPattern, Options options)
            : this(expectedCss, options)
        {
            this.textPattern = textPattern;
        }

        public HasNoCssMatcher(string expectedCss, string exactText, Options options)
            : this(expectedCss, options)
        {
            this.exactText = exactText;
        }

        public override bool Matches(object actual)
        {
            this.actual = actual;
            var scope = ((Scope)actual);

            bool hasNoCss;
            if (exactText != null)
                hasNoCss = scope.HasNoCss(_expectedCss, exactText, _options);
            else if (textPattern != null)
                hasNoCss = scope.HasNoCss(_expectedCss, textPattern, _options);
            else
                hasNoCss = scope.HasNoCss(_expectedCss, _options);

            if (!hasNoCss)
                _actualContent = scope.Find().InnerHTML;

            return hasNoCss;
        }

        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.WriteMessageLine("Expected NOT to find element with css selector: {0}\nin:\n{1} ", _expectedCss, _actualContent);
            if (exactText != null)
                writer.WriteMessageLine("With text: \"" + exactText + "\"");
            if (textPattern != null)
                writer.WriteMessageLine("With text matching: \"" + textPattern + "\"");
        }
    }
}