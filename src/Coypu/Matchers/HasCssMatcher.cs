using System.Text.RegularExpressions;
using NUnit.Framework.Constraints;

namespace Coypu.Matchers
{
    public class HasCssMatcher : Constraint
    {
        private readonly Regex textPattern;
        private readonly string exactText;
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
            this.textPattern = textPattern;
        }

        public HasCssMatcher(string expectedCss, string exactText, Options options)
            : this(expectedCss, options)
        {
            this.exactText = exactText;
        }

        public override bool Matches(object actual)
        {
            this.actual = actual;
            var scope = ((Scope)actual);

            bool hasCss;
            if (exactText != null)
                hasCss = scope.HasCss(_expectedCss, exactText, _options);
            else if (textPattern != null)
                hasCss = scope.HasCss(_expectedCss, textPattern, _options);
            else
                hasCss = scope.HasCss(_expectedCss, _options);

            if (!hasCss)
                _actualContent = scope.Find().InnerHTML;

            return hasCss;
        }

        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.WriteMessageLine("Expected to find element with css selector: {0}\n", _expectedCss);
            if (exactText != null)
                writer.WriteMessageLine("With text: \"" + exactText + "\"");
            if (textPattern != null)
                writer.WriteMessageLine("With text matching: \"" + textPattern + "\"");

            writer.WriteLine("in:\n{0}",_actualContent);
        }
    }
}