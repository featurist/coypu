using NUnit.Framework.Constraints;

namespace Coypu.Matchers
{
    public class HasCssMatcher : Constraint
    {
        private readonly string _expectedCss;
        private readonly Options _options;
        private string _actualContent;

        public HasCssMatcher(string expectedCss, Options options)
        {
            _expectedCss = expectedCss;
            _options = options;
        }

        public override bool Matches(object actual)
        {
            this.actual = actual;
            var scope = ((Scope)actual);
            var hasCss = scope.HasCss(_expectedCss, _options);
            if (!hasCss)
                _actualContent = scope.Now().ToString();

            return hasCss;
        }

        public override void WriteDescriptionTo(MessageWriter writer)
        {
            writer.WriteMessageLine("Expected to find element with css selector: {0}\nin:\n{1}", _expectedCss, _actualContent);
        }
    }
}