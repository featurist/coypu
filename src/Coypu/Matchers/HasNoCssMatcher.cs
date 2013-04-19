using NUnit.Framework.Constraints;

namespace Coypu.Matchers
{
    public class HasNoCssMatcher : Constraint
    {
        private readonly string _expectedCss;
        private readonly Options _options;
        private string _actualContent;

        public HasNoCssMatcher(string expectedCss, Options options)
        {
            _expectedCss = expectedCss;
            _options = options;
        }

        public override bool Matches(object actual)
        {
            this.actual = actual;
            var scope = ((Scope)actual);
            var hasNoCss = scope.HasNoCss(_expectedCss, _options);
            if (!hasNoCss)
                _actualContent = scope.Now().ToString();

            return hasNoCss;
        }

        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.WriteMessageLine("Expected NOT to find element with css selector: {0}\nin:\n{1}", _expectedCss, _actualContent);
        }
    }
}