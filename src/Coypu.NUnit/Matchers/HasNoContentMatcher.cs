using NUnit.Framework.Constraints;

namespace Coypu.NUnit.Matchers
{
    public class HasNoContentMatcher : Constraint {
        private readonly string _expectedContent;
        private readonly Options _options;
        private string _actualContent;

        public HasNoContentMatcher(string expectedContent, Options options) {
            _expectedContent = expectedContent;
            _options = options;
        }

        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            var scope = (Scope)actual;
            var hasNoContent = scope.HasNoContent(_expectedContent, _options);
            if (!hasNoContent)
            {
                _actualContent = scope.Now().Text;
                hasNoContent = !_actualContent.Contains(_expectedContent);
            }

            return new ConstraintResult(this, actual, hasNoContent);
        }

        public override string Description => $"Expected NOT to find content: {_expectedContent}\nin:\n{_actualContent}";
    }
}