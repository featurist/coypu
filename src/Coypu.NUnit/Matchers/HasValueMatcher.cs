using NUnit.Framework.Constraints;

namespace Coypu.NUnit.Matchers
{
    public class HasValueMatcher : Constraint
    {
        private readonly string _expectedContent;
        private readonly Options _options;
        private string _actualContent;

        public HasValueMatcher(string expectedContent, Options options)
        {
            _expectedContent = expectedContent;
            _options = options;
        }

        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            var elementScope = actual as ElementScope;
            var hasValue = elementScope.HasValue(_expectedContent, _options);
            if (!hasValue)
            {
                _actualContent = elementScope.Value;
                hasValue = _actualContent == _expectedContent;
            }
            return new ConstraintResult(this, actual, hasValue);
        }

        public override string Description => $"Expected to find value: {_expectedContent}\nin:\n{_actualContent}";
    }
}