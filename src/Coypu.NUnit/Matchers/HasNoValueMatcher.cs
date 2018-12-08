using NUnit.Framework.Constraints;

namespace Coypu.NUnit.Matchers
{
    public class HasNoValueMatcher : Constraint
    {
        private readonly string _expectedContent;
        private readonly Options _options;
        private string _actualContent;

        public HasNoValueMatcher(string expectedContent, Options options)
        {
            _expectedContent = expectedContent;
            _options = options;
        }

        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            var elementScope = actual as ElementScope;
            var hasNoValue = elementScope.HasNoValue(_expectedContent, _options);
            if (!hasNoValue)
                _actualContent = elementScope.Value;
            return new ConstraintResult(this, actual, hasNoValue);
        }

        public override string Description => "Expected NOT to find value: " + _expectedContent + "\nin:\n" + _actualContent;
    }
}