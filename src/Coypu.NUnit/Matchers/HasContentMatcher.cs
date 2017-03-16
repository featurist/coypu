using System.Text.RegularExpressions;
using NUnit.Framework.Constraints;

namespace Coypu.NUnit.Matchers
{
    public class HasContentMatcher : Constraint
    {
        private readonly string _expectedContent;
        private readonly Options _options;
        private string _actualContent;

        public HasContentMatcher(string expectedContent, Options options)
        {
            _expectedContent = expectedContent;
            _options = options;
        }

        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            var scope = actual as DriverScope;
            var hasContent = scope.HasContent(_expectedContent, _options);
            if (!hasContent)
            {
                _actualContent = scope.Text;
                hasContent = _actualContent.Contains(_expectedContent);
            }
            return new ConstraintResult(this, actual, hasContent);
        }

        public override string Description {
            get {
                return "Expected to find content: " + _expectedContent + "\nin:\n" + _actualContent;
            }
        }
    }

    public class HasContentMatchMatcher : Constraint
    {
        private readonly Regex _expectedContent;
        private readonly Options _options;
        private string _actualContent;

        public HasContentMatchMatcher(Regex expectedContent, Options options)
        {
            _expectedContent = expectedContent;
            _options = options;
        }

        public override ConstraintResult ApplyTo<TActual>(TActual actual)
        {
            var scope = actual as DriverScope;
            var hasContent = scope.HasContentMatch(_expectedContent, _options);
            if (!hasContent)
            {
                _actualContent = scope.Text;
                hasContent = _expectedContent.IsMatch(_actualContent);
            }
            return new ConstraintResult(this, actual, hasContent);
        }

        public override string Description
        {
            get
            {
                return "Expected to find content: " + _expectedContent + "\nin:\n" + _actualContent;
            }
        }
    }
}