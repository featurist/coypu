using NUnit.Framework.Constraints;

namespace Coypu.Matchers
{
    public class HasContentMatcher : Constraint {
        private readonly string _expectedContent;
        private readonly Options _options;
        private string _actualContent;

        public HasContentMatcher(string expectedContent, Options options) {
            _expectedContent = expectedContent;
            _options = options;
        }

        public override bool Matches(object actual) {
            this.actual = actual;
            var scope = ((Scope)actual);
            var hasContent = scope.HasContent(_expectedContent, _options);
            if (!hasContent)
                _actualContent = scope.Now().Text;
            return hasContent;
        }

        public override void WriteDescriptionTo(MessageWriter writer) {
            writer.WriteMessageLine("Expected to find content: {0}\nin:\n{1}", _expectedContent, _actualContent);
        }
    }
}