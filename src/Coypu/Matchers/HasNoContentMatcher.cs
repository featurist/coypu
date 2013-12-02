using NUnit.Framework.Constraints;

namespace Coypu.Matchers
{
    public class HasNoContentMatcher : Constraint {
        private readonly string _expectedContent;
        private readonly Options _options;
        private string _actualContent;

        public HasNoContentMatcher(string expectedContent, Options options) {
            _expectedContent = expectedContent;
            _options = options;
        }

        public override bool Matches(object actual) {
            this.actual = actual;
            var scope = ((Scope)actual);
            var hasNoContent = scope.HasNoContent(_expectedContent, _options);
            if (!hasNoContent)
            {
                _actualContent = scope.Find().Text;
                hasNoContent = !_actualContent.Contains(_expectedContent);
            }

            return hasNoContent;
        }

        public override void WriteDescriptionTo(MessageWriter writer) {
            writer.WriteMessageLine("Expected NOT to find content: {0}\nin:\n{1}", _expectedContent, _actualContent);
        }
    }
}